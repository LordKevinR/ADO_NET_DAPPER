using ADO_NET_DAPPER.Entities;

namespace ADO_NET_DAPPER.Repositories.Articulos
{
	public interface IRepositorioArticulo
	{
		Task<int> CrearArticulo(Articulo articulo);
		Task<Articulo?> ObtenerPorId(int id);
		Task<List<Articulo>> ObtenerTodos();
		Task<bool> Existe(int id);
		Task Actualizar(Articulo articulo);
		Task Borrar(int id);
	}
}