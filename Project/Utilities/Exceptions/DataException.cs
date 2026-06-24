namespace Utilities.Exceptions
{
    public class DataException : Exception
    {
        public DataException(string message) : base(message)
        {
        }

        public DataException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

    public class DatabaseConnectionException : DataException
    {
        public DatabaseConnectionException(string message) : base(message)
        {
        }

        public DatabaseConnectionException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

    public class QueryExecutionException : DataException
    {
        public QueryExecutionException(string message) : base(message)
        {
        }

        public QueryExecutionException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

    public class ConcurrencyException : DataException
    {
        public ConcurrencyException(string message) : base(message)
        {
        }

        public ConcurrencyException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

    public class DataIntegrityException : DataException
    {
        public DataIntegrityException(string message) : base(message)
        {
        }

        public DataIntegrityException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}