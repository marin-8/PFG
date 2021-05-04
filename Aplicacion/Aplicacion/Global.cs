
using System;
using System.Collections.Generic;
using System.Text;

using PFG.Comun;

namespace PFG.Aplicacion
{
	public static class Global
	{
		public static string IPGestor = "";

		public static Usuario UsuarioActual;

		public static ControladorRed Servidor;
		public static ProcesadorAplicacion ProcesadorMensajesRecibidos;
	}
}
