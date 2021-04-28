
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
	public class Procesador
	{
		public Procesador()
		{

		}

		public void Procesar(string IP, string ComandoString)
		{
			// TODO - Procesar (App)

			var parametrosComando = ComandoString.Split(',');
			var tipoComando = (TiposComando)Enum.Parse(typeof(TiposComando), parametrosComando[0]);

			switch(tipoComando)
			{
				case TiposComando.ResultadoDelIntentoDeIniciarSesion:
				{
					Procesar_ResultadoDelIntentoDeIniciarSesion(
						new Comando_ResultadoIntentoIniciarSesion(ComandoString));

					break;
				}

				//case TiposComando.XXXXX:
				//{
				//	var comando = new Comando_XXXXX(ComandoString);

				//	Procesar_ResultadoDelIntentoDeIniciarSesion(
				//		new Comando_XXXXX(ComandoString));

				//	break;
				//}

				default:
				{
					UserDialogs.Instance.Alert("Se ha recibido un comando que no se puede procesar. Tranqui, no pasa nada serio. Contacta con el desarollador e infórmale sobre cómo ha pasado", "Información", "Aceptar");

					break;
				}
			}
		}

		private async void Procesar_ResultadoDelIntentoDeIniciarSesion(Comando_ResultadoIntentoIniciarSesion Comando)
		{
			switch(Comando.ResultadIntentoIniciarSesion)
			{
				case ResultadosIntentoIniciarSesion.Correcto:
				{
					Global.RolActual = Comando.Rol;

					await Device.InvokeOnMainThreadAsync(async () =>
						await Shell.Current.GoToAsync("//Principal") );

					return;
				}
				case ResultadosIntentoIniciarSesion.UsuarioNoExiste:
				{
					UserDialogs.Instance.Alert("El usuario no existe", "Alerta", "Aceptar");

					return;
				}
				case ResultadosIntentoIniciarSesion.ContrasenaIncorrecta:
				{
					UserDialogs.Instance.Alert("Contraseña incorrecta", "Alerta", "Aceptar");

					return;
				}
				case ResultadosIntentoIniciarSesion.UsuarioYaConectado:
				{
					UserDialogs.Instance.Alert("Usuario ya conectado", "Alerta", "Aceptar");

					return;
				}
			}
		}

		//private void Procesar_XXXXX(Comando_XXXXX Comando)
		//{
		//	//
		//}
	}
}
