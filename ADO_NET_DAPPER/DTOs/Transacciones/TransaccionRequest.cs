namespace ADO_NET_DAPPER.DTOs.Transacciones
{
	public class TransaccionRequest
	{
		public string Descripcion { get; set; } = string.Empty;
		public int TipoTransaccionId { get; set; }
		public int ArticuloId { get; set; }
		public int Cantidad { get; set; }
	}
}
