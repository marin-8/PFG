
using System;
using System.Collections.Generic;
using System.Text;

using PFG.Comun;

namespace PFG.Aplicacion
{
	public static class Global
	{
		public static string IPGestor = "";

		public static string UsuarioActual = "";
		public static string ContrasenaActual = "";
		
		public static Roles RolActual = Roles.Ninguno;

		public static ControladorRed Servidor;
		public static Procesador ProcesadorMensajesRecibidos;
	}
}
