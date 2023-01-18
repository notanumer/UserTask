using Core.Dto;
using Core.Entities;

namespace Logic.Interfaces
{
    public interface IUserRepository
    {
        Task<(bool, ErrorResponse?)> TryAddUser(RegisterRequest request);
        Task<(User, ErrorResponse?)> TryLogin(LoginRequest request);
        bool SetLoginDate(DateTime date, string phone);
    }
}
