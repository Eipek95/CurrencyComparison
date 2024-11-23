using Dapper;
using DataAccess.Dapper.Abstract;
using DataAccess.Dapper.Contexts;
using Entities;

namespace DataAccess.Dapper.Concrete
{
    public class LogDal : ILogDal
    {
        private readonly DapperDbContext _dbContext;

        public LogDal(DapperDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddLogAsync(LogModel log)
        {
            var sql = @"
                INSERT INTO TBL_LOG 
                  (ISLEM_TARIHI, KULLANICI_IP, KUR, YUZDESEL_DEGISIM, HATA_FONKSIYON, HATA_ACIKLAMASI)
                  VALUES (@TransactionDate, @UserIP, @Currency, @PercentageChange, @ErrorFunction, @ErrorDescription)";
            using (var connection = _dbContext.Connection)
            {
                await connection.ExecuteAsync(sql, log);
            }
        }

        public async Task<IEnumerable<LogModel>> GetAllLog()
        {
            var sql = "SELECT * FROM TBL_LOG";
            using (var connection = _dbContext.Connection)
            {
                return await connection.QueryAsync<LogModel>(sql);
            }
        }
    }
}
