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
				var articulos = await connection.QueryAsync<Articulo>("Articulos_ObtenerTodos", commandType: CommandType.StoredProcedure);
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
				var existe = await connection
					.QuerySingleAsync<bool>
					(@"IF EXISTS (SELECT 1 FROM Articulo WHERE Id = @Id)
						SELECT 1
					ELSE
						SELECT 0", new {id});

				return existe;
			}
		}

		public async Task Actualizar(Articulo articulo)
		{
			using (var connection = new SqlConnection(connectionString))
			{
				await connection.ExecuteAsync(@"UPDATE Articulo
												SET 
												Descripcion = @Descripcion,
												FechaIngreso = @FechaIngreso,
												Estado = @Estado,
												FechaVencimiento = @FechaVencimiento,
												Cantidad = @Cantidad,
												Costo = @Costo
												WHERE Id = @id
												", articulo);


			}
		}

		public async Task Borrar(int id)
		{
			using(var connection = new SqlConnection(connectionString))
			{
				await connection.ExecuteAsync(@"DELETE Articulo
												WHERE Id = @Id", new {id});
			}
		}
	}
}
