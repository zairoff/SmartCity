using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sport.Service.Notification
{
    public class SportEvent : ISportEvent
    {
        private readonly List<IObserver> observers;

        public SportEvent()
        {
            observers = new List<IObserver>();
        }

        public void Notify()
        {
            foreach (var observer in observers)
                observer.EventRegistered();
        }

        public void Subscribe(IObserver observer)
        {
            observers.Add(observer);
        }

        public void Unsubscribe(IObserver observer)
        {
            observers.Remove(observer);
        }
    }
}
