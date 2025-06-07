namespace Ordering.Domain.Abstractions
{
    // Generico para la entidad
    public interface IAggregate<T> : IAggregate, IEntity<T>
    {
    }


    // Entidad especial que representa un agregado
    // Un agregado es un conjunto de entidades que se agrupan para formar una unidad de consistencia
    // Un agregado tiene una raíz (root) que es la entidad principal del agregado
    // La raíz del agregado es la única entidad que puede ser accedida desde el exterior
    // El resto de las entidades del agregado son entidades internas que no pueden ser accedidas desde el exterior
    // El agregado contiene una lista de eventos de dominio que representan cambios en el estado del agregado !!!!
    public interface IAggregate : IEntity
    {

        IReadOnlyList<IDomainEvent> DomainEvents { get; }
        IDomainEvent[] ClearDomainEvents();
    }

}
