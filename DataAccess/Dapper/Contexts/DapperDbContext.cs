using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace DataAccess.Dapper.Contexts
{
    public class DapperDbContext
    {
        private readonly IConfiguration _configuration;
        private IDbConnection _dbConnection;

        public DapperDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _dbConnection = new SqlConnection(_configuration.GetConnectionString("SqlConnection"));
        }
        public IDbConnection Connection
        {
            get
            {
                if (_dbConnection == null)
                {
                    _dbConnection = new SqlConnection(_configuration.GetConnectionString("SqlConnection"));
                }

                if (_dbConnection.State == ConnectionState.Closed)
                {
                    _dbConnection.Open();
                }

                return _dbConnection;
            }
        }

        public void Dispose()
        {
            if (_dbConnection != null && _dbConnection.State != ConnectionState.Closed)
            {
                _dbConnection.Close();
                _dbConnection.Dispose();
            }
        }
    }
}
