
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
		private const TiposComando TipoComandoInit = TiposComando.ResultadoDelIntentoDeIniciarSesion;

		[JsonProperty("1")] public ResultadosIntentoIniciarSesion ResultadIntentoIniciarSesion { get; private set; }
		[JsonProperty("2")] public Roles Rol { get; private set; }

		private void InicializarPropiedades(ResultadosIntentoIniciarSesion ResultadIntentoIniciarSesion, Roles Rol)
		{
			this.ResultadIntentoIniciarSesion = ResultadIntentoIniciarSesion;
			this.Rol = Rol;
		}

		public Comando_ResultadoIntentoIniciarSesion(ResultadosIntentoIniciarSesion ResultadIntentoIniciarSesion, Roles Rol)
			: base(TipoComandoInit)
		{
			InicializarPropiedades(ResultadIntentoIniciarSesion, Rol);
		}

		[JsonConstructor]
		private Comando_ResultadoIntentoIniciarSesion(TiposComando TipoComandoJson, ResultadosIntentoIniciarSesion ResultadIntentoIniciarSesion, Roles Rol)
			: base(TipoComandoJson)
		{
			InicializarPropiedades(ResultadIntentoIniciarSesion, Rol);
		}
	}
}
