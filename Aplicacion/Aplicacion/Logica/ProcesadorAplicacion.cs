
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

		//private string Procesar_XXXXX(Comando_XXXXX Comando)
		//{
		//	//
		//}
	}
}
