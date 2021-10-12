using Sport.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sport.Service.Interfaces
{
    public interface INotificationService
    {
        Task SendAsync(SportEvent sportEvent);
    }
}
