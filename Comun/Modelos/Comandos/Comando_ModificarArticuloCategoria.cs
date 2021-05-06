
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

	public class Comando_ModificarArticuloCategoria : Comando
	{
		private const TiposComando TipoComandoInit = TiposComando.ModificarArticuloCategoria;

		[JsonProperty("1")] public string NombreArticulo { get; private set; }
		[JsonProperty("2")] public string NuevaCategoria { get; private set; }

		private void InicializarPropiedades(string NombreArticulo, string NuevaCategoria)
		{
			this.NombreArticulo = NombreArticulo;
			this.NuevaCategoria = NuevaCategoria;
		}

		public Comando_ModificarArticuloCategoria(string NombreArticulo, string NuevaCategoria)
			: base(TipoComandoInit)
		{
			InicializarPropiedades(NombreArticulo, NuevaCategoria);
		}

		[JsonConstructor]
		#pragma warning disable IDE0051
		private Comando_ModificarArticuloCategoria(TiposComando TipoComandoJson, string NombreArticulo, string NuevaCategoria)
		#pragma warning restore IDE0051
			: base(TipoComandoJson)
		{
			InicializarPropiedades(NombreArticulo, NuevaCategoria);
		}
	}
}
	