
using System;

namespace PFG.Comun
{
	public class MensajeInicioSesion : Mensaje
	{
		public string Usuario { get; private set; }
		public string Contrasena { get; private set; }

		public MensajeInicioSesion(string Usuario, string Contrasena)
			: base(TiposMensaje.InicioSesion)
		{
			if(Usuario.Contains(",") || Contrasena.Contains(","))
				throw new Exception("Los parámetros no puden contener el carácter ','");

			this.Usuario = Usuario;
			this.Contrasena = Contrasena;
		}

		public MensajeInicioSesion(string Mensaje)
			: base(TiposMensaje.InicioSesion)
		{
			var parametros = Mensaje.Split(',');

			if(parametros.Length != 3)
				throw new Exception("El número de parámetros que contiene el Mensaje origen es erróneo");

			if((TiposMensaje)Enum.Parse(typeof(TiposMensaje), parametros[0]) != TiposMensaje.InicioSesion)
				throw new Exception("Mensaje origen con TipoMensaje distinto al objetivo");

			Usuario = parametros[1];
			Contrasena = parametros[2];
		}

		public static string ParametrosToString(string Usuario, string Contrasena)
		{
			if(Usuario.Contains(",") || Contrasena.Contains(","))
				throw new Exception("Los parámetros no puden contener el carácter ','");

			return $"{(ushort)TiposMensaje.InicioSesion},{Usuario},{Contrasena}";
		}
	}
}
