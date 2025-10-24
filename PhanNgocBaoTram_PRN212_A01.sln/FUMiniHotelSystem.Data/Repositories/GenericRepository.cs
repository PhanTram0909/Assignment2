using Microsoft.EntityFrameworkCore;
using FUMiniHotelSystem.Data.Database;

namespace FUMiniHotelSystem.Data.Repositories
{
    public class GenericRepository<TEntity> where TEntity : class
    {
        protected readonly FUMiniHotelDbContext _db;
        protected readonly DbSet<TEntity> _dbSet;

        public GenericRepository()
        {
            // Tạo DbContext mỗi khi khởi tạo repository
            _db = DbContextFactory.CreateDbContext();
            _dbSet = _db.Set<TEntity>();
        }

        public IEnumerable<TEntity> GetAll()
        {
            // AsNoTracking để tránh tracking khi chỉ đọc
            return _dbSet.AsNoTracking().ToList();
        }

        public TEntity? GetById(int id)
        {
            return _dbSet.Find(id);
        }

        public void Add(TEntity entity)
        {
            _dbSet.Add(entity);
            _db.SaveChanges();
        }

        public void Update(TEntity entity)
        {
            _dbSet.Update(entity);
            _db.SaveChanges();
        }

        public void Delete(TEntity entity)
        {
            _dbSet.Remove(entity);
            _db.SaveChanges();
        }
    }
}
