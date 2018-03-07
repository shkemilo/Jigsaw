using System.Collections.Generic;


namespace Jigsaw
{
    /// <summary>
    /// Superclass for engine-type classes, used for making calculations for the game.
    /// </summary>
    public abstract class Engine : Subject
    {
        protected List<Observer> observers;

        public Engine()
        {
            observers = new List<Observer>();
        }

        public void Subscribe(Observer o)
        {
            observers.Add(o);
        }

        public void Unsubscribe(Observer o)
        {
            observers.Remove(o);
        }

        public void Broadcast<T>(T message)
        {
            foreach (Observer o in observers)
                o.Update(message);
        }

    }


}
