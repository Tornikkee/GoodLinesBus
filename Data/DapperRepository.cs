using Dapper;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System.Collections.Generic;
using AdminPanel.Models;

public class DapperRepository
{
    private readonly string _connectionString;

    public DapperRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    public IDbConnection Connection
    {
        get
        {
            return new SqlConnection(_connectionString);
        }
    }

    public async Task<IEnumerable<ApplicationUser>> GetAllUsersAsync()
    {
        using (IDbConnection dbConnection = Connection)
        {
            string query = "SELECT * FROM AspNetUsers";
            dbConnection.Open();
            return await dbConnection.QueryAsync<ApplicationUser>(query);
        }
    }
}
