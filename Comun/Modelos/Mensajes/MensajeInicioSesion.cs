
namespace PFG.Comun
{
	public class MensajeInicioSesion : Mensaje
	{
		public string Usuario { get; private set; }
		public string Contrasena { get; private set; }

		public MensajeInicioSesion(string Usuario, string Contrasena)
			: base(TiposMensaje.InicioSesion)
		{
			this.Usuario = Usuario;
			this.Contrasena = Contrasena;
		}

		public MensajeInicioSesion(string Mensaje)
			: base(TiposMensaje.InicioSesion)
		{
			var parametros = Mensaje.Split(',');

			Usuario = parametros[1];
			Contrasena = parametros[2];
		}
	}
}
