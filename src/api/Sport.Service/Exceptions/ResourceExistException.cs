using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sport.Service.Exceptions
{
    [Serializable]
    public class ResourceExistException : Exception
    {
        public ResourceExistException()
        {

        }

        public ResourceExistException(string message) : base(message)
        {
        }
    }
}
