using ADO_NET_DAPPER.Entities;
using Dapper;
using Microsoft.Data.SqlClient;

namespace ADO_NET_DAPPER.Repositories
{
	public class RepositorioTransaccion : IRepositorioTransaccion
	{
		private readonly string? connectionString;

		public RepositorioTransaccion(IConfiguration configuration)
		{
			connectionString = configuration.GetConnectionString("DefaultConnection");
		}
		public Task<int> CrearTransaccion(Transaccion transaccion)
		{
			using (var connection = new SqlConnection(connectionString))
			{
				var query = connection.Query("SELECT 1").FirstOrDefault();
			}

			return Task.FromResult(0);
		}
	}
}
