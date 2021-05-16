
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

	public class Comando_ModificarCategoriaNombre : Comando
	{
		private const TiposComando TipoComandoInit = TiposComando.ModificarCategoriaNombre;

		[JsonProperty("1")] public string NombreActual { get; private set; }
		[JsonProperty("2")] public string NuevoNombre { get; private set; }

		private void InicializarPropiedades(string NombreActual, string NuevoNombre)
		{
			this.NombreActual = NombreActual;
			this.NuevoNombre = NuevoNombre;
		}

		public Comando_ModificarCategoriaNombre(string NombreActual, string NuevoNombre)
			: base(TipoComandoInit)
		{
			InicializarPropiedades(NombreActual, NuevoNombre);
		}

		[JsonConstructor]
		#pragma warning disable IDE0051
		private Comando_ModificarCategoriaNombre(TiposComando TipoComandoJson, string NombreActual, string NuevoNombre)
		#pragma warning restore IDE0051
			: base(TipoComandoJson)
		{
			InicializarPropiedades(NombreActual, NuevoNombre);
		}
	}
}
	