
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

	public class Comando_ResultadoCrearUsuario : Comando
	{
		private const TiposComando TipoComandoInit = TiposComando.ResultadoCrearUsuario;

		[JsonProperty("1")] public ResultadosCrearUsuario ResultadoCrearUsuario { get; private set; }

		private void InicializarPropiedades(ResultadosCrearUsuario ResultadoCrearUsuario)
		{
			this.ResultadoCrearUsuario = ResultadoCrearUsuario;
		}

		public Comando_ResultadoCrearUsuario(ResultadosCrearUsuario ResultadoCrearUsuario)
			: base(TipoComandoInit)
		{
			InicializarPropiedades(ResultadoCrearUsuario);
		}

		[JsonConstructor]
		private Comando_ResultadoCrearUsuario(TiposComando TipoComandoJson, ResultadosCrearUsuario ResultadoCrearUsuario)
			: base(TipoComandoJson)
		{
			InicializarPropiedades(ResultadoCrearUsuario);
		}
	}
}
