namespace SeaBattleORM
{
    public class UnitOfWork<T> : IUnitOfWork
    {
        private string _connectionString;
        private Repository<T> _repository;
        public Repository<T> Repository
        {
            get
            {
                if (_repository == null)
                    _repository = new Repository<T>(_connectionString);
                return _repository;
            }
        }

        public UnitOfWork(string connectionString)
        {
            _connectionString = connectionString;
        }
    }
}