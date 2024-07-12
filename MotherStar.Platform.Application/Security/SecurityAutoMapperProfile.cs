using AutoMapper;
using MotherStar.Platform.Application.Contracts.Security;
using MotherStar.Platform.Domain.Security.Models;

namespace MotherStar.Platform.Application.Security
{
    public class SecurityAutoMapperProfile : Profile
    {
        public SecurityAutoMapperProfile()
        {
            // User -> AuthenticateResponse
            CreateMap<User, AuthenticateResponse>();

            // RegisterRequest -> User
            CreateMap<RegisterRequest, User>();

            // UpdateRequest -> User
            CreateMap<UpdateRequest, User>()
                .ForAllMembers(x => x.Condition(
                    (src, dest, prop) =>
                    {
                        // ignore null & empty string properties
                        if (prop == null) return false;
                        if (prop.GetType() == typeof(string) && string.IsNullOrEmpty((string)prop)) return false;

                        return true;
                    }
                ));
        }
    }
}