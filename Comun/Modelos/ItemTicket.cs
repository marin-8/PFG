
namespace PFG.Comun
{
	public class ItemTicket
	{
		public byte Unidades { get; private set; }
		public string NombreArticulo { get; private set; }
		public float PrecioUnitario { get; private set; }
		public float PrecioTotal { get; private set; }

		public ItemTicket(byte Unidades, string NombreArticulo, float PrecioUnitario)
		{
			this.Unidades = Unidades;
			this.NombreArticulo = NombreArticulo;
			this.PrecioUnitario = PrecioUnitario;

			PrecioTotal = Unidades * PrecioUnitario;
		}
	}
}
