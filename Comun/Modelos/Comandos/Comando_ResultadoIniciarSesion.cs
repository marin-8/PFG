
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

	public class Comando_ResultadoIniciarSesion : Comando
	{
		private const TiposComando TipoComandoInit = TiposComando.ResultadoIniciarSesion;

		[JsonProperty("1")] public ResultadosIniciarSesion ResultadoIniciarSesion { get; private set; }
		[JsonProperty("2")] public Usuario UsuarioActual { get; private set; }

		private void InicializarPropiedades(ResultadosIniciarSesion ResultadoIniciarSesion, Usuario UsuarioActual)
		{
			this.ResultadoIniciarSesion = ResultadoIniciarSesion;
			this.UsuarioActual = UsuarioActual;
		}

		public Comando_ResultadoIniciarSesion(ResultadosIniciarSesion ResultadoIniciarSesion, Usuario UsuarioActual)
			: base(TipoComandoInit)
		{
			InicializarPropiedades(ResultadoIniciarSesion, UsuarioActual);
		}

		[JsonConstructor]
		#pragma warning disable IDE0051
		private Comando_ResultadoIniciarSesion(TiposComando TipoComandoJson, ResultadosIniciarSesion ResultadoIniciarSesion, Usuario UsuarioActual)
		#pragma warning restore IDE0051
			: base(TipoComandoJson)
		{
			InicializarPropiedades(ResultadoIniciarSesion, UsuarioActual);
		}
	}
}
