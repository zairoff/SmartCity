using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sport.Service.Notification
{
    public class Observer : IObserver
    {
        private readonly string _url;

        public Observer(string url)
        {
            _url = url;
        }

        public void EventRegistered()
        {
            
        }
    }
}
