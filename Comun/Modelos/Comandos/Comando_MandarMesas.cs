
using Newtonsoft.Json;

namespace PFG.Comun
{
/*
	COSAS QUE HACER AL CREAR UN NUEVO COMANDO:

	- Renombrar Clase (CTRL+H)
	- Cambiar TipoComandoInit
	- Poner Propiedades
	- Enumerar JsonProperty's
	- Método InicializarPropiedades
	- Constructor public
	- Constructor private
*/

	public class Comando_MandarMesas : Comando
	{
		private const TiposComando TipoComandoInit = TiposComando.MandarMesas;

		[JsonProperty("1")] public byte AnchoGrid { get; private set; }
		[JsonProperty("2")] public byte AltoGrid { get; private set; }
		[JsonProperty("3")] public Mesa[] Mesas { get; private set; }

		private void InicializarPropiedades(byte AnchoGrid, byte AltoGrid, Mesa[] Mesas)
		{
			this.AnchoGrid = AnchoGrid;
			this.AltoGrid = AltoGrid;
			this.Mesas = Mesas;
		}

		public Comando_MandarMesas(byte AnchoGrid, byte AltoGrid, Mesa[] Mesas)
			: base(TipoComandoInit)
		{
			InicializarPropiedades(AnchoGrid, AltoGrid, Mesas);
		}

		[JsonConstructor]
		private Comando_MandarMesas(TiposComando TipoComandoJson, byte AnchoGrid, byte AltoGrid, Mesa[] Mesas)
			: base(TipoComandoJson)
		{
			InicializarPropiedades(AnchoGrid, AltoGrid, Mesas);
		}
	}
}
