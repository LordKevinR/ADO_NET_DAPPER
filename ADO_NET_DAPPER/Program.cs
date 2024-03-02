using ADO_NET_DAPPER.Endpoints.Articulos;
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

builder.Services.AddAutoMapper(typeof(Program));

//End Service Area


var app = builder.Build();

// middleware
app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("NuevaPolitica");

app.MapGroup("/articulos").MapArticulos();


// end middleware


app.Run();


