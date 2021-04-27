
using System;

namespace PFG.Comun
{
	public class Comando_ResultadoIntentoIniciarSesion : Comando
	{
		public bool Correcto { get; private set; }
		public ResultadosIntentoIniciarSesion ResultadIntentoIniciarSesion { get; private set; }
		public Roles Rol { get; private set; }

		public Comando_ResultadoIntentoIniciarSesion(bool Correcto, ResultadosIntentoIniciarSesion ResultadIntentoIniciarSesion, Roles Rol)
			: base(TiposComando.ResultadoDelIntentoDeIniciarSesion)
		{
			this.Correcto = Correcto;
			this.ResultadIntentoIniciarSesion = ResultadIntentoIniciarSesion;
			this.Rol = Rol;
		}

		public Comando_ResultadoIntentoIniciarSesion(string ComandoString)
			: base(TiposComando.ResultadoDelIntentoDeIniciarSesion)
		{
			var parametrosComando = ComandoString.Split(',');

			Correcto = parametrosComando[1].Equals("1") ? true : false;
			ResultadIntentoIniciarSesion = (ResultadosIntentoIniciarSesion)Enum.Parse(typeof(ResultadosIntentoIniciarSesion), parametrosComando[2]);
			Rol = (Roles)Enum.Parse(typeof(Roles), parametrosComando[3]);
		}

		public static string ParametrosToString(bool Correcto, ResultadosIntentoIniciarSesion ResultadIntentoIniciarSesion, Roles Rol)
		{
			return $"{(byte)TiposComando.ResultadoDelIntentoDeIniciarSesion},{(Correcto ? "1" : "0")},{(byte)ResultadIntentoIniciarSesion},{(byte)Rol}";
		}
	}
}
