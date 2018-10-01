namespace Infrastructure.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Infrastructure.Factory.Interfaces;
    using Interfaces;
    using models = Infrastructure.Models;

    public class ListRepository : IListRepository
    {
        private readonly IMySQLDatabaseConnectionFactory databaseConnectionFactory;

        public ListRepository(IMySQLDatabaseConnectionFactory databaseConnectionFactory)
        {
            this.databaseConnectionFactory = databaseConnectionFactory;
        }

        public bool AddList(models.List list)
        {
            bool createdResult = false;

            using (var connection = this.databaseConnectionFactory.CreateConnection())
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = @"INSERT INTO list
                                            VALUES(UUID_TO_BIN(@id), @name, @description, @created, @modified);";
                    command.CommandTimeout = 60;
                    command.CommandType = CommandType.Text;
                    command.Connection.Open();

                    var id = command.CreateParameter();
                    id.ParameterName = "id";
                    id.Value = list.Id;
                    id.DbType = DbType.Guid;
                    command.Parameters.Add(id);

                    var name = command.CreateParameter();
                    name.ParameterName = "name";
                    name.Value = list.Name;
                    name.DbType = DbType.String;
                    command.Parameters.Add(name);

                    var description = command.CreateParameter();
                    description.ParameterName = "description";
                    description.Value = list.Description;
                    description.DbType = DbType.String;
                    command.Parameters.Add(description);

                    var created = command.CreateParameter();
                    created.ParameterName = "created";
                    created.Value = list.Created;
                    created.DbType = DbType.DateTime;
                    command.Parameters.Add(created);

                    var modified = command.CreateParameter();
                    modified.ParameterName = "modified";
                    modified.Value = list.Modified;
                    modified.DbType = DbType.DateTime;
                    command.Parameters.Add(modified);

                    var result = command.ExecuteNonQuery();

                    createdResult = result > 0;
                }
            }

            return createdResult;
        }

        public models.List GetList(Guid listId)
        {
            models.List list = null;

            using (var connection = this.databaseConnectionFactory.CreateConnection())
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = @"SELECT BIN_TO_UUID(id) AS id, name, description, created, modified
                                            FROM list WHERE id = UUID_TO_BIN(@id);";
                    command.CommandTimeout = 60;
                    command.CommandType = CommandType.Text;
                    command.Connection.Open();

                    var id = command.CreateParameter();
                    id.ParameterName = "id";
                    id.Value = listId;
                    id.DbType = DbType.Guid;
                    command.Parameters.Add(id);

                    using (var dataReader = command.ExecuteReader(CommandBehavior.SingleRow))
                    {
                        while (dataReader.Read())
                        {
                            list = new models.List()
                            {
                                Id = dataReader.GetGuid(0),
                                Name = dataReader.IsDBNull(1) ? string.Empty : dataReader.GetString(1),
                                Description = dataReader.IsDBNull(2) ? string.Empty : dataReader.GetString(2),
                                Created = dataReader.GetDateTime(3),
                                Modified = dataReader.GetDateTime(4)
                            };
                        }
                    }
                }
            }

            return list;
        }

        public List<models.List> GetLists(string searchValue, int skip, int take)
        {
            List<models.List> lists = null;

            using (var connection = this.databaseConnectionFactory.CreateConnection())
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = @"SELECT BIN_TO_UUID(id) AS id, name, description, created, modified
                                            FROM list";
                    command.CommandTimeout = 60;
                    command.CommandType = CommandType.Text;

                    if (!string.IsNullOrWhiteSpace(searchValue))
                    {
                        command.CommandText += " WHERE (name like @searchValue OR description like @searchValue)";

                        var searchParam = command.CreateParameter();
                        searchParam.ParameterName = "searchValue";
                        searchParam.Value = searchValue;
                        searchParam.DbType = DbType.String;
                        command.Parameters.Add(searchParam);
                    }

                    command.CommandText += $" LIMIT ({skip}, {take});";

                    command.Connection.Open();

                    using (var dataReader = command.ExecuteReader())
                    {
                        lists = new List<models.List>();

                        while (dataReader.Read())
                        {
                            var list = new models.List()
                            {
                                Id = dataReader.GetGuid(0),
                                Name = dataReader.IsDBNull(1) ? string.Empty : dataReader.GetString(1),
                                Description = dataReader.IsDBNull(2) ? string.Empty : dataReader.GetString(2),
                                Created = dataReader.GetDateTime(3),
                                Modified = dataReader.GetDateTime(4)
                            };

                            lists.Add(list);
                        }
                    }
                }
            }

            return lists;
        }

        public bool UpdateList(models.List list)
        {
            bool updatedResult = false;

            using (var connection = this.databaseConnectionFactory.CreateConnection())
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = @"UPDATE list
                                            SET name = @name,
                                            description = @description,
                                            modified = @modified
                                            WHERE id = UUID_TO_BIN(@id)";
                    command.CommandTimeout = 60;
                    command.CommandType = CommandType.Text;
                    command.Connection.Open();

                    var id = command.CreateParameter();
                    id.ParameterName = "id";
                    id.Value = list.Id;
                    id.DbType = DbType.Guid;
                    command.Parameters.Add(id);

                    var name = command.CreateParameter();
                    name.ParameterName = "name";
                    name.Value = list.Name;
                    name.DbType = DbType.String;
                    command.Parameters.Add(name);

                    var description = command.CreateParameter();
                    description.ParameterName = "description";
                    description.Value = list.Description;
                    description.DbType = DbType.String;
                    command.Parameters.Add(description);

                    var modified = command.CreateParameter();
                    modified.ParameterName = "modified";
                    modified.Value = list.Modified;
                    modified.DbType = DbType.DateTime;
                    command.Parameters.Add(modified);

                    var result = command.ExecuteNonQuery();

                    updatedResult = result > 0;
                }
            }

            return updatedResult;
        }
    }
}