using Business.Abstract;
using Business.Dtos;
using DataAccess.Dapper.Abstract;
using Entities;

namespace Business.Concrete
{
    public class LogService : ILogService
    {
        private readonly ILogDal _logDal;

        public LogService(ILogDal logDal)
        {
            _logDal = logDal;
        }

        public async Task AddLogAsync(CreateLogDtoModel log)
        {
            var logModel = new LogModel
            {
                PercentageChange = log.PercentageChange,
                ErrorDescription = log.ErrorDescription,
                ErrorFunction = log.ErrorFunction,
                Currency = log.Currency,
                TransactionDate = log.TransactionDate,
                UserIP = log.UserIP,
            };

            await _logDal.AddLogAsync(logModel);
        }

        public async Task<IEnumerable<LogDtoModel>> GetAllLog()
        {
            var log = await _logDal.GetAllLog();
            var logDto = log.Select(x => new LogDtoModel
            {
                Id = x.Id,
                UserIP = x.UserIP,
                Currency = x.Currency,
                ErrorDescription = x.ErrorDescription,
                ErrorFunction = x.ErrorFunction,
                PercentageChange = x.PercentageChange,
                TransactionDate = x.TransactionDate,
            }).ToList();

            return logDto;
        }
    }
}
