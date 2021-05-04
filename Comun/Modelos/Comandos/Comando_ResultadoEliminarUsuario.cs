
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

	public class Comando_ResultadoEliminarUsuario : Comando
	{
		private const TiposComando TipoComandoInit = TiposComando.ResultadoEliminarUsuario;
		[JsonProperty("1")] public ResultadosEliminarUsuario ResultadoEliminarUsuario { get; private set; }

		private void InicializarPropiedades(ResultadosEliminarUsuario ResultadoEliminarUsuario)
		{
			this.ResultadoEliminarUsuario = ResultadoEliminarUsuario;
		}

		public Comando_ResultadoEliminarUsuario(ResultadosEliminarUsuario ResultadoEliminarUsuario)
			: base(TipoComandoInit)
		{
			InicializarPropiedades(ResultadoEliminarUsuario);
		}

		[JsonConstructor]
		private Comando_ResultadoEliminarUsuario(TiposComando TipoComandoJson, ResultadosEliminarUsuario ResultadoEliminarUsuario)
			: base(TipoComandoJson)
		{
			InicializarPropiedades(ResultadoEliminarUsuario);
		}
	}
}
