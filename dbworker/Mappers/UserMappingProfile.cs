using AutoMapper;
using dbworker.Data.EF;
using Stkpnt.Contracts;

namespace dbworker.Mappers
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<ApplicationUserAdd, User>()
              .ForMember(i => i.Guid, f => f.MapFrom(o => o.Id));
            CreateMap<User, ApplicationUserAdd>()
              .ForMember(i => i.Id, f => f.MapFrom(o => o.Guid));

            //.ForMember(i => i.Id, f => f.MapFrom(o => o.Id));

            /*
            CreateMap<ApplicationUserAdd, User>()
              .ForMember(i => i.Guid, f => f.MapFrom(o => o.Id));
            CreateMap<User, ApplicationUserAdd>()
              .ForMember(i => i.Id, f => f.MapFrom(o => o.Guid));
            */
        }
    }
}
