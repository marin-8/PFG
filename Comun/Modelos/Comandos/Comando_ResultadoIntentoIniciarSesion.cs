
namespace PFG.Comun
{
	public class Comando_ResultadoIntentoIniciarSesion : Comando
	{
		public ResultadosIntentoIniciarSesion ResultadIntentoIniciarSesion { get; private set; }
		public Roles Rol { get; private set; }

		public Comando_ResultadoIntentoIniciarSesion(ResultadosIntentoIniciarSesion ResultadIntentoIniciarSesion, Roles Rol)
			: base(TiposComando.ResultadoDelIntentoDeIniciarSesion)
		{
			this.ResultadIntentoIniciarSesion = ResultadIntentoIniciarSesion;
			this.Rol = Rol;
		}

		public Comando_ResultadoIntentoIniciarSesion(string ComandoString)
			: base(ComandoString) { }
	}
}
