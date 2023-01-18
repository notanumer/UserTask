using Microsoft.AspNetCore.Mvc;

namespace Core.Dto
{
    public class ErrorResponse : IActionResult
    {
        public string Code { get; set; }
        public string Message { get; set; }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            var objectResult = new ObjectResult(Message)
            {
                StatusCode = int.Parse(Code)
            };

            await objectResult.ExecuteResultAsync(context);
        }
    }
}
