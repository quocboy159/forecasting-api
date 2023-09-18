namespace ForecastingSystem.DataSyncServices.DataAccessServices.CustomExceptions
{
    public class InvalidDataException: CustomException
    {
        public InvalidDataException()
        {
        }

        public InvalidDataException(string message)
            : base(message)
        {
        }

        public InvalidDataException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
