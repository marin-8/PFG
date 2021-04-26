
namespace PFG.Comun
{
	public class Comando_IniciarSesion : Comando
	{
		public string Usuario { get; private set; }
		public string Contrasena { get; private set; }

		public Comando_IniciarSesion(string Usuario, string Contrasena)
			: base(TiposComando.IniciarSesion)
		{
			this.Usuario = Usuario;
			this.Contrasena = Contrasena;
		}

		public Comando_IniciarSesion(string ComandoString)
			: base(TiposComando.IniciarSesion)
		{
			var parametrosComando = ComandoString.Split(',');

			Usuario = parametrosComando[1];
			Contrasena = parametrosComando[2];
		}

		public static string ParametrosToString(string Usuario, string Contrasena)
		{
			return $"{(ushort)TiposComando.IniciarSesion},{Usuario},{Contrasena}";
		}
	}
}
