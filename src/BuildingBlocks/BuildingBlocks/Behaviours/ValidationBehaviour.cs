using BuildingBlocks.CQRS;
using FluentValidation;
using MediatR;


namespace BuildingBlocks.Behaviours
{
    public class ValidationBehaviour<TRequest, TResponse>
        (IEnumerable<IValidator<TRequest>> validators)
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : ICommand<TResponse>    //Solo Funciona con comandos - Create, Update
        where TResponse : notnull
    {

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var context = new ValidationContext<TRequest>(request);
            var validationResult = await Task.WhenAll(
                validators.Select(v => v.ValidateAsync(context, cancellationToken))
            );
            var validationFailures = validationResult
                .Where(r => r.Errors.Any())
                .SelectMany(r => r.Errors)
                .ToList();
            if (validationFailures.Any())
            {
                throw new ValidationException(validationFailures);
            }

            return await next();
        }


    }


}
