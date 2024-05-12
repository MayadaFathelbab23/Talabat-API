
namespace Talabat.APIs.Error
{
    public class ApiResponse
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }

        public ApiResponse(int code , string? message = null )
        {
            StatusCode = code ;
            Message = message ?? SetDefaultMessageToStatuseCode(code);
        }

        private string? SetDefaultMessageToStatuseCode(int code)
        {
            return code switch // switch expression
            {
                400 => "A Bad Request . You have made" ,
                401 => "Authorized , you are not",
                404 => "Resource was not found",
                500 => "Errors are the path to the darke side. Errors lead to anger. Anger leads to hate. Hate leads to career change",
                _=> null
            };
    }
    }
}
