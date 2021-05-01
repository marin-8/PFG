
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

	public class Comando_ResultadoIntentoEliminarUsuario : Comando
	{
		private const TiposComando TipoComandoInit = TiposComando.ResultadoIntentoEliminarUsuario;
		[JsonProperty("1")] public ResultadosIntentoEliminarUsuario ResultadoEliminarUsuario { get; private set; }

		private void InicializarPropiedades(ResultadosIntentoEliminarUsuario ResultadoEliminarUsuario)
		{
			this.ResultadoEliminarUsuario = ResultadoEliminarUsuario;
		}

		public Comando_ResultadoIntentoEliminarUsuario(ResultadosIntentoEliminarUsuario ResultadoEliminarUsuario)
			: base(TipoComandoInit)
		{
			InicializarPropiedades(ResultadoEliminarUsuario);
		}

		[JsonConstructor]
		private Comando_ResultadoIntentoEliminarUsuario(TiposComando TipoComandoJson, ResultadosIntentoEliminarUsuario ResultadoEliminarUsuario)
			: base(TipoComandoJson)
		{
			InicializarPropiedades(ResultadoEliminarUsuario);
		}
	}
}
