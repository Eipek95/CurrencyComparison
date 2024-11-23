using Entities;

namespace DataAccess.Dapper.Abstract
{
    public interface ILogDal
    {

        Task<IEnumerable<LogModel>> GetAllLog();
        Task AddLogAsync(LogModel log);
    }
}
