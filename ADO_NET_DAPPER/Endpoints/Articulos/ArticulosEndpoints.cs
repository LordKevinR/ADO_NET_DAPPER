using ADO_NET_DAPPER.Entities;
using ADO_NET_DAPPER.Repositories.Articulos;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ADO_NET_DAPPER.Endpoints.Articulos
{
	public static class ArticulosEndpoints
	{
		public static RouteGroupBuilder MapArtiulos(this RouteGroupBuilder group)
		{
			group.MapGet("/", )
			return group;
		}

		public static async Task<Ok<List<Articulo>>> ObtenerTodos(IRepositorioArticulo repositorioArticulo)
		{
			var articulos = await repositorioArticulo.ObtenerTodos();
			return TypedResults.Ok(articulos);
		}
	}
}
