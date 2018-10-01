namespace Infrastructure.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Infrastructure.Factory.Interfaces;
    using Interfaces;
    using models = Infrastructure.Models;

    /// <summary>
    /// Task Repository
    /// </summary>
    public class TaskRepository : ITaskRepository
    {
        private readonly IMySQLDatabaseConnectionFactory databaseConnectionFactory;

        public TaskRepository(IMySQLDatabaseConnectionFactory databaseConnectionFactory)
        {
            this.databaseConnectionFactory = databaseConnectionFactory;
        }

        public bool AddTask(models.Task task)
        {
            bool createdResult = false;

            using (var connection = this.databaseConnectionFactory.CreateConnection())
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = @"INSERT INTO task
                                            VALUES(UUID_TO_BIN(@id), UUID_TO_BIN(@listId), @name, @completed, @created, @modified, @dateCompleted);";
                    command.CommandTimeout = 60;
                    command.CommandType = CommandType.Text;
                    command.Connection.Open();

                    var id = command.CreateParameter();
                    id.ParameterName = "id";
                    id.Value = task.Id;
                    id.DbType = DbType.Guid;
                    command.Parameters.Add(id);

                    var listId = command.CreateParameter();
                    listId.ParameterName = "listId";
                    listId.Value = task.ListId;
                    listId.DbType = DbType.Guid;
                    command.Parameters.Add(listId);

                    var name = command.CreateParameter();
                    name.ParameterName = "name";
                    name.Value = task.Name;
                    name.DbType = DbType.String;
                    command.Parameters.Add(name);

                    var completed = command.CreateParameter();
                    completed.ParameterName = "completed";
                    completed.Value = task.Completed;
                    completed.DbType = DbType.String;
                    command.Parameters.Add(completed);

                    var created = command.CreateParameter();
                    created.ParameterName = "created";
                    created.Value = task.Created;
                    created.DbType = DbType.DateTime;
                    command.Parameters.Add(created);

                    var modified = command.CreateParameter();
                    modified.ParameterName = "modified";
                    modified.Value = task.Modified;
                    modified.DbType = DbType.DateTime;
                    command.Parameters.Add(modified);

                    var dateCompleted = command.CreateParameter();
                    dateCompleted.ParameterName = "dateCompleted";
                    dateCompleted.Value = task.DateCompleted;
                    dateCompleted.DbType = DbType.DateTime;
                    command.Parameters.Add(dateCompleted);

                    var result = command.ExecuteNonQuery();

                    createdResult = result > 0;
                }
            }

            return createdResult;
        }

        public models.Task GetTask(Guid taskId, Guid listId)
        {
            models.Task task = null;

            using (var connection = this.databaseConnectionFactory.CreateConnection())
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = @"SELECT BIN_TO_UUID(id) AS id, BIN_TO_UUID(listId) as listId, name, completed, created, modified, dateCompleted
                                            FROM task
                                            WHERE id = UUID_TO_BIN(@id)
                                            AND listId = UUID_TO_BIN(@listId);";
                    command.CommandTimeout = 60;
                    command.CommandType = CommandType.Text;
                    command.Connection.Open();

                    var id = command.CreateParameter();
                    id.ParameterName = "id";
                    id.Value = taskId;
                    id.DbType = DbType.Guid;
                    command.Parameters.Add(id);

                    var listIdParam = command.CreateParameter();
                    listIdParam.ParameterName = "listId";
                    listIdParam.Value = listId;
                    listIdParam.DbType = DbType.Guid;
                    command.Parameters.Add(listIdParam);

                    using (var dataReader = command.ExecuteReader(CommandBehavior.SingleRow))
                    {
                        while (dataReader.Read())
                        {
                            task = new models.Task()
                            {
                                Id = dataReader.GetGuid(0),
                                ListId = dataReader.GetGuid(1),
                                Name = dataReader.GetString(2),
                                Completed = dataReader.GetBoolean(2),
                                Created = dataReader.GetDateTime(4),
                                Modified = dataReader.GetDateTime(5),
                                DateCompleted = dataReader.IsDBNull(6) ? (DateTime?)null : dataReader.GetDateTime(6)
                            };
                        }
                    }
                }
            }

            return task;
        }

        public List<models.Task> GetTasks(Guid listId)
        {
            List<models.Task> tasks = null;

            using (var connection = this.databaseConnectionFactory.CreateConnection())
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = @"SELECT BIN_TO_UUID(id) AS id, BIN_TO_UUID(listId) as listId, name, completed, created, modified, dateCompleted
                                            FROM task
                                            WHERE listId = UUID_TO_BIN(@listId);";
                    command.CommandTimeout = 60;
                    command.CommandType = CommandType.Text;
                    command.Connection.Open();

                    var listIdParam = command.CreateParameter();
                    listIdParam.ParameterName = "listId";
                    listIdParam.Value = listId;
                    listIdParam.DbType = DbType.Guid;
                    command.Parameters.Add(listIdParam);

                    using (var dataReader = command.ExecuteReader(CommandBehavior.SingleRow))
                    {
                        tasks = new List<models.Task>();
                        while (dataReader.Read())
                        {
                            var task = new models.Task()
                            {
                                Id = dataReader.GetGuid(0),
                                ListId = dataReader.GetGuid(1),
                                Name = dataReader.GetString(2),
                                Completed = dataReader.GetBoolean(2),
                                Created = dataReader.GetDateTime(4),
                                Modified = dataReader.GetDateTime(5),
                                DateCompleted = dataReader.IsDBNull(6) ? (DateTime?)null : dataReader.GetDateTime(6)
                            };

                            tasks.Add(task);
                        }
                    }
                }
            }

            return tasks;
        }

        public bool UpdateTask(models.Task task)
        {
            bool updatedResult = false;

            using (var connection = this.databaseConnectionFactory.CreateConnection())
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = @"UPDATE task
                                            SET name = @name,
                                            completed = @completed,
                                            modified = @modified,
                                            dateCompleted = @dateCompleted
                                            WHERE id = UUID_TO_BIN(@id)
                                            AND listId = UUID_TO_BIN(@listId)";
                    command.CommandTimeout = 60;
                    command.CommandType = CommandType.Text;
                    command.Connection.Open();

                    var id = command.CreateParameter();
                    id.ParameterName = "id";
                    id.Value = task.Id;
                    id.DbType = DbType.Guid;
                    command.Parameters.Add(id);

                    var name = command.CreateParameter();
                    name.ParameterName = "name";
                    name.Value = task.Name;
                    name.DbType = DbType.String;
                    command.Parameters.Add(name);

                    var completed = command.CreateParameter();
                    completed.ParameterName = "completed";
                    completed.Value = task.Completed;
                    completed.DbType = DbType.String;
                    command.Parameters.Add(completed);

                    var modified = command.CreateParameter();
                    modified.ParameterName = "modified";
                    modified.Value = task.Modified;
                    modified.DbType = DbType.DateTime;
                    command.Parameters.Add(modified);

                    var dateCompleted = command.CreateParameter();
                    dateCompleted.ParameterName = "dateCompleted";
                    dateCompleted.Value = task.DateCompleted;
                    dateCompleted.DbType = DbType.DateTime;
                    command.Parameters.Add(dateCompleted);

                    var result = command.ExecuteNonQuery();

                    updatedResult = result > 0;
                }
            }

            return updatedResult;
        }
    }
}