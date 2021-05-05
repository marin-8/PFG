
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

using Acr.UserDialogs;

using PFG.Comun;

namespace PFG.Aplicacion
{
	public class ProcesadorAplicacion
	{
		public ProcesadorAplicacion()
		{

		}

		public string Procesar(string IP, string ComandoJson)
		{
			var tipoComando = Comando.Get_TipoComando_De_Json(ComandoJson);

			string comandoRespuesta = "OK";

			switch(tipoComando)
			{
				case TiposComando.Error:
				{
					Procesar_Error(ComandoJson);

					break;
				}

				//case TiposComando.XXXXX:
				//{
				//	comandoRespuesta =
				//		Procesar_XXXXX(
				//			Comando.DeJson
				//				<Comando_XXXXX>
				//					(ComandoJson));

				//	break;
				//}

				default:
				{
					UserDialogs.Instance.Alert("Se ha recibido un comando que no se puede procesar. Contacta con el desarollador para que solucione el problema", "Alerta", "Aceptar");

					break;
				}
			}

			return comandoRespuesta;
		}

		private static void Procesar_Error(string ComandoJson)
		{
			try
			{
				var comando = Comando.DeJson<Comando_Error>(ComandoJson);
				UserDialogs.Instance.Alert(comando.Mensaje, "Error en el Gestor", "Aceptar");
			}
			catch
			{
				UserDialogs.Instance.Alert("Ha ocurrido un error recibiendo un comando del Gestor.\nSi esto vuelve a ocurrir, por favor contacta con el desarrollador.\nGracias",  "Error en la Aplicación", "Aceptar");
			}
		}

		//private string Procesar_XXXXX(Comando_XXXXX Comando)
		//{
		//	//
		//}
	}
}
