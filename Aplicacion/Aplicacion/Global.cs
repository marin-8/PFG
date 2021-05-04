
using System;
using System.Collections.Generic;
using System.Text;

using Acr.UserDialogs;

using PFG.Comun;

namespace PFG.Aplicacion
{
	public static class Global
	{
		public static string IPGestor = "";

		public static Usuario UsuarioActual;

		public static ControladorRed Servidor;
		public static ProcesadorAplicacion ProcesadorMensajesRecibidos;

		public static void Procesar_ResultadoGenerico(Comando_ResultadoGenerico Comando, Action FuncionCuandoCorrecto, Action FuncionCuandoErroneo=null)
		{
			if(Comando.Correcto)
			{
				FuncionCuandoCorrecto();
			}
			else
			{
				FuncionCuandoErroneo?.Invoke();

				UserDialogs.Instance.Alert(Comando.Mensaje, "Error", "Aceptar");
			}
		}
	}
}
