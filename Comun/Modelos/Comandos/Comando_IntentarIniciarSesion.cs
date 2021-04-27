
namespace PFG.Comun
{
	public class Comando_IntentarIniciarSesion : Comando
	{
		public string Usuario { get; private set; }
		public string Contrasena { get; private set; }

		public Comando_IntentarIniciarSesion(string Usuario, string Contrasena)
			: base(TiposComando.IntentarIniciarSesion)
		{
			this.Usuario = Usuario;
			this.Contrasena = Contrasena;
		}

		public Comando_IntentarIniciarSesion(string ComandoString)
			: base(TiposComando.IntentarIniciarSesion)
		{
			var parametrosComando = ComandoString.Split(',');

			Usuario = parametrosComando[1];
			Contrasena = parametrosComando[2];
		}

		public static string ParametrosToString(string Usuario, string Contrasena)
		{
			return $"{(byte)TiposComando.IntentarIniciarSesion},{Usuario},{Contrasena}";
		}
	}
}
