namespace Cinema.Repositories
{
    public class Repository<T>: IRepository<T> where T : class
    {
        private ApplicationDBContxet _db;
        private DbSet<T> _dbset;
        public Repository(ApplicationDBContxet db)
        {
            _db = db;
            _dbset = _db.Set<T>();
        }
        //CRUD operations will be implemented here  
        public async Task CreateAsync(T entity)
        {
            await _dbset.AddAsync(entity);
        }
        public void ubdate(T entity)
        {
            _dbset.Update(entity);
        }
        public void Delete(T entity)
        {

            _dbset.Remove(entity);
        }
        //read operations
        public async Task<IEnumerable<T>> GetAllasync(Expression<Func<T, bool>>?
            expression = null, Expression<Func<T, object>>[]? includes = null, bool Tracke = true)
        {
            var Categories = _dbset.AsQueryable();
            if (expression is not null)
                Categories = Categories.Where(expression);
            if (!Tracke)
                Categories = _dbset.AsNoTracking();
            if (includes is not null)
            {
                foreach (var include in includes)
                {
                    Categories = Categories.Include(include);
                }
            }

            return await Categories.ToListAsync();
        }
        public async Task<T?> GetoneAsync(Expression<Func<T, bool>>?
            expression = null, Expression<Func<T, object>>[]? includes = null, bool Tracke = true)
        {
            return (await GetAllasync(expression, includes, Tracke)).FirstOrDefault();
        }
        public async Task<int> CommitAsync()
        {
            try
            {
                return await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while saving changes to the database. {ex.Message}");
                return 0;
            }
        }
    }
}
