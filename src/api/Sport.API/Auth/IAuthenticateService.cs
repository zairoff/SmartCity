using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sport.API.Auth
{
    public interface IAuthenticateService
    {
        string Authenticate(string username, string password);
    }
}
