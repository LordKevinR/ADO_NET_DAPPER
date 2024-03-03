using ADO_NET_DAPPER.DTOs.Transacciones;
using ADO_NET_DAPPER.Entities;
using ADO_NET_DAPPER.Repositories.Articulos;
using ADO_NET_DAPPER.Repositories.Transacciones;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ADO_NET_DAPPER.Endpoints.Transacciones
{
	public static class TransaccionesEndpoints
	{
		public static RouteGroupBuilder MapTransacciones(this RouteGroupBuilder group)
		{
			group.MapGet("/", ObtenerTodos);
			group.MapGet("/{id:int}", ObtenerPorId);
			group.MapPost("/", Crear);
			group.MapPut("/{id:int}", Actualizar);
			group.MapDelete("/{id:int}", Borrar);

			return group;
		}

		public static async Task<Ok<List<TransaccionResponse>>> ObtenerTodos(IRepositorioTransaccion repositorioTransaccion, IRepositorioArticulo repositorioArticulo, IMapper mapper)
		{
			var transacciones = await repositorioTransaccion.ObtenerTodos();
			var transaccionesResponse = mapper.Map<List<TransaccionResponse>>(transacciones);

			foreach(var transaccionResponse in transaccionesResponse)
			{
				var articulo = await repositorioArticulo.ObtenerPorId(transaccionResponse.ArticuloId);
				transaccionResponse.Articulo = articulo;
			}

			foreach (var transaccionResponse in transaccionesResponse)
			{
				if (transaccionResponse.TipoTransaccionId == 1)
				{
					transaccionResponse.TipoTransaccion = new TipoTransaccion { Id = transaccionResponse.TipoTransaccionId, Descripcion = "Entrada" };
				}
				else
				{
					transaccionResponse.TipoTransaccion = new TipoTransaccion { Id = transaccionResponse.TipoTransaccionId, Descripcion = "Salida" };
				}
			}


			foreach (var transaccionResponse in transaccionesResponse)
			{
				if (transaccionResponse.EstadoId == 1)
				{
					transaccionResponse.Estado = new Estado { Id = transaccionResponse.EstadoId, Descripcion = "Completa" };
				}
				else
				{
					transaccionResponse.Estado = new Estado { Id = transaccionResponse.EstadoId, Descripcion = "Incompleta" };
				}
			}

			return TypedResults.Ok(transaccionesResponse);
		}

		public static async Task<Results<Ok<TransaccionResponse>, NotFound<string>>> ObtenerPorId(int id, IRepositorioTransaccion repositorioTransaccion, IRepositorioArticulo repositorioArticulo, IMapper mapper)
		{
			var transaccion = await repositorioTransaccion.ObtenerPorId(id);

			if (transaccion is null)
			{
				return TypedResults.NotFound("Transaccion no encontrada");
			}

			var transaccionResponse = mapper.Map<TransaccionResponse>(transaccion);

			//mostrar articulos

			var articulo = await repositorioArticulo.ObtenerPorId(transaccionResponse.ArticuloId);

			transaccionResponse.Articulo = articulo;



			//mostrar estado

			if(transaccionResponse.EstadoId == 1)
			{
				transaccionResponse.Estado = new Estado { Id = transaccionResponse.EstadoId, Descripcion = "Completa" };
			}
			else
			{
				transaccionResponse.Estado = new Estado { Id = transaccionResponse.EstadoId, Descripcion = "Incompleta" };
			}


			//mostrar tipoTransaccion

			if (transaccionResponse.TipoTransaccionId == 1)
			{
				transaccionResponse.TipoTransaccion = new TipoTransaccion { Id = transaccionResponse.TipoTransaccionId, Descripcion = "Entrada" };
			}
			else
			{
				transaccionResponse.TipoTransaccion = new TipoTransaccion { Id = transaccionResponse.TipoTransaccionId, Descripcion = "Salida" };
			}

			return TypedResults.Ok(transaccionResponse);
		}

		public static async Task<Results<Created<TransaccionResponse>, NotFound<string>, BadRequest<string>>> Crear(TransaccionRequest transaccionRequest, IRepositorioTransaccion repositorioTransaccion, IRepositorioArticulo repositorioArticulo, IMapper mapper)
		{
			var transaccion = mapper.Map<Transaccion>(transaccionRequest);


			var articulo = await repositorioArticulo.ObtenerPorId(transaccion.ArticuloId);

			if(articulo is null)
			{
				return TypedResults.NotFound("Articulo no encontrado");
			}

			if(articulo.Cantidad < transaccion.Cantidad && transaccion.TipoTransaccionId != 1)
			{
				return TypedResults.BadRequest("No hay suficientes articulos");
			}

			if(transaccion.TipoTransaccionId == 1)
			{
				articulo.Cantidad = articulo.Cantidad + transaccion.Cantidad;
			}
			else
			{
				articulo.Cantidad = articulo.Cantidad - transaccion.Cantidad;
			}

			transaccion.CostoTotal = transaccion.Cantidad * articulo.Costo;
			transaccion.FechaDocumento = DateTime.Now;
			transaccion.EstadoId = 1;

			await repositorioArticulo.Actualizar(articulo);
			var id = await repositorioTransaccion.CrearTransaccion(transaccion);

			var transaccionResponse = mapper.Map<TransaccionResponse>(transaccion);

			return TypedResults.Created($"/transacciones/{id}", transaccionResponse);
		}

		public static async Task<Results<NoContent, NotFound<string>>> Actualizar(int id, TransaccionRequest transaccionRequest, IRepositorioTransaccion repositorioTransaccion, IMapper mapper)
		{
			var existe = await repositorioTransaccion.Existe(id);

			if (!existe)
			{
				return TypedResults.NotFound("Transaccion no encontrada");
			}

			var transaccion = mapper.Map<Transaccion>(transaccionRequest);
			transaccion.Id = id;

			await repositorioTransaccion.Actualizar(transaccion);
			return TypedResults.NoContent();
		}

		public static async Task<Results<NoContent, NotFound<string>>> Borrar(int id, IRepositorioTransaccion repositorioTransaccion)
		{
			var existe = await repositorioTransaccion.Existe(id);

			if (!existe)
			{
				return TypedResults.NotFound("Transaccion no encontrada");
			}

			await repositorioTransaccion.Borrar(id);
			return TypedResults.NoContent();
		}
	}
}
