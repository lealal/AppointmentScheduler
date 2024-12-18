using Microsoft.Data.SqlClient;

namespace AppointmentScheduler.Logging
{
    public class SqlLogger : ILogger
    {
        private readonly string connectionString;

        public SqlLogger(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            var logMessage = formatter(state, exception);

            using (var connection = new SqlConnection(connectionString))
            {
                var command = new SqlCommand("INSERT INTO Logs (LogLevel, Message) VALUES (@LogLevel, @Message)", connection);
                command.Parameters.AddWithValue("@LogLevel", logLevel.ToString());
                command.Parameters.AddWithValue("@Message", logMessage);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        IDisposable? ILogger.BeginScope<TState>(TState state)
        {
            return null;
        }
    }
}