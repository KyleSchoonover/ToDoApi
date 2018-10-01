namespace Infrastructure.Factory.Interfaces
{
    using System.Data;

    /// <summary>
    /// MySQL Database Connection Factory
    /// </summary>
    public interface IMySQLDatabaseConnectionFactory
    {
        /// <summary>
        /// Creates the connection.
        /// </summary>
        /// <returns>
        /// Database Connection
        /// </returns>
        IDbConnection CreateConnection();
    }
}