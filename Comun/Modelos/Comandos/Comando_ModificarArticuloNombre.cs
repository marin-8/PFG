
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

	public class Comando_ModificarArticuloNombre : Comando
	{
		private const TiposComando TipoComandoInit = TiposComando.ModificarArticuloNombre;

		[JsonProperty("1")] public string NombreActual { get; private set; }
		[JsonProperty("2")] public string NombreNuevo { get; private set; }

		private void InicializarPropiedades(string NombreActual, string NombreNuevo)
		{
			this.NombreActual = NombreActual;
			this.NombreNuevo = NombreNuevo;
		}

		public Comando_ModificarArticuloNombre(string NombreActual, string NombreNuevo)
			: base(TipoComandoInit)
		{
			InicializarPropiedades(NombreActual, NombreNuevo);
		}

		[JsonConstructor]
		#pragma warning disable IDE0051
		private Comando_ModificarArticuloNombre(TiposComando TipoComandoJson, string NombreActual, string NombreNuevo)
		#pragma warning restore IDE0051
			: base(TipoComandoJson)
		{
			InicializarPropiedades(NombreActual, NombreNuevo);
		}
	}
}
	