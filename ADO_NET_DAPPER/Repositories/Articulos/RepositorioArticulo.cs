using ADO_NET_DAPPER.Entities;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ADO_NET_DAPPER.Repositories.Articulos
{
	public class RepositorioArticulo : IRepositorioArticulo
	{
		private readonly string? connectionString;

		public RepositorioArticulo(IConfiguration configuration)
		{
			connectionString = configuration.GetConnectionString("DefaultConnection");
		}

		public async Task<int> CrearArticulo(Articulo articulo)
		{
			using (var connection = new SqlConnection(connectionString))
			{
				var id = await connection
					.QuerySingleAsync<int>      //QuerySingleAsycn es para indicar que tendremos 1 solo resultado
					(@"Articulos_Crear", 
					new 
					{
						articulo.Descripcion, 
						articulo.FechaIngreso, 
						articulo.Estado,
						articulo.FechaVencimiento,
						articulo.Cantidad,
						articulo.Costo
					}, commandType: CommandType.StoredProcedure);

				articulo.Id = id;
				return id;
			}
		}


		public async Task<List<Articulo>> ObtenerTodos()
		{
			using (var connection = new SqlConnection(connectionString))
			{
				var articulos = await connection
					.QueryAsync<Articulo>
					("Articulos_ObtenerTodos", commandType: CommandType.StoredProcedure);
				return articulos.ToList();
			}
		}

		public async Task<Articulo?> ObtenerPorId(int id)
		{
			using (var connection = new SqlConnection(connectionString))
			{
				var articulo = await connection
					.QueryFirstOrDefaultAsync<Articulo>
					("Articulos_ObtenerPorId", new { id }, commandType: CommandType.StoredProcedure);
				return articulo;
			}
		}

		public async Task<bool> Existe(int id)
		{
			using(var connection = new SqlConnection(connectionString))
			{
				var existe = await connection
					.QuerySingleAsync<bool>
					(@"Articulos_Existe", new {id}, commandType: CommandType.StoredProcedure);

				return existe;
			}
		}

		public async Task Actualizar(Articulo articulo)
		{
			using (var connection = new SqlConnection(connectionString))
			{
				await connection
					.ExecuteAsync
					("Articulos_Actualizar", articulo, commandType: CommandType.StoredProcedure);
			}
		}

		public async Task Borrar(int id)
		{
			using(var connection = new SqlConnection(connectionString))
			{
				await connection
					.ExecuteAsync
					(@"Articulos_Borrar", new {id}, commandType: CommandType.StoredProcedure);
			}
		}
	}
}
