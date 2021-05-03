
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

				case TiposComando.ResultadoIntentoEliminarUsuario:
				{
					Procesar_ResultadoIntentoEliminarUsuario(
						Comando.DeJson
							<Comando_ResultadoIntentoEliminarUsuario>
								(ComandoJson));

					break;
				}

				case TiposComando.MandarMesas:
				{
					Procesar_MandarMesas(
						Comando.DeJson
							<Comando_MandarMesas>
								(ComandoJson));

					break;
				}

				case TiposComando.ResultadoIntentoEditarMapaMesas:
				{
					Procesar_ResultadoIntentoEditarMapaMesas(
						Comando.DeJson
							<Comando_ResultadoIntentoEditarMapaMesas>
								(ComandoJson));

					break;
				}

				case TiposComando.ResultadoIntentoCrearMesa:
				{
					Procesar_ResultadoIntentoCrearMesa(
						Comando.DeJson
							<Comando_ResultadoIntentoCrearMesa>
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
					Global.UsuarioActual = Comando.UsuarioActual;

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
			Usuarios.Instancia.UsuariosLocal.Clear();
			
			foreach(var usuario in Comando.Usuarios)
				Usuarios.Instancia.UsuariosLocal.Add(usuario);

			Usuarios.Instancia.DejarDeRefrescarLista();

			UserDialogs.Instance.HideLoading();
		}

		private async void Procesar_ResultadoIntentoCrearUsuario(Comando_ResultadoIntentoCrearUsuario Comando)
		{
			UserDialogs.Instance.HideLoading();

			if(Comando.ResultadoIntentoCrearUsuario == ResultadosIntentoCrearUsuario.Correcto)
			{
				await Usuarios.Instancia.RefrescarUsuarios();

				UserDialogs.Instance.Alert("Usuario creado correctamente", "Información", "Aceptar");
			}
			else
			{
				UserDialogs.Instance.Alert("Alguien ha creado un usuario con el mismo Nombre de Usuario antes de que se haya introducido el que has creado, por lo que no se ha añadido el tuyo", "Error", "Aceptar");
			}
		}

		private async void Procesar_ResultadoIntentoEliminarUsuario(Comando_ResultadoIntentoEliminarUsuario Comando)
		{
			UserDialogs.Instance.HideLoading();

			if(Comando.ResultadoEliminarUsuario == ResultadosIntentoEliminarUsuario.Correcto)
			{
				UserDialogs.Instance.Alert("Usuario eliminado correctamente", "Información", "Aceptar");

				await Usuarios.Instancia.RefrescarUsuarios();
			}
			else
			{
				UserDialogs.Instance.Alert("No se puede eliminar el usuario porque está conectado", "Error", "Aceptar");
			}
		}

		private async void Procesar_MandarMesas(Comando_MandarMesas Comando)
		{
			await Mesas.Instancia.ActualizarMesas(
				Comando.AnchoGrid,
				Comando.AltoGrid,
				Comando.Mesas);

			UserDialogs.Instance.HideLoading();
		}

		private async void Procesar_ResultadoIntentoEditarMapaMesas(Comando_ResultadoIntentoEditarMapaMesas Comando)
		{
			UserDialogs.Instance.HideLoading();

			switch(Comando.ResultadoIntentoEditarMapaMesas)
			{
				case ResultadosIntentoEditarMapaMesas.Correcto:
				{
					await Mesas.Instancia.PedirMesas();
					break;
				}
				case ResultadosIntentoEditarMapaMesas.MaximoColumnas:
				{
					UserDialogs.Instance.Alert("Máximo de columnas alcanzado", "Error", "Aceptar");
					break;
				}
				case ResultadosIntentoEditarMapaMesas.MinimoColumnas:
				{
					UserDialogs.Instance.Alert("Mínimo de columnas alcanzado", "Error", "Aceptar");
					break;
				}
				case ResultadosIntentoEditarMapaMesas.MaximoFilas:
				{
					UserDialogs.Instance.Alert("Máximo de filas alcanzado", "Error", "Aceptar");
					break;
				}
				case ResultadosIntentoEditarMapaMesas.MinimoFilas:
				{
					UserDialogs.Instance.Alert("Mínimo de filas alcanzado", "Error", "Aceptar");
					break;
				}
			}
		}

		private async void Procesar_ResultadoIntentoCrearMesa(Comando_ResultadoIntentoCrearMesa Comando)
		{
			UserDialogs.Instance.HideLoading();

			switch(Comando.ResultadoIntentoCrearMesa)
			{
				case ResultadosIntentoCrearMesa.Correcto:
				{
					await Mesas.Instancia.PedirMesas();
					UserDialogs.Instance.Alert("Mesa creada correctamente", "Información", "Aceptar");
					break;
				}
				case ResultadosIntentoCrearMesa.MesaYaExisteConMismoNumero:
				{
					UserDialogs.Instance.Alert("Alguien ha creado una mesa con el mismo Número antes de que se haya introducido la que has creado, por lo que no se ha añadido la tuya", "Error", "Aceptar");
					break;
				}
				case ResultadosIntentoCrearMesa.MesaYaExisteConMismoSitio:
				{
					UserDialogs.Instance.Alert("Alguien ha creado una mesa en el mismo Sitio antes de que se haya introducido la que has creado, por lo que no se ha añadido la tuya", "Error", "Aceptar");
					break;
				}
			}
		}

		//private void Procesar_XXXXX(Comando_XXXXX Comando)
		//{
		//	//
		//}
	}
}
