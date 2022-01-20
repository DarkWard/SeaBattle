using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace SeaBattleORM
{
    public class Repository<T> : IRepository<T>
    {
        private DataBase _db;
        private readonly string _connectionString;

        public Repository(string connectionString)
        {
            _connectionString = connectionString;
            _db = new DataBase();
        }

        public void Create(T item)
        {
            if (item == null)
            {
                return;
            }

            SqlConnection connection = new SqlConnection(_connectionString);

            var type = item.GetType();
            var properties = type.GetProperties().Where(f => f.GetCustomAttribute<PrimaryKeyAttribute>() == null);
            var propertyValues = properties.ToDictionary(x => x.Name, x => x.GetValue(item));
            var table = type.GetCustomAttribute<TableAttribute>().Name;

            var listProps = properties.Where(x =>
                x.PropertyType == typeof(Dictionary<CoordinateModel, ShipsModel>))
                .ToDictionary(x => x.Name, x => x.GetValue(item));

            if (!listProps.Any())
            {
                using (connection)
                {
                    connection.Open();
                    SqlTransaction transaction = connection.BeginTransaction();

                    try
                    {
                        _db.ExecuteNonQuery(SqlQueryBuilder.BuildInsertQuery(table,
                            string.Join(",", propertyValues.Select(x => x.Key)),
                            $"({string.Join(",", propertyValues.Select(x => x.Value is string ? "\'" + x.Value + "\'" : x.Value))})"),
                            connection, transaction);

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Commit Exception Type: {0}", ex.GetType());
                        Console.WriteLine("  Message: {0}", ex.Message);

                        transaction.Rollback();
                    }
                }
            }
            else if (listProps.Any())
            {
                using (connection)
                {
                    connection.Open();
                    SqlTransaction transaction = connection.BeginTransaction();

                    try
                    {

                        var lists = listProps.Values;

                        var res =
                            _db.ExecuteScalar<int>(
                                SqlQueryBuilder.BuildInsertQuery(table,
                                    string.Join(",", propertyValues.Keys.Except(listProps.Select(x => x.Key))),
                                    $"({string.Join(",", propertyValues.Values.Except(listProps.Select(x => x.Value is string ? "\'" + x.Value + "\'" : x.Value)))})", true),
                                connection, transaction);

                        foreach (var list in lists)
                        {
                            var targetCollection = list as Dictionary<CoordinateModel, ShipsModel>;

                            if (targetCollection == null)
                            {
                                return;
                            }

                            Type keyType = targetCollection.Keys.First().GetType();
                            Type valueType = targetCollection.Values.First().GetType();

                            var keyProps = keyType.GetProperties().Where(f => f.GetCustomAttribute<PrimaryKeyAttribute>() == null).Select(x => x.Name);
                            var valueProps = valueType.GetProperties().Where(f => f.GetCustomAttribute<PrimaryKeyAttribute>() == null).Select(x => x.Name);

                            var k = targetCollection.ToDictionary(x => $"({x.Key})", x => $"({x.Value})");

                            var keyIDs =
                                _db.ExecuteQuery<int>(
                                    SqlQueryBuilder.BuildInsertQuery(keyType.GetCustomAttribute<TableAttribute>().Name,
                                        string.Join(", ", keyProps),
                                        string.Join(", ", k.Keys),
                                        true), connection, transaction).ToList();

                            var valueIDs =
                                _db.ExecuteQuery<int>(
                                    SqlQueryBuilder.BuildInsertQuery(valueType.GetCustomAttribute<TableAttribute>().Name,
                                        string.Join(", ", valueProps),
                                        string.Join(", ", k.Values),
                                        true), connection, transaction).ToList();

                            var keyAttr = keyType.GetCustomAttribute<ForeignKeyAttribute>();
                            var valueAttr = valueType.GetCustomAttribute<ForeignKeyAttribute>();

                            if (keyAttr != null && valueAttr != null
                                && keyAttr.StoredType == valueAttr.StoredType
                                && keyIDs.Count > 0 && keyIDs.Count == valueIDs.Count)
                            {
                                var valuesList = new List<string>();

                                for (int i = 0; i < keyIDs.Count; i++)
                                {
                                    valuesList.Add($"({keyIDs[i]}, {valueIDs[i]})");
                                }

                                var idList =
                                    _db.ExecuteQuery<int>(
                                        SqlQueryBuilder.BuildInsertQuery(keyAttr.StoredType.GetCustomAttribute<TableAttribute>().Name,
                                            $"{keyAttr.CollumnName}, {valueAttr.CollumnName}",
                                            string.Join(", ", valuesList),
                                            true), connection, transaction).ToList();


                                if (keyAttr.StoredType.GetCustomAttribute<ForeignKeyAttribute>() != null)
                                {
                                    Type t = keyAttr.StoredType.GetCustomAttribute<ForeignKeyAttribute>().StoredType;
                                    var storedProps = t.GetProperties().Select(x => x.Name);

                                    var storedResult = new List<string>();

                                    foreach (var id in idList)
                                    {
                                        storedResult.Add($"({id}, {res})");
                                    }

                                    _db.ExecuteQuery<int>(
                                        SqlQueryBuilder.BuildInsertQuery(t.GetCustomAttribute<TableAttribute>().Name,
                                            string.Join(", ", storedProps),
                                            string.Join(", ", storedResult)),
                                        connection, transaction);
                                }
                            }
                        }

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Commit Exception Type: {0}", ex.GetType());
                        Console.WriteLine("  Message: {0}", ex.Message);

                        transaction.Rollback();
                    }
                }
            }
        }

        public T Get(int id)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            T res = default(T);

            var tableName = typeof(T).GetCustomAttribute<TableAttribute>().Name;

            var primaryKey = typeof(T).GetProperties()
                .Where(f => f.GetCustomAttribute<PrimaryKeyAttribute>() != null).FirstOrDefault().Name;

            using (connection)
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();

                try
                {
                    res = _db.ExecuteQuery<T>(SqlQueryBuilder.BuildSelectQuery(tableName, primaryKey, $"{id}"), connection, transaction).First();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Commit Exception Type: {0}", ex.GetType());
                    Console.WriteLine("  Message: {0}", ex.Message);

                    transaction.Rollback();
                }
            }

            return res;
        }

        public IEnumerable<T> GetAll()
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            var tableName = typeof(T).GetCustomAttribute<TableAttribute>().Name;

            IEnumerable<T> result = null;

            using (connection)
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();

                try
                {
                    result = _db.ExecuteQuery<T>(SqlQueryBuilder.BuildSelectAllQuery(tableName), connection, transaction);
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Commit Exception Type: {0}", ex.GetType());
                    Console.WriteLine("  Message: {0}", ex.Message);

                    transaction.Rollback();
                }
            }

            return result;
        }

        public void Update(T item)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            var tableName = typeof(T).GetCustomAttribute<TableAttribute>().Name;

            var columns = String.Join(", ", item.GetType().GetProperties()
                .Where(f => f.GetCustomAttribute<PrimaryKeyAttribute>() == null && f.GetValue(item) != null)
                .Select(x => x.Name));

            var primaryKey = item.GetType().GetProperties()
                .Where(f => f.GetCustomAttribute<PrimaryKeyAttribute>() != null).FirstOrDefault();

            var data = item.GetType().GetProperties()
               .Where(f => f.GetCustomAttribute<PrimaryKeyAttribute>() == null && f.GetValue(item) != null)
               .Select(x =>
               {
                   var value = x.GetValue(item);
                   if (value is string)
                   {
                       return $"'{value}'";
                   }
                   return value;
               }).ToArray();

            var dataIndex = 0;
            for (var index = 0; index < columns.Length; index++)
            {
                var tempIndex = 0;
                if (columns[index] == ',')
                {
                    columns = columns.Insert(index, string.Concat("=", data[dataIndex]));
                    tempIndex = data[dataIndex].ToString().Length;
                    index = index + tempIndex + 1;
                    dataIndex = dataIndex + 1;
                }

                if (index == columns.Length - 1)
                {
                    columns = columns.Insert(index + 1, string.Concat("=", data[dataIndex]));
                    break;
                }
            }

            using (connection)
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();

                try
                {
                    _db.ExecuteNonQuery(SqlQueryBuilder.BuildUpdateQuery(tableName, columns, primaryKey.Name, primaryKey.GetValue(item).ToString()), connection, transaction);
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Commit Exception Type: {0}", ex.GetType());
                    Console.WriteLine("  Message: {0}", ex.Message);

                    transaction.Rollback();
                }
            }
        }

        public void Delete(T item)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            var tableName = typeof(T).GetCustomAttribute<TableAttribute>().Name;
            var primaryKey = typeof(T).GetProperties().Where(f => f.GetCustomAttribute<PrimaryKeyAttribute>() != null).FirstOrDefault();

            using (connection)
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();

                try
                {

                    _db.ExecuteNonQuery(SqlQueryBuilder.BuildDeleteQuery(tableName, primaryKey.Name, primaryKey.GetValue(item).ToString()), connection, transaction);
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Commit Exception Type: {0}", ex.GetType());
                    Console.WriteLine("  Message: {0}", ex.Message);

                    transaction.Rollback();
                }
            }
        }
    }
}