using FluentValidation;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Subscription.Application.Abstractions.Handlers
{
    public abstract class BaseHandler<TInput, TOutput>
        where TInput : IInput
        where TOutput : class
    {
        public readonly ILogger<TInput> _logger;
        public readonly AbstractValidator<TInput> _validator;

        public BaseHandler(ILogger<TInput> logger, AbstractValidator<TInput> validator)
        {
            _logger = logger;
            _validator = validator;
        }

        public Output<TOutput> Response { get; private set; } = new();
        public Guid CorrelationId { get; private set; }
        public void LogInformation(string message) => _logger.LogInformation(string.Concat(CorrelationId,"->",message));

        public abstract Task Handle(TInput input, CancellationToken cancellationToken);

        public async Task<Output<TOutput>> HandleExecutionAsync(TInput input, CancellationToken cancellationToken)
        {

            CorrelationId = input.CorrelationId;

            LogInformation(string.Concat(CorrelationId, " ", "Process started ", " ", JsonConvert.SerializeObject(input)));


            try
            {
                var validation = _validator.Validate(input);

                if (!validation.IsValid)
                {
                    Response.AddMessages(validation.Errors.Select(item => item.ErrorMessage).ToList());
                    return Response;
                }

                LogInformation("starting execution");
                await Handle(input, cancellationToken);
                LogInformation("end execution");
                return Response;
            }catch(Exception ex)
            {
                LogInformation("Unknow error ocurred");
                LogInformation(ex.Message);
                Response.AddMessage("Unknow error ocurred");
                return Response;
            }
        }
    }
}
