
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

	public class Comando_CrearArticulo : Comando
	{
		private const TiposComando TipoComandoInit = TiposComando.CrearArticulo;

		[JsonProperty("1")] public Articulo NuevoArticulo { get; private set; }

		private void InicializarPropiedades(Articulo NuevoArticulo)
		{
			this.NuevoArticulo = NuevoArticulo;
		}

		public Comando_CrearArticulo(Articulo NuevoArticulo)
			: base(TipoComandoInit)
		{
			InicializarPropiedades(NuevoArticulo);
		}

		[JsonConstructor]
		#pragma warning disable IDE0051
		private Comando_CrearArticulo(TiposComando TipoComandoJson,  Articulo NuevoArticulo)
		#pragma warning restore IDE0051
			: base(TipoComandoJson)
		{
			InicializarPropiedades(NuevoArticulo);
		}
	}
}
