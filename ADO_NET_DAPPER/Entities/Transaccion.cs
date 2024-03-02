namespace ADO_NET_DAPPER.Entities
{
	public class Transaccion
	{
		public int Id { get; set; }
		public string Descripcion { get; set; } = string.Empty;
		public int TipoTransaccionId { get; set; }
		public int ArticuloId { get; set; }
		public DateTime FechaDocumento { get; set; }
		public int EstadoId { get; set; }
		public int Cantidad { get; set; }
		public decimal CostoTotal { get; set; }
	}
}
