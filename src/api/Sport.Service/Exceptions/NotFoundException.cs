using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sport.Service.Exceptions
{
    [Serializable]
    public class NotFoundException : Exception
    {
        public NotFoundException()
        {

        }

        public NotFoundException(string message) : base(message)
        {
        }
    }
}
