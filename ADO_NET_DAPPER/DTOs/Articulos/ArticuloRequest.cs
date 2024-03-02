namespace ADO_NET_DAPPER.DTOs.Articulos
{
	public class ArticuloRequest
	{
		public string Descripcion { get; set; } = string.Empty;
		public DateTime FechaIngreso { get; set; }
		public bool Estado { get; set; }
		public DateTime? FechaVencimiento { get; set; }
		public int Cantidad { get; set; }
		public decimal Costo { get; set; }
	}
}
