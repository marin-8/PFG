
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

	public class Comando_ModificarArticuloPrecio : Comando
	{
		private const TiposComando TipoComandoInit = TiposComando.ModificarArticuloPrecio;

		[JsonProperty("1")] public string NombreArticulo { get; private set; }
		[JsonProperty("2")] public float NuevoPrecio { get; private set; }

		private void InicializarPropiedades(string NombreArticulo, float NuevoPrecio)
		{
			this.NombreArticulo = NombreArticulo;
			this.NuevoPrecio = NuevoPrecio;
		}

		public Comando_ModificarArticuloPrecio(string NombreArticulo, float NuevoPrecio)
			: base(TipoComandoInit)
		{
			InicializarPropiedades(NombreArticulo, NuevoPrecio);
		}

		[JsonConstructor]
		#pragma warning disable IDE0051
		private Comando_ModificarArticuloPrecio(TiposComando TipoComandoJson, string NombreArticulo, float NuevoPrecio)
		#pragma warning restore IDE0051
			: base(TipoComandoJson)
		{
			InicializarPropiedades(NombreArticulo, NuevoPrecio);
		}
	}
}
	