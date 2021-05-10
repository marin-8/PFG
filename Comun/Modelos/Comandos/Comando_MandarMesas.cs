
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

		[JsonProperty("1")] public byte AnchoMapa { get; private set; }
		[JsonProperty("2")] public byte AltoMapa { get; private set; }
		[JsonProperty("3")] public Mesa[] Mesas { get; private set; }

		private void InicializarPropiedades(byte AnchoMapa, byte AltoMapa, Mesa[] Mesas)
		{
			this.AnchoMapa = AnchoMapa;
			this.AltoMapa = AltoMapa;
			this.Mesas = Mesas;
		}

		public Comando_MandarMesas(byte AnchoMapa, byte AltoMapa, Mesa[] Mesas)
			: base(TipoComandoInit)
		{
			InicializarPropiedades(AnchoMapa, AltoMapa, Mesas);
		}

		[JsonConstructor]
		#pragma warning disable IDE0051
		private Comando_MandarMesas(TiposComando TipoComandoJson, byte AnchoMapa, byte AltoMapa, Mesa[] Mesas)
		#pragma warning restore IDE0051
			: base(TipoComandoJson)
		{
			InicializarPropiedades(AnchoMapa, AltoMapa, Mesas);
		}
	}
}
