using Business.Dtos;

namespace Business.Abstract
{
    public interface ILogService
    {
        Task<IEnumerable<LogDtoModel>> GetAllLog();
        Task AddLogAsync(CreateLogDtoModel log);
    }
}
