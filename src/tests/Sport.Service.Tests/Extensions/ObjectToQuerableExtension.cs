using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sport.Service.Tests.Extensions
{
    public static class ObjectToQuerableExtension
    {
        public static IQueryable<T> ToQueryable<T>(this T instance)
        {
            return new[] { instance }.AsQueryable();
        }
    }
}