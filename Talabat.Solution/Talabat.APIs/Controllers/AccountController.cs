using AutoMapper;
using AutoMapper.Configuration.Annotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Talabat.APIs.DTOs.IdentityDtos;
using Talabat.APIs.Error;
using Talabat.APIs.Extentions;
using Talabat.Core.Models.Identity;
using Talabat.Core.Services;

namespace Talabat.APIs.Controllers
{
    public class AccountController : APIBaseController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AccountController(UserManager<AppUser> userManager 
            , SignInManager<AppUser> signInManager
            , ITokenService tokenService,
            IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _mapper = mapper;
        }
        // Register
        [HttpPost("register")] // /api/account/register
        public async Task<ActionResult<UserDto>> Register(RegisterDto register)
        {
            // validate Email duplicate
            if(CheckEmailExists(register.Email).Result)
                return BadRequest(new ApiResponse(400, "Email Already Exists"));

            AppUser user = new AppUser()
            {
                Email = register.Email,
                DisplayName = register.DisplayName,
                PhoneNumber = register.PhoneNumber,
                UserName = register.Email.Split("@")[0]
            };
            var result = await _userManager.CreateAsync(user , register.Password);
            if (!result.Succeeded)
                return BadRequest(new ApiResponse(400));
            var userDto = new UserDto()
            {
                Email = user.Email,
                DisplayName = user.DisplayName,
                Token = await _tokenService.CreateTokenAsync(user, _userManager)
        };
            return Ok(userDto);
        }

        // Login
        [HttpPost("login")] // GET /api/account/login
        public async Task<ActionResult<UserDto>> Login(LoginDto login)
        {
            var user = await _userManager.FindByEmailAsync(login.Email);
            if(user is null) return Unauthorized(new ApiResponse(401));
            var result = await _signInManager.CheckPasswordSignInAsync(user, login.Password , false);
            if (!result.Succeeded) return Unauthorized(new ApiResponse(401)); // loogin faild
            return Ok(new UserDto() { DisplayName = user.DisplayName , 
                                      Email = user.Email , 
                                      Token = await _tokenService.CreateTokenAsync(user, _userManager)
                                    });

        }

        // Get Current User
        [Authorize]
        [HttpGet("getcurrentuser")] // GET /api/account/getcurrentuser
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            // find email of current user [user claims]
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(email);
            return Ok(new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _tokenService.CreateTokenAsync(user, _userManager)
            }); ;
        }

        // Get Current User Address
        [Authorize , HttpGet("userAddress")] // GET /api/account/userAddress
        public async Task<ActionResult<AddressDto>> GetCurrentUserAddress()
        {
            //var Email = User.FindFirstValue(ClaimTypes.Email); 
            //var user = await _userManager.FindByEmailAsync(Email);
            var user = await _userManager.GetUserByEmailWithAddressAsync(User);
            var addressDto = _mapper.Map<Address, AddressDto>(user.Address);
            return Ok(addressDto);
        }

        // Update Current User Address
        [Authorize , HttpPut("userAddress")] // PUT /api/account/userAddress
        public async Task<ActionResult<AddressDto>> UpdateUserAddress(AddressDto addressDto)
        {
            var user = await _userManager.GetUserByEmailWithAddressAsync(User);
            var updatedAddress = _mapper.Map<Address>(addressDto);
            updatedAddress.Id = user.Address.Id;
            user.Address = updatedAddress;
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded) return BadRequest(new ApiResponse(400));
            return Ok(addressDto);
        }

        // Valide Email Duplicate
        [HttpGet("emailExists")]
        public async Task<bool> CheckEmailExists(string Email) 
            => await _userManager.FindByEmailAsync(Email) is not null;

    }
}
