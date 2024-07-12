using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using BCryptNet = BCrypt.Net.BCrypt;
using MotherStar.Platform.Application.Contracts.Security;
using MotherStar.Platform.Domain.Security.Models;
using RCommon;
using RCommon.Persistence.Crud;
using System.Threading.Tasks;

namespace MotherStar.Platform.Application.Security
{


    public class UserService : IUserService
    {
        private readonly IGraphRepository<User> _userRepository;
        private IJwtUtils _jwtUtils;
        private readonly IMapper _mapper;

        public UserService(
            IGraphRepository<User> userRepository,
            IJwtUtils jwtUtils,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _jwtUtils = jwtUtils;
            _mapper = mapper;
        }

        public async Task<AuthenticateResponse> Authenticate(AuthenticateRequest model)
        {
            var user = await _userRepository.FindSingleOrDefaultAsync(x => x.Username == model.Username);

            // validate
            if (user == null || !BCryptNet.Verify(model.Password, user.PasswordHash))
                throw new GeneralException("Username or password is incorrect");

            // authentication successful
            var response = _mapper.Map<AuthenticateResponse>(user);
            response.Token = _jwtUtils.GenerateToken(user);
            return response;
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await _userRepository.FindAsync(x => true);
        }

        public async Task<User> GetById(int id)
        {
            return await GetUser(id);
        }

        public async Task Register(RegisterRequest model)
        {
            // validate
            if (_userRepository.Any(x => x.Username == model.Username))
                throw new GeneralException("Username '" + model.Username + "' is already taken");

            // map model to new user object
            var user = _mapper.Map<User>(model);

            // hash password
            user.PasswordHash = BCryptNet.HashPassword(model.Password);

            // save user
            await _userRepository.AddAsync(user);
        }

        public async Task Update(int id, UpdateRequest model)
        {
            var user = await GetUser(id);

            // validate
            if (model.Username != user.Username && _userRepository.Any(x => x.Username == model.Username))
                throw new GeneralException("Username '" + model.Username + "' is already taken");

            // hash password if it was entered
            if (!string.IsNullOrEmpty(model.Password))
                user.PasswordHash = BCryptNet.HashPassword(model.Password);

            // copy model to user and save
            _mapper.Map(model, user);
            await _userRepository.UpdateAsync(user);
        }

        public async Task Delete(int id)
        {
            var user = await GetUser(id);
            await _userRepository.DeleteAsync(user);
        }

        // helper methods

        private async Task<User> GetUser(int id)
        {
            var user = await _userRepository.FindAsync(id);
            if (user == null) throw new KeyNotFoundException("User not found");
            return user;
        }
    }
}
