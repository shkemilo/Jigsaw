
namespace Jigsaw
{
    /// <summary>
    /// Extends a class to be able to listen to Subject updates.
    /// </summary>
    public interface Observer
    {
        void Update<T>(T message);
    }

    /// <summary>
    /// Extends a class to be able to send a message to all its Observers.
    /// </summary>
    public interface Subject
    {
        void Subscribe(Observer o);

        void Unsubscribe(Observer o);

        void Broadcast<T>(T message);
    }
}
