
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
		[JsonProperty("2")] public Roles Rol { get; private set; }

		private void InicializarPropiedades(ResultadosIntentoIniciarSesion ResultadoIntentoIniciarSesion, Roles Rol)
		{
			this.ResultadoIntentoIniciarSesion = ResultadoIntentoIniciarSesion;
			this.Rol = Rol;
		}

		public Comando_ResultadoIntentoIniciarSesion(ResultadosIntentoIniciarSesion ResultadoIntentoIniciarSesion, Roles Rol)
			: base(TipoComandoInit)
		{
			InicializarPropiedades(ResultadoIntentoIniciarSesion, Rol);
		}

		[JsonConstructor]
		private Comando_ResultadoIntentoIniciarSesion(TiposComando TipoComandoJson, ResultadosIntentoIniciarSesion ResultadoIntentoIniciarSesion, Roles Rol)
			: base(TipoComandoJson)
		{
			InicializarPropiedades(ResultadoIntentoIniciarSesion, Rol);
		}
	}
}
