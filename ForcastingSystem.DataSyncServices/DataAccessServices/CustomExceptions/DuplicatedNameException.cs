using ForecastingSystem.DataSyncServices.Helpers;

namespace ForecastingSystem.DataSyncServices.DataAccessServices.CustomExceptions
{
    public class DuplicatedNameException : CustomException
    {
        public DuplicatedNameException()
        {
        }

        public DuplicatedNameException(string message)
            : base(message)
        {
        }

        public DuplicatedNameException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
