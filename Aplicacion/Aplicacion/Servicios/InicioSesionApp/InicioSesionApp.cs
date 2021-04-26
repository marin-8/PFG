
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using PFG.Comun;

namespace PFG.Aplicacion
{
	public static class InicioSesionApp
	{
		public static void IniciarSesion(string IPGestor, string Usuario, string Contrasena)
		{
			ControladorRed.Enviar
			(
				IPGestor,
				new MensajeInicioSesion
				
			);
		}
	}
}
