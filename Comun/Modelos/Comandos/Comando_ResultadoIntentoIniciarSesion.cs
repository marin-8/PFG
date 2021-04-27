
using System;

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
			: base(TiposComando.ResultadoDelIntentoDeIniciarSesion)
		{
			var parametrosComando = ComandoString.Split(',');

			ResultadIntentoIniciarSesion = (ResultadosIntentoIniciarSesion)Enum.Parse(typeof(ResultadosIntentoIniciarSesion), parametrosComando[1]);
			Rol = (Roles)Enum.Parse(typeof(Roles), parametrosComando[2]);
		}

		public static string ParametrosToString(ResultadosIntentoIniciarSesion ResultadIntentoIniciarSesion, Roles Rol)
		{
			return $"{(byte)TiposComando.ResultadoDelIntentoDeIniciarSesion},{(byte)ResultadIntentoIniciarSesion},{(byte)Rol}";
		}
	}
}
