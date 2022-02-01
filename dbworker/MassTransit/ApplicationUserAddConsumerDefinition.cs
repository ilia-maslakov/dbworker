using MassTransit;
using Stkpnt.Contracts;
using MassTransit.Definition;
using MassTransit.ConsumeConfigurators;
using GreenPipes;

namespace MassTransit
{

    public class ApplicationUserAddConsumerDefinition : ConsumerDefinition<ApplicationUserAddConsumer>
    {
        public ApplicationUserAddConsumerDefinition()
        {
            EndpointName = Constants.QueueNameDbWorker;
        }

        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<ApplicationUserAddConsumer> consumerConfigurator)
        {
            endpointConfigurator.UseRetry(x => x.Intervals(100, 500, 1000));
        }

    }
}


