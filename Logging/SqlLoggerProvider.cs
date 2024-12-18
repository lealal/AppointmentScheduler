namespace AppointmentScheduler.Logging
{
    using Microsoft.Extensions.Logging;

    public class SqlLoggerProvider : ILoggerProvider
    {
        private readonly string connectionString;

        public SqlLoggerProvider(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new SqlLogger(connectionString);
        }

        public void Dispose()
        { }
    }

    

}