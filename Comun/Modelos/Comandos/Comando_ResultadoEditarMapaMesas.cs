
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

	public class Comando_ResultadoEditarMapaMesas : Comando
	{
		private const TiposComando TipoComandoInit = TiposComando.ResultadoEditarMapaMesas;

		[JsonProperty("1")] public ResultadosEditarMapaMesas ResultadoEditarMapaMesas { get; private set; }

		private void InicializarPropiedades(ResultadosEditarMapaMesas ResultadoEditarMapaMesas)
		{
			this.ResultadoEditarMapaMesas = ResultadoEditarMapaMesas;
		}

		public Comando_ResultadoEditarMapaMesas(ResultadosEditarMapaMesas ResultadoEditarMapaMesas)
			: base(TipoComandoInit)
		{
			InicializarPropiedades(ResultadoEditarMapaMesas);
		}

		[JsonConstructor]
		private Comando_ResultadoEditarMapaMesas(TiposComando TipoComandoJson, ResultadosEditarMapaMesas ResultadoEditarMapaMesas)
			: base(TipoComandoJson)
		{
			InicializarPropiedades(ResultadoEditarMapaMesas);
		}
	}
}
