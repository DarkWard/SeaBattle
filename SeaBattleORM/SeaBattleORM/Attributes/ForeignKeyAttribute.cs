namespace SeaBattleORM
{
    public class ForeignKeyAttribute : Attribute
    {
        private readonly string _name;

        private readonly string _collumnName;

        private readonly Type _storedType;

        public string Name { get { return _name; } }

        public string CollumnName { get { return _collumnName; } }

        public Type StoredType { get { return _storedType; } }


        public ForeignKeyAttribute(string tableName, string collumnName, Type storedType)
        {
            _name = tableName;
            _collumnName = collumnName;
            _storedType = storedType;
        }
    }
}