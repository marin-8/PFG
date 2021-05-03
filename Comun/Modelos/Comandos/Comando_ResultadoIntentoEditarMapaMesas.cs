
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

	public class Comando_ResultadoIntentoEditarMapaMesas : Comando
	{
		private const TiposComando TipoComandoInit = TiposComando.ResultadoIntentoEditarMapaMesas;

		[JsonProperty("1")] public ResultadosIntentoEditarMapaMesas ResultadoIntentoEditarMapaMesas { get; private set; }

		private void InicializarPropiedades(ResultadosIntentoEditarMapaMesas ResultadoIntentoEditarMapaMesas)
		{
			this.ResultadoIntentoEditarMapaMesas = ResultadoIntentoEditarMapaMesas;
		}

		public Comando_ResultadoIntentoEditarMapaMesas(ResultadosIntentoEditarMapaMesas ResultadoIntentoEditarMapaMesas)
			: base(TipoComandoInit)
		{
			InicializarPropiedades(ResultadoIntentoEditarMapaMesas);
		}

		[JsonConstructor]
		private Comando_ResultadoIntentoEditarMapaMesas(TiposComando TipoComandoJson, ResultadosIntentoEditarMapaMesas ResultadoIntentoEditarMapaMesas)
			: base(TipoComandoJson)
		{
			InicializarPropiedades(ResultadoIntentoEditarMapaMesas);
		}
	}
}
