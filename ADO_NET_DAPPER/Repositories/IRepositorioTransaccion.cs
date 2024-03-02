using ADO_NET_DAPPER.Entities;

namespace ADO_NET_DAPPER.Repositories
{
	public interface IRepositorioTransaccion
	{
		Task<int> CrearTransaccion(Transaccion transaccion);
	}
}