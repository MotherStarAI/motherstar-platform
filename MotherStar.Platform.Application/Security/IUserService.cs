using MotherStar.Platform.Application.Contracts.Security;
using MotherStar.Platform.Domain.Security.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MotherStar.Platform.Application.Security
{
    public interface IUserService
    {
        Task<AuthenticateResponse> Authenticate(AuthenticateRequest model);
        Task Delete(int id);
        Task<IEnumerable<User>> GetAll();
        Task<User> GetById(int id);
        Task Register(RegisterRequest model);
        Task Update(int id, UpdateRequest model);
    }
}