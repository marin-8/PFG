
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

	public class Comando_ModificarMesaSitio : Comando
	{
		private const TiposComando TipoComandoInit = TiposComando.ModificarMesaSitio;

		[JsonProperty("1")] public byte NumeroMesa { get; private set; }
		[JsonProperty("2")] public byte NuevoSitioX { get; private set; }
		[JsonProperty("3")] public byte NuevoSitioY { get; private set; }

		private void InicializarPropiedades(byte NumeroMesa, byte NuevoSitioX, byte NuevoSitioY)
		{
			this.NumeroMesa = NumeroMesa;
			this.NuevoSitioX = NuevoSitioX;
			this.NuevoSitioY = NuevoSitioY;
		}

		public Comando_ModificarMesaSitio(byte NumeroMesa, byte NuevoSitioX, byte NuevoSitioY)
			: base(TipoComandoInit)
		{
			InicializarPropiedades(NumeroMesa, NuevoSitioX, NuevoSitioY);
		}

		[JsonConstructor]
		private Comando_ModificarMesaSitio(TiposComando TipoComandoJson, byte NumeroMesa, byte NuevoSitioX, byte NuevoSitioY)
			: base(TipoComandoJson)
		{
			InicializarPropiedades(NumeroMesa, NuevoSitioX, NuevoSitioY);
		}
	}
}
	