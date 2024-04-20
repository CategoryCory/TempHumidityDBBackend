using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Data;
using TempHumidityBackend.Data.Models;

namespace TempHumidityBackend.Data;

public sealed class TempHumidityData : ITempHumidityData
{
    private readonly IConfiguration _config;
    private readonly string _connectionId;

    public TempHumidityData(IConfiguration config, string connectionId = "ApplicationConnection")
    {
        _config = config;
        _connectionId = connectionId;
    }

    public async Task<int> Insert(TempHumidity tempHumidity)
    {
        string sql = "INSERT INTO temp_humidity (temp_c, rel_humidity, read_at) VALUES (@temp_c, @rel_humidity, @read_at);";

        using IDbConnection conn = new NpgsqlConnection(_config.GetConnectionString(_connectionId));

        int insertedRows = await conn.ExecuteAsync(sql, new { temp_c = tempHumidity.TempC, rel_humidity = tempHumidity.RelHumidity, read_at = tempHumidity.ReadAt });

        return insertedRows;
    }
}