using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sport.Service.Notification
{
    public interface IObserver
    {
        void EventRegistered();
    }
}
