using MediatR;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Ordering.Infrastructure.Data.Inerceptors
{
    public class DispatchDomainEventInterceptor(IMediator mediator) 
        : SaveChangesInterceptor
    {
        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            DispatchDomainEvents(eventData.Context).GetAwaiter().GetResult();
            return base.SavingChanges(eventData, result);
        }


        public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            await DispatchDomainEvents(eventData.Context);
            return await base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        public async Task DispatchDomainEvents(DbContext? context)
        {
            if (context == null) return;
            //Obtiene los agregados que tienen eventos de dominio pendientes
            //a través de ChangeTracker y busca en las entidades que implementan IAggregate
            // y que tienen eventos de dominio en su colección DomainEvents.
            var aggregates = context.ChangeTracker
                .Entries<IAggregate>()
                .Where(e => e.Entity.DomainEvents.Any())
                .Select(e => e.Entity);

            //Obtiene todos los eventos de dominio de los agregados como una lista. List<IDomainEvents>
            //Aggregates tiene un tipo IEnumerable<IAggregate> que es una interfaz que define un agregado en DDD.
            var domainEvents = aggregates
                .SelectMany(a => a.DomainEvents)
                .ToList();

            //Limpiamos los enventos antes de despacharlos para evitar que se envíen múltiples veces.
            //Porque ya los tenemos almacenados en domainEvents.
            aggregates
                .ToList()
                .ForEach(a => a.ClearDomainEvents());

            //Despachamos los eventos de dominio a través del mediator.
            foreach (var domainEvent in domainEvents)
            {
                await mediator.Publish(domainEvent);
            }


        }


    }
}
