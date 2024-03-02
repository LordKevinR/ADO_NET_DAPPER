using ADO_NET_DAPPER.Entities;
using Dapper;
using Microsoft.Data.SqlClient;

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
				var id = await connection.QuerySingleAsync<int>      //QuerySingleAsycn es para 
					(@"INSERT INTO Articulo 
					(Descripcion, FechaIngreso, Estado, FechaVencimiento, Cantidad, Costo) 
					VALUES
					(@Descripcion, @FechaIngreso, @Estado, @FechaVencimiento, @Cantidad, @Costo) ;

					SELECT SCOPE_IDENTITY();
					", articulo);

				articulo.Id = id;
				return id;
			}
		}


		public async Task<List<Articulo>> ObtenerTodos()
		{
			using (var connection = new SqlConnection(connectionString))
			{
				var articulos = await connection.QueryAsync<Articulo>(@"SELECT * FROM Articulo");
				return articulos.ToList();
			}
		}

		public async Task<Articulo?> ObtenerPorId(int id)
		{
			using (var connection = new SqlConnection(connectionString))
			{
				var articulo = await connection.QueryFirstOrDefaultAsync<Articulo>(@"SELECT * FROM Articulo WHERE Id = @id", new {id});
				return articulo;
			}
		}

		public async Task<bool> Existe(int id)
		{
			using(var connection = new SqlConnection(connectionString))
			{
				var existe = await connection.Query
			}
		}

		public Task Actualizar(Articulo articulo)
		{
			throw new NotImplementedException();
		}
	}
}
