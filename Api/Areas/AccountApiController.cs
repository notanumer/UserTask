using Core;
using Core.Dto;
using Logic.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Areas
{
    [NamespaceRoutingConvention]
    public class AccountApiController : ControllerBase
    {
        private readonly IUserRepository _repository;
        private readonly IJwtGenerator _jwtGenerator;

        public AccountApiController(IUserRepository repository, IJwtGenerator jwtGenerator)
        {
            _repository = repository;
            _jwtGenerator = jwtGenerator;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var validateErorr = ValidateModel();
            if (validateErorr != null)
            {
                return validateErorr;
            }

            var (isSuccess, erorr) = await _repository.TryAddUser(request);
            if (!isSuccess)
                return erorr;

            return Ok();
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var validateErorr = ValidateModel();
            if (validateErorr != null)
            {
                return validateErorr;
            }

            var (user, error) = await _repository.TryLogin(request);
            if (error != null)
            {
                return error;
            }
            var loginDate = DateTime.Now;
            _repository.SetLoginDate(loginDate, request.Phone);
            var tokenString = _jwtGenerator.GenerateJwt(user);
            return Ok(new { token = tokenString });
        }

        [Authorize]
        [HttpGet]
        [Route("get-my-info")]
        public IActionResult GetUserInfo()
        {
            var userInfo = User.Claims.Select(x => x.Value).ToList();
            return Ok(new { info = userInfo });
        }

        [Authorize]
        [HttpPost]
        [Route("logout")]
        public IActionResult Logout()
        {
            Response.Headers.Remove("Authorization");
            return Ok();
        }


        private ErrorResponse ValidateModel()
        {
            var errorResponse = new ErrorResponse();
            errorResponse.Code = "400";
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.Select(x => x.Errors).FirstOrDefault();
                foreach (var error in errors)
                {
                    errorResponse.Message = error.ErrorMessage;
                    return errorResponse;
                }
            }
            return null;
        }
    }
}
