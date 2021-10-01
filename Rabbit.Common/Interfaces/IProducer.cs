namespace Rabbit.Common.Interfaces
{
    public interface IProducer<in T>
    {
        void Publish(T @event);
    }
}
