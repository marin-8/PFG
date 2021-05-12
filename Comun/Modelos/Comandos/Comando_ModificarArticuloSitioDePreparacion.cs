
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

	public class Comando_ModificarArticuloSitioDePreparacion : Comando
	{
		private const TiposComando TipoComandoInit = TiposComando.ModificarArticuloSitioDePreparacion;

		[JsonProperty("1")] public string NombreArticulo { get; private set; }
		[JsonProperty("2")] public SitioPreparacionArticulo NuevoSitioPreparacion { get; private set; }

		private void InicializarPropiedades(string NombreArticulo, SitioPreparacionArticulo NuevoSitioPreparacion)
		{
			this.NombreArticulo = NombreArticulo;
			this.NuevoSitioPreparacion = NuevoSitioPreparacion;
		}

		public Comando_ModificarArticuloSitioDePreparacion(string NombreArticulo, SitioPreparacionArticulo NuevoSitioPreparacion)
			: base(TipoComandoInit)
		{
			InicializarPropiedades(NombreArticulo, NuevoSitioPreparacion);
		}

		[JsonConstructor]
		#pragma warning disable IDE0051
		private Comando_ModificarArticuloSitioDePreparacion(TiposComando TipoComandoJson, string NombreArticulo, SitioPreparacionArticulo NuevoSitioPreparacion)
		#pragma warning restore IDE0051
			: base(TipoComandoJson)
		{
			InicializarPropiedades(NombreArticulo, NuevoSitioPreparacion);
		}
	}
}
	