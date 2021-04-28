
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
			: base(ComandoString) { }
	}
}
