namespace SeaBattleORM
{
    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = false)]
    public class TableAttribute : Attribute
    {
        private readonly string _name;

        public string Name { get { return _name; } }

        public TableAttribute(string tableName)
        {
            _name = tableName;
        }
    }
}