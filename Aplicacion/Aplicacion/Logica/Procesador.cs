
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
				case TiposComando.ResultadoIniciarSesion:
				{
					Procesar_ResultadoIniciarSesion(
						Comando.DeJson
							<Comando_ResultadoIniciarSesion>
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

				case TiposComando.ResultadoCrearUsuario:
				{
					Procesar_ResultadoCrearUsuario(
						Comando.DeJson
							<Comando_ResultadoCrearUsuario>
								(ComandoJson));

					break;
				}

				case TiposComando.ResultadoEliminarUsuario:
				{
					Procesar_ResultadoEliminarUsuario(
						Comando.DeJson
							<Comando_ResultadoEliminarUsuario>
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

				case TiposComando.ResultadoEditarMapaMesas:
				{
					Procesar_ResultadoEditarMapaMesas(
						Comando.DeJson
							<Comando_ResultadoEditarMapaMesas>
								(ComandoJson));

					break;
				}

				case TiposComando.ResultadoCrearMesa:
				{
					Procesar_ResultadoCrearMesa(
						Comando.DeJson
							<Comando_ResultadoCrearMesa>
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

		private async void Procesar_ResultadoIniciarSesion(Comando_ResultadoIniciarSesion Comando)
		{
			switch(Comando.ResultadoIniciarSesion)
			{
				case ResultadosIniciarSesion.Correcto:
				{
					Global.UsuarioActual = Comando.UsuarioActual;

					await Device.InvokeOnMainThreadAsync(async () => 
						await Shell.Current.GoToAsync("//Principal") );

					UserDialogs.Instance.HideLoading();

					break;
				}
				case ResultadosIniciarSesion.UsuarioNoExiste:
				{
					UserDialogs.Instance.HideLoading();

					UserDialogs.Instance.Alert("El usuario no existe", "Alerta", "Aceptar");

					break;
				}
				case ResultadosIniciarSesion.ContrasenaIncorrecta:
				{
					UserDialogs.Instance.HideLoading();

					UserDialogs.Instance.Alert("Contraseña incorrecta", "Alerta", "Aceptar");

					break;
				}
				case ResultadosIniciarSesion.UsuarioYaConectado:
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

		private void Procesar_ResultadoCrearUsuario(Comando_ResultadoCrearUsuario Comando)
		{
			UserDialogs.Instance.HideLoading();

			if(Comando.ResultadoCrearUsuario == ResultadosCrearUsuario.Correcto)
			{
				Usuarios.Instancia.RefrescarUsuarios();

				UserDialogs.Instance.Alert("Usuario creado correctamente", "Información", "Aceptar");
			}
			else
			{
				UserDialogs.Instance.Alert("Alguien ha creado un usuario con el mismo Nombre de Usuario antes de que se haya introducido el que has creado, por lo que no se ha añadido el tuyo", "Error", "Aceptar");
			}
		}

		private void Procesar_ResultadoEliminarUsuario(Comando_ResultadoEliminarUsuario Comando)
		{
			UserDialogs.Instance.HideLoading();

			if(Comando.ResultadoEliminarUsuario == ResultadosEliminarUsuario.Correcto)
			{
				UserDialogs.Instance.Alert("Usuario eliminado correctamente", "Información", "Aceptar");

				Usuarios.Instancia.RefrescarUsuarios();
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

		private void Procesar_ResultadoEditarMapaMesas(Comando_ResultadoEditarMapaMesas Comando)
		{
			UserDialogs.Instance.HideLoading();

			switch(Comando.ResultadoEditarMapaMesas)
			{
				case ResultadosEditarMapaMesas.Correcto:
				{
					Mesas.Instancia.PedirMesas();
					break;
				}
				case ResultadosEditarMapaMesas.MaximoColumnas:
				{
					UserDialogs.Instance.Alert("Máximo de columnas alcanzado", "Error", "Aceptar");
					break;
				}
				case ResultadosEditarMapaMesas.MinimoColumnas:
				{
					UserDialogs.Instance.Alert("Mínimo de columnas alcanzado", "Error", "Aceptar");
					break;
				}
				case ResultadosEditarMapaMesas.MaximoFilas:
				{
					UserDialogs.Instance.Alert("Máximo de filas alcanzado", "Error", "Aceptar");
					break;
				}
				case ResultadosEditarMapaMesas.MinimoFilas:
				{
					UserDialogs.Instance.Alert("Mínimo de filas alcanzado", "Error", "Aceptar");
					break;
				}
				case ResultadosEditarMapaMesas.MesaBloquea:
				{
					UserDialogs.Instance.Alert("Hay mesas que bloquean las celdas a eliminar", "Error", "Aceptar");
					break;
				}
			}
		}

		private void Procesar_ResultadoCrearMesa(Comando_ResultadoCrearMesa Comando)
		{
			UserDialogs.Instance.HideLoading();

			switch(Comando.ResultadoCrearMesa)
			{
				case ResultadosCrearMesa.Correcto:
				{
					Mesas.Instancia.PedirMesas();
					UserDialogs.Instance.Alert("Mesa creada correctamente", "Información", "Aceptar");
					break;
				}
				case ResultadosCrearMesa.MesaYaExisteConMismoNumero:
				{
					UserDialogs.Instance.Alert("Alguien ha creado una mesa con el mismo Número antes de que se haya introducido la que has creado, por lo que no se ha añadido la tuya", "Error", "Aceptar");
					break;
				}
				case ResultadosCrearMesa.MesaYaExisteConMismoSitio:
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
