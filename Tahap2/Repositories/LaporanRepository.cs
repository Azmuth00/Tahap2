using System.Data;
using Microsoft.Data.SqlClient;
using Tahap2.Models;

namespace Tahap2.Repositories;

public class LaporanRepository : ILaporanRepository
{
    private readonly string _connectionString;

    public LaporanRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    public List<Laporan> GetAll()
    {
        var list = new List<Laporan>();

        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand("GetLaporan", connection)
        {
            CommandType = CommandType.StoredProcedure
        };

        connection.Open();
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            list.Add(new Laporan
            {
                Id = reader.GetInt32(0),
                Nama = reader.GetString(1),
                Tanggal = reader.GetDateTime(2),
                Total = reader.GetDecimal(3)
            });
        }

        return list;
    }
}
