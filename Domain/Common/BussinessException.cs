using System;

namespace ForecastingSystem.Domain.Common
{
    public class BussinessException: Exception
    {
        public BussinessException():base() { }

        public BussinessException(string message) : base(message) { }
    }
}
