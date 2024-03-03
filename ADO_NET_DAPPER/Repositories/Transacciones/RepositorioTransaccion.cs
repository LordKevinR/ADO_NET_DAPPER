using ADO_NET_DAPPER.Entities;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ADO_NET_DAPPER.Repositories.Transacciones
{
    public class RepositorioTransaccion : IRepositorioTransaccion
    {
        private readonly string? connectionString;

        public RepositorioTransaccion(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }
		public async Task<int> CrearTransaccion(Transaccion transaccion)
		{
			using (var connection = new SqlConnection(connectionString))
			{
				var id = await connection
							.QuerySingleAsync<int>      //QuerySingleAsycn es para indicar que tendremos 1 solo resultado
							(@"Transacciones_Crear",
							new
							{
								transaccion.Descripcion,
								transaccion.TipoTransaccionId,
								transaccion.ArticuloId,
								transaccion.FechaDocumento,
								transaccion.EstadoId,
								transaccion.Cantidad,
								transaccion.CostoTotal
							}, commandType: CommandType.StoredProcedure);

				transaccion.Id = id;
				return id;
			}

		}

		public async Task<List<Transaccion>> ObtenerTodos()
		{
			using (var connection = new SqlConnection(connectionString))
			{
				var transacciones = await connection
					.QueryAsync<Transaccion>
					("Transacciones_ObtenerTodos", commandType: CommandType.StoredProcedure);
				return transacciones.ToList();
			}
		}

		public async Task<Transaccion?> ObtenerPorId(int id)
		{
			using (var connection = new SqlConnection(connectionString))
			{
				var transaccion = await connection
					.QueryFirstOrDefaultAsync<Transaccion>
					("Transacciones_ObtenerPorId", new { id }, commandType: CommandType.StoredProcedure);
				return transaccion;
			}
		}

		public async Task<bool> Existe(int id)
		{
			using (var connection = new SqlConnection(connectionString))
			{
				var existe = await connection
					.QuerySingleAsync<bool>
					(@"Transacciones_Existe", new { id }, commandType: CommandType.StoredProcedure);

				return existe;
			}
		}

		public async Task Actualizar(Transaccion articulo)
		{
			using (var connection = new SqlConnection(connectionString))
			{
				await connection
					.ExecuteAsync
					("Transacciones_Actualizar", articulo, commandType: CommandType.StoredProcedure);
			}
		}

		public async Task Borrar(int id)
		{
			using (var connection = new SqlConnection(connectionString))
			{
				await connection
					.ExecuteAsync
					(@"Transacciones_Borrar", new { id }, commandType: CommandType.StoredProcedure);
			}
		}
	}
}
