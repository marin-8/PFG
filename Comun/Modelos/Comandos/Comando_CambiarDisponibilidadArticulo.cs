
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

	public class Comando_CambiarDisponibilidadArticulo : Comando
	{
		private const TiposComando TipoComandoInit = TiposComando.CambiarDisponibilidadArticulo;

		[JsonProperty("1")] public string NombreArticulo { get; private set; }

		private void InicializarPropiedades(string NombreArticulo)
		{
			this.NombreArticulo = NombreArticulo;
		}

		public Comando_CambiarDisponibilidadArticulo(string NombreArticulo)
			: base(TipoComandoInit)
		{
			InicializarPropiedades(NombreArticulo);
		}

		[JsonConstructor]
		#pragma warning disable IDE0051
		private Comando_CambiarDisponibilidadArticulo(TiposComando TipoComandoJson, string NombreArticulo)
		#pragma warning restore IDE0051
			: base(TipoComandoJson)
		{
			InicializarPropiedades(NombreArticulo);
		}
	}
}
	