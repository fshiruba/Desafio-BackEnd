namespace Desafio_Backend.Services
{
    public interface IQueueService<T>
    {
        void Publish(T message);
    }
}