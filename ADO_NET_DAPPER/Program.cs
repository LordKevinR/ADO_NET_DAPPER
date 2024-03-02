using ADO_NET_DAPPER.Entities;
using ADO_NET_DAPPER.Repositories;
using ADO_NET_DAPPER.Repositories.Articulos;

var builder = WebApplication.CreateBuilder(args);

//Service Area

builder.Services.AddCors(options =>
{
	options.AddPolicy("NuevaPolitica", app =>
	{
		app.AllowAnyOrigin()
		.AllowAnyHeader()
		.AllowAnyMethod();
	});
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IRepositorioTransaccion, RepositorioTransaccion>();
builder.Services.AddScoped<IRepositorioArticulo, RepositorioArticulo>();

//End Service Area


var app = builder.Build();

// middleware
app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("NuevaPolitica");

app.MapGet("/", () => "Hello World!");

app.MapPost("/transacciones", async (Transaccion transaccion, IRepositorioTransaccion repositorioTransaccion) => 
{ 
	await repositorioTransaccion.CrearTransaccion(transaccion);
	return TypedResults.Ok();
});

app.MapPost("/articulos", async (Articulo articulo, IRepositorioArticulo repositorioArticulo) => 
{ 
	var id = await repositorioArticulo.CrearArticulo(articulo);
	return TypedResults.Created($"/articulos/{id}", articulo);
});

app.MapGet("/articulos", async (IRepositorioArticulo repositorioArticulo) =>
{
	var articulos = await repositorioArticulo.ObtenerTodos();
	return TypedResults.Ok(articulos);
});

app.MapGet("/articulos/{id:int}", async (int id, IRepositorioArticulo repositorioArticulo) =>
{
	var articulo = await repositorioArticulo.ObtenerPorId(id);
	if (articulo is null) { return Results.NotFound("Articulo no encontrado"); }
	return Results.Ok(articulo);
});

app.MapPut("/articulos/{id:int}", async (int id, Articulo articulo, IRepositorioArticulo repositorioArticulo) =>
{
	var existe = await repositorioArticulo.Existe(id);

	if (!existe)
	{
		return Results.NotFound();
	}

	await repositorioArticulo.Actualizar(articulo);

});

// end middleware


app.Run();
