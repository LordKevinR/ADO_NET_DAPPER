using ADO_NET_DAPPER.Entities;

namespace ADO_NET_DAPPER.Repositories.Transacciones
{
    public interface IRepositorioTransaccion
    {
		Task Actualizar(Transaccion articulo);
		Task Borrar(int id);
		Task<int> CrearTransaccion(Transaccion transaccion);
		Task<bool> Existe(int id);
		Task<Transaccion?> ObtenerPorId(int id);
		Task<List<Transaccion>> ObtenerTodos();
	}
}