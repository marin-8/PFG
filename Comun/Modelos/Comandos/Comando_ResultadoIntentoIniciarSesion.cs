
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

	public class Comando_ResultadoIntentoIniciarSesion : Comando
	{
		private const TiposComando TipoComandoInit = TiposComando.ResultadoIntentoIniciarSesion;

		[JsonProperty("1")] public ResultadosIntentoIniciarSesion ResultadoIntentoIniciarSesion { get; private set; }
		[JsonProperty("2")] public Usuario UsuarioActual { get; private set; }

		private void InicializarPropiedades(ResultadosIntentoIniciarSesion ResultadoIntentoIniciarSesion, Usuario UsuarioActual)
		{
			this.ResultadoIntentoIniciarSesion = ResultadoIntentoIniciarSesion;
			this.UsuarioActual = UsuarioActual;
		}

		public Comando_ResultadoIntentoIniciarSesion(ResultadosIntentoIniciarSesion ResultadoIntentoIniciarSesion, Usuario UsuarioActual)
			: base(TipoComandoInit)
		{
			InicializarPropiedades(ResultadoIntentoIniciarSesion, UsuarioActual);
		}

		[JsonConstructor]
		private Comando_ResultadoIntentoIniciarSesion(TiposComando TipoComandoJson, ResultadosIntentoIniciarSesion ResultadoIntentoIniciarSesion, Usuario UsuarioActual)
			: base(TipoComandoJson)
		{
			InicializarPropiedades(ResultadoIntentoIniciarSesion, UsuarioActual);
		}
	}
}
