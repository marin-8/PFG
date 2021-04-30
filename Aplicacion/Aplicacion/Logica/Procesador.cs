
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

		public void Procesar(string IP, string ComandoJson)
		{
			var tipoComando = Comando.Get_TipoComando_De_Json(ComandoJson);

			switch(tipoComando)
			{
				case TiposComando.ResultadoIntentoIniciarSesion:
				{
					Procesar_ResultadoDelIntentoDeIniciarSesion(
						Comando.DeJson
							<Comando_ResultadoIntentoIniciarSesion>
								(ComandoJson));

					break;
				}

				case TiposComando.MandarUsuarios:
				{
					Procesar_MandarUsuarios(
						Comando.DeJson
							<Comando_MandarUsuarios>
								(ComandoJson));

					break;
				}

				case TiposComando.ResultadoIntentoCrearUsuario:
				{
					Procesar_ResultadoIntentoCrearUsuario(
						Comando.DeJson
							<Comando_ResultadoIntentoCrearUsuario>
								(ComandoJson));

					break;
				}

				//case TiposComando.XXXXX:
				//{
				//	Procesar_XXXXX(
				//		Comando.DeJson
				//			<Comando_XXXXX>
				//				(ComandoJson));

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
			switch(Comando.ResultadoIntentoIniciarSesion)
			{
				case ResultadosIntentoIniciarSesion.Correcto:
				{
					Global.RolActual = Comando.Rol;

					await Device.InvokeOnMainThreadAsync(async () => 
						await Shell.Current.GoToAsync("//Principal") );

					UserDialogs.Instance.HideLoading();

					break;
				}
				case ResultadosIntentoIniciarSesion.UsuarioNoExiste:
				{
					UserDialogs.Instance.HideLoading();

					UserDialogs.Instance.Alert("El usuario no existe", "Alerta", "Aceptar");

					break;
				}
				case ResultadosIntentoIniciarSesion.ContrasenaIncorrecta:
				{
					UserDialogs.Instance.HideLoading();

					UserDialogs.Instance.Alert("Contraseña incorrecta", "Alerta", "Aceptar");

					break;
				}
				case ResultadosIntentoIniciarSesion.UsuarioYaConectado:
				{
					UserDialogs.Instance.HideLoading();

					UserDialogs.Instance.Alert("Usuario ya conectado", "Alerta", "Aceptar");

					break;
				}
			}
		}

		private void Procesar_MandarUsuarios(Comando_MandarUsuarios Comando)
		{
			Usuarios.UsuariosLocal.Clear();
			
			foreach(var usuario in Comando.Usuarios)
				Usuarios.UsuariosLocal.Add(usuario);

			UserDialogs.Instance.HideLoading();
		}

		private void Procesar_ResultadoIntentoCrearUsuario(Comando_ResultadoIntentoCrearUsuario Comando)
		{
			UserDialogs.Instance.HideLoading();

			if(Comando.ResultadoIntentoCrearUsuario == ResultadosIntentoCrearUsuario.Correcto)
			{
				if(Usuarios.nuevoUsuario != null)
				{
					Usuarios.UsuariosLocal.Add(Usuarios.nuevoUsuario);
					Usuarios.nuevoUsuario = null;
				}

				UserDialogs.Instance.Alert("Usuario creado correctamente", "Información", "Aceptar");
			}
			else
			{
				UserDialogs.Instance.Alert("Alguien ha creado un usuario con el mismo Nombre de Usuario antes de que se haya introducido el que has creado, por lo que no se ha añadido el tuyo", "Error", "Aceptar");
			}
		}

		//private void Procesar_XXXXX(Comando_XXXXX Comando)
		//{
		//	//
		//}
	}
}
