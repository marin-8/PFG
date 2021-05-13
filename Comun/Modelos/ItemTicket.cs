
using Newtonsoft.Json;

namespace PFG.Comun
{
	public class ItemTicket
	{
		[JsonProperty("1")] public int Unidades { get; private set; }
		[JsonProperty("2")] public string NombreArticulo { get; private set; }
		[JsonProperty("3")] public float PrecioUnitario { get; private set; }
			   [JsonIgnore] public float PrecioTotal { get; private set; }

		public ItemTicket(int Unidades, string NombreArticulo, float PrecioUnitario)
		{
			this.Unidades = Unidades;
			this.NombreArticulo = NombreArticulo;
			this.PrecioUnitario = PrecioUnitario;

			PrecioTotal = Unidades * PrecioUnitario;
		}
	}
}
