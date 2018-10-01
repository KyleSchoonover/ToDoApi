namespace Infrastructure.Factory
{
    using System.Data;
    using Infrastructure.Factory.Interfaces;
    using MySql.Data.MySqlClient;

    /// <summary>
    /// MySQL Database Connection Factory
    /// </summary>
    public class MySQLDatabaseConnectionFactory : IMySQLDatabaseConnectionFactory
    {
        /// <summary>
        /// The connection string
        /// </summary>
        private readonly string connectionString;

        /// <summary>
        /// Initializes a new instance of the <see cref="MySQLDatabaseConnectionFactory" /> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public MySQLDatabaseConnectionFactory(string connectionString)
        {
            this.connectionString = connectionString;
        }

        /// <summary>
        /// Creates the connection.
        /// </summary>
        /// <returns>
        /// Database Connection Object
        /// </returns>
        public IDbConnection CreateConnection()
        {
            return new MySqlConnection(this.connectionString);
        }
    }
}