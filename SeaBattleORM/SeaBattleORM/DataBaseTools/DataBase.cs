using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace SeaBattleORM
{
    public class DataBase
    {
        public T ExecuteScalar<T>(string query, SqlConnection connection, SqlTransaction transaction, bool isStoredProc = false)
        {
            SqlCommand command = connection.CreateCommand();
            command.Connection = connection;
            command.Transaction = transaction;

            if (isStoredProc)
            {
                command.CommandType = CommandType.StoredProcedure;
            }

            command.CommandText = query;

            return (T)command.ExecuteScalar();
        }

        public IEnumerable<T> ExecuteQuery<T>(string query, SqlConnection connection, SqlTransaction transaction, bool isStoredProc = false, string addToEnd = "")
        {
            var list = new List<T>();
            var t = typeof(T);

            SqlCommand command = connection.CreateCommand();
            command.Connection = connection;
            command.Transaction = transaction;

            if (isStoredProc)
            {
                command.CommandType = CommandType.StoredProcedure;
            }

            query += CheckForeignKey(t);
            query += "\n" + addToEnd;

            command.CommandText = query;

            if (t.IsPrimitive)
            {
                return ExecutePrimitiveQuery<T>(command);
            }

            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                T obj = (T)Activator.CreateInstance(t);
                t.GetProperties().ToList().ForEach(p =>
                {
                    p.SetValue(obj, reader[p.Name]);
                });
                list.Add(obj);
            }

            return list;
        }

        public IEnumerable<T> ExecutePrimitiveQuery<T>(SqlCommand command)
        {
            var list = new List<T>();
            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                T obj = (T)reader[0];

                list.Add(obj);
            }

            return list;
        }

        public int ExecuteNonQuery(string query, SqlConnection connection, SqlTransaction transaction, bool isStoredProc = false)
        {
            SqlCommand command = connection.CreateCommand();
            command.Connection = connection;
            command.Transaction = transaction;

            if (isStoredProc)
            {
                command.CommandType = CommandType.StoredProcedure;
            }

            command.CommandText = query;

            return command.ExecuteNonQuery();
        }

        public string CheckForeignKey(Type t)
        {
            var res = string.Empty;
            var attributes = t.GetProperties().Where(f => f.GetCustomAttribute<ForeignKeyAttribute>() != null);

            if (!attributes.Any())
            {
                return res;
            }

            var table = t.GetCustomAttribute<TableAttribute>().Name;
            var key = attributes.FirstOrDefault().Name;

            var foreignKey = attributes.Select(p => new
            {
                foreignKeyTable = p.GetCustomAttribute<ForeignKeyAttribute>().Name,
                foreignKeyColumn = p.GetCustomAttribute<ForeignKeyAttribute>().CollumnName,
                foreignKeyType = p.GetCustomAttribute<ForeignKeyAttribute>().StoredType
            });

            if (foreignKey.Any())
            {
                foreach (var item in foreignKey)
                {
                    res += $"\ninner join {item.foreignKeyTable} on {table}.{key}={item.foreignKeyTable}.{item.foreignKeyColumn}";
                    res += CheckForeignKey(item.foreignKeyType);
                }
            }

            return res;
        }
    }
}