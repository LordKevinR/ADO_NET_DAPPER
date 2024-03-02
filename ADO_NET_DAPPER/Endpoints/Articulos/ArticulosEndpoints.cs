using ADO_NET_DAPPER.DTOs.Articulos;
using ADO_NET_DAPPER.Entities;
using ADO_NET_DAPPER.Repositories.Articulos;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ADO_NET_DAPPER.Endpoints.Articulos
{
	public static class ArticulosEndpoints
	{
		public static RouteGroupBuilder MapArticulos(this RouteGroupBuilder group)
		{
			group.MapGet("/", ObtenerTodos);
			group.MapGet("/{id:int}", ObtenerPorId);
			group.MapPost("/", Crear);
			group.MapPut("/{id:int}", Actualizar);
			group.MapDelete("/{id:int}", Borrar);

			return group;
		}

		public static async Task<Ok<List<ArticuloResponse>>> ObtenerTodos(IRepositorioArticulo repositorioArticulo, IMapper mapper)
		{
			var articulos = await repositorioArticulo.ObtenerTodos();
			var articulosResponse = mapper.Map<List<ArticuloResponse>>(articulos);
			return TypedResults.Ok(articulosResponse);
		}

		public static async Task<Results<Ok<ArticuloResponse>, NotFound<string>>> ObtenerPorId(int id, IRepositorioArticulo repositorioArticulo, IMapper mapper)
		{
			var articulo = await repositorioArticulo.ObtenerPorId(id);

			if (articulo is null) 
			{ 
				return TypedResults.NotFound("Articulo no encontrado"); 
			}

			var articuloResponse = mapper.Map<ArticuloResponse>(articulo);

			return TypedResults.Ok(articuloResponse);
		}

		public static async Task<Created<ArticuloResponse>> Crear(ArticuloRequest articuloRequest, IRepositorioArticulo repositorioArticulo, IMapper mapper)
		{
			var articulo = mapper.Map<Articulo>(articuloRequest);
			var id = await repositorioArticulo.CrearArticulo(articulo);
			var articuloResponse = mapper.Map<ArticuloResponse>(articulo);
			return TypedResults.Created($"/articulos/{id}", articuloResponse);
		}

		public static async Task<Results<NoContent, NotFound<string>>> Actualizar(int id, ArticuloRequest articuloRequest, IRepositorioArticulo repositorioArticulo, IMapper mapper)
		{
			var existe = await repositorioArticulo.Existe(id);

			if (!existe)
			{
				return TypedResults.NotFound("Articulo no encontrado");
			}

			var articulo = mapper.Map<Articulo>(articuloRequest);
			articulo.Id = id;

			await repositorioArticulo.Actualizar(articulo);
			return TypedResults.NoContent();
		}

		public static async Task<Results<NoContent, NotFound<string>>> Borrar(int id, IRepositorioArticulo repositorioArticulo)
		{
			var existe = await repositorioArticulo.Existe(id);

			if (!existe)
			{
				return TypedResults.NotFound("Articulo no encontrado");
			}

			await repositorioArticulo.Borrar(id);
			return TypedResults.NoContent();
		}
	}
}
