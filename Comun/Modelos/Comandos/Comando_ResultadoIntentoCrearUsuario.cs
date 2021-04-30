
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

	public class Comando_ResultadoIntentoCrearUsuario : Comando
	{
		private const TiposComando TipoComandoInit = TiposComando.ResultadoIntentoCrearUsuario;

		[JsonProperty("1")] public ResultadosIntentoCrearUsuario ResultadoIntentoCrearUsuario { get; private set; }

		private void InicializarPropiedades(ResultadosIntentoCrearUsuario ResultadoIntentoCrearUsuario)
		{
			this.ResultadoIntentoCrearUsuario = ResultadoIntentoCrearUsuario;
		}

		public Comando_ResultadoIntentoCrearUsuario(ResultadosIntentoCrearUsuario ResultadoIntentoCrearUsuario)
			: base(TipoComandoInit)
		{
			InicializarPropiedades(ResultadoIntentoCrearUsuario);
		}

		[JsonConstructor]
		private Comando_ResultadoIntentoCrearUsuario(TiposComando TipoComandoJson, ResultadosIntentoCrearUsuario ResultadoIntentoCrearUsuario)
			: base(TipoComandoJson)
		{
			InicializarPropiedades(ResultadoIntentoCrearUsuario);
		}
	}
}
