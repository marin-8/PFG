
using System.Threading.Tasks;

using Xamarin.Forms;

using Acr.UserDialogs;

using PFG.Comun;

namespace PFG.Aplicacion
{
	public static class ProcesadorAplicacion
	{
		#pragma warning disable IDE0060
		public static string ProcesarComandosRecibidos(string IP, string ComandoJson)
		#pragma warning restore IDE0060
		{
			TiposComando tipoComando;

			try
			{
				tipoComando = Comando.Get_TipoComando_De_Json(ComandoJson);
			}
			catch
			{
				tipoComando =  TiposComando.Error;
			}

			string comandoRespuesta = new Comando_ResultadoGenerico(true, "Correcto").ToString();

			switch(tipoComando)
			{
				case TiposComando.Error:
				{
					Global.Get_TareasPersonales();

					break;
				}

				case TiposComando.JornadaTerminada:
				{
					Procesar_JornadaTerminada();

					break;
				}

				case TiposComando.EnviarTarea:
				{
					Procesar_EnviarTarea(
						Comando.DeJson
							<Comando_EnviarTarea>
								(ComandoJson));

					break;
				}

				case TiposComando.RefrescarDisponibilidadArticulos:
				{
					Procesar_RefrescarDisponibilidadArticulos();

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

		private static async void Procesar_JornadaTerminada()
		{
			UserDialogs.Instance.ShowLoading("Cerrando sesión...");

			Shell.Current.FlyoutIsPresented = false;

			Global.UsuarioActual = null;

			await Shell.Current.GoToAsync("//IniciarSesion");

			await Task.Delay(200);

			UserDialogs.Instance.HideLoading();
		}

		private static void Procesar_EnviarTarea(Comando_EnviarTarea Comando)
		{
			lock(Global.TareasPersonalesLock)
			{
				Global.TareasPersonales.Add(Comando.Tarea);

				Global.TareasPersonales.Ordenar();
			}
		}

		private static async void Procesar_RefrescarDisponibilidadArticulos()
		{
			await Device.InvokeOnMainThreadAsync(async () => 
				await Global.Get_Articulos() );
		}

		//private string Procesar_XXXXX(Comando_XXXXX Comando)
		//{
		//	//
		//}
	}
}
