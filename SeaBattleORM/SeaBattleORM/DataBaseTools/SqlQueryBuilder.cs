namespace SeaBattleORM
{
    public class SqlQueryBuilder
    {
        private const string _selectAllQuery = "select * from {0}\n";
        private const string _insertQuery = "insert into {0} ({1})\n";
        private const string _deleteQuery = "delete from {0}\n";
        private const string _updateQuery = "update {0} set {1}\n";

        private const string _valuesQueryArgument = "values {0}\n";
        private const string _outputQueryArgument = "output inserted.ID\n";
        private const string _whereQueryArgument = "where {0}={1}\n";

        public static string BuildSelectAllQuery(string tableName)
        {
            return string.Format(_selectAllQuery, tableName);
        }

        public static string BuildSelectQuery(string tableName, string keyParameterName, string itemID)
        {
            return string.Format(_selectAllQuery, tableName) + string.Format(_whereQueryArgument, keyParameterName, itemID);
        }

        public static string BuildInsertQuery(string tableName, string columnsNames, string valuesList, bool output = false)
        {
            if (output)
            {
                return string.Format(_insertQuery, tableName, columnsNames) + _outputQueryArgument + string.Format(_valuesQueryArgument, valuesList);
            }

            return string.Format(_insertQuery, tableName, columnsNames) + string.Format(_valuesQueryArgument, valuesList);
        }

        public static string BuildDeleteQuery(string tableName, string keyParameterName, string itemID, bool output = false)
        {
            if (output)
            {
                return string.Format(_deleteQuery, tableName) + _outputQueryArgument + string.Format(_whereQueryArgument, keyParameterName, itemID);
            }

            return string.Format(_deleteQuery, tableName) + string.Format(_whereQueryArgument, keyParameterName, itemID);
        }

        public static string BuildUpdateQuery(string tableName, string newValue, string keyParameterName, string itemID)
        {
            return string.Format(_updateQuery, tableName, newValue) + string.Format(_whereQueryArgument, keyParameterName, itemID);
        }
    }
}