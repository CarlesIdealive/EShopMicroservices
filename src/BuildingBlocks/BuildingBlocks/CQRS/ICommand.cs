using MediatR;

namespace BuildingBlocks.CQRS
{
    // ICommand que no produce una respuesta
    public interface ICommand : ICommand<Unit>
    {
    }

    // ICommand que produce una respuesta
    public interface ICommand<out TResponse> : IRequest<TResponse>
    {
    }


}
