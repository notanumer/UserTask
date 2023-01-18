using AutoMapper;
using Core;
using Core.Dto;
using Core.Entities;
using Logic.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Logic
{
    public class AccountManager : IUserRepository, IJwtGenerator
    {
        private readonly Context _usersRepo;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;

        public AccountManager(Context usersRepo, IMapper mapper, IConfiguration config)
        {
            _usersRepo = usersRepo;
            _mapper = mapper;
            _config = config;
        }

        public string GenerateJwt(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtAuth:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = GetClaims(user);
            var token = new JwtSecurityToken(
              claims: claims,
              expires: DateTime.Now.AddMinutes(120),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public List<Claim> GetClaims(User user)
        {
            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.MobilePhone, user.Phone),
                    new Claim("FIO", user.FIO),
                    new Claim(ClaimTypes.UserData, user.LastLogin.ToShortDateString())
                };

            return claims;
        }

        public bool SetLoginDate(DateTime date, string phone)
        {
            var user = _usersRepo.Users.First(x => x.Phone == phone);
            user.LastLogin = date;
            _usersRepo.Users.Update(user);
            _usersRepo.SaveChanges();
            return true;
        }

        public async Task<(bool,ErrorResponse?)> TryAddUser(RegisterRequest request)
        {
            var user = _mapper.Map<User>(request);
            var isEmailExsist = await _usersRepo.Users.AnyAsync(us => us.Email == request.Email);
            if (isEmailExsist)
                return (false,new ErrorResponse() 
                {
                    Code = "400",
                    Message = "Пользователь с данной эл.почтой уже зарегестрирован"
                });

            var isPhoneExsist = await _usersRepo.Users.AnyAsync(us => us.Phone == request.Phone);
            if (isPhoneExsist)
                return (false, new ErrorResponse()
                {
                    Code = "400",
                    Message = "Пользователь с данным номером телефона уже зарегестрирован"
                });

            await _usersRepo.AddAsync(user);
            _usersRepo.SaveChanges();
            return (true, null);
        }

        public async Task<(User, ErrorResponse?)> TryLogin(LoginRequest request)
        {
            var user = await _usersRepo.Users
                .FirstOrDefaultAsync(u => u.Phone == request.Phone && u.Password == request.Password);
            if (user == null)
            {
                return (null, new ErrorResponse { Code = "400", Message = "Пользователь не найден" });
            }

            return (user, null);
        }
    }
}
