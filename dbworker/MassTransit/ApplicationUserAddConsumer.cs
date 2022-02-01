using MassTransit;
using System;
using System.Threading.Tasks;
using Stkpnt.Contracts;
using Microsoft.Extensions.Logging;
using dbworker.Data.EF;
using AutoMapper;

namespace MassTransit
{
    public class ApplicationUserAddConsumer : IConsumer<ApplicationUserAdd>
    {
        private readonly ILogger<ApplicationUserAddConsumer> _logger;
        private readonly DBworkerContext _context;
        private readonly IMapper _mapper;

        public ApplicationUserAddConsumer(ILogger<ApplicationUserAddConsumer> logger, DBworkerContext context, IMapper mapper)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
        }
        public Task Consume(ConsumeContext<ApplicationUserAdd> context)
        {
            _logger.LogError($"{context.Message}");
            var r = context.Message;
            //var u = _mapper.Map<User>(r); //надо разобраться почему Exeption

            var u = new User { Name = r.Name, Surname = r.Surname, Patronymic = r.Patronymic, Email = r.Surname };
            _context.User.Add(u);
            _context.SaveChanges();
            return Task.CompletedTask;
        }
    }
}
