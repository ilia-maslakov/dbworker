using AutoMapper;
using dbworker.Data.EF;
using Stkpnt.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dbworker
{

    public class ConfigureMapper : Profile
    {
        public ConfigureMapper()
        {
            CreateMap<ApplicationUserAdd, User>()
              .ForMember(i => i.Guid, f => f.MapFrom(o => o.Id));
        }

    }
}
