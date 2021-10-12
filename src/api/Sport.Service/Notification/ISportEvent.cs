using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sport.Service.Notification
{
    public interface ISportEvent
    {
        void Subscribe(IObserver observer);
        void Unsubscribe(IObserver observer);

        void Notify();
    }
}
