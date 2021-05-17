
using System;
using System.Threading.Tasks;

using Xamarin.Forms;

using Acr.UserDialogs;

using PFG.Comun;

namespace PFG.Aplicacion
{
	public partial class IniciarSesion : ContentPage
	{
	// ============================================================================================== //

		// Variables y constantes

	// ============================================================================================== //

		// Inicialización

		public IniciarSesion()
		{
			InitializeComponent();

			Shell.Current.Navigated += OnNavigatedTo;
		}

		private async void OnNavigatedTo(object sender, ShellNavigatedEventArgs e)
		{
			if(e.Current.Location.OriginalString.Contains(((BaseShellItem)Parent).Route.ToString()))
			{
				if(Global.UsuarioActual != null)
					await Shell.Current.GoToAsync("//Principal");
			}
		}

    // ============================================================================================== //

        // Eventos UI -> Barra navegación

    // ============================================================================================== //

        // Eventos UI -> Contenido

		private async void IniciarSesion_Clicked(object sender, EventArgs e)
		{
			string ipGestor = IPGestor.Text ?? "";
			string usuario = Usuario.Text ?? "";
			string contrasena = Contrasena.Text ?? "";

			if(         ipGestor == "") { await DisplayAlert("Alerta", "IP del Gestor vacía. Este campo es obligatorio",    "Aceptar"); return; }
			if(          usuario == "") { await DisplayAlert("Alerta", "Usuario vacío. Este campo es obligatorio",          "Aceptar"); return; }
			if(       contrasena == "") { await DisplayAlert("Alerta", "Contraseña vacía. Este campo es obligatorio",       "Aceptar"); return; }

			if(								        !ipGestor.EsIPValida()) { await DisplayAlert("Alerta", "La IP introducida no es válida",                                "Aceptar"); return; }
			if(         usuario.Length > Comun.Global.MAX_CARACTERES_LOGIN) { await DisplayAlert("Alerta", "El Usuario no puede estar formado por más de 20 caracteres",    "Aceptar"); return; }
			if(      contrasena.Length > Comun.Global.MAX_CARACTERES_LOGIN) { await DisplayAlert("Alerta", "La Contraseña no puede estar formada por más de 20 caracteres", "Aceptar"); return; }

			UserDialogs.Instance.ShowLoading("Intentando iniciar sesión...");

			var comandoRespuesta = await Task.Run(() =>
			{
				Global.IPGestor = ipGestor;

				string respuestaGestor = new Comando_IniciarSesion(usuario,contrasena).Enviar(Global.IPGestor);
				return Comando.DeJson<Comando_ResultadoIniciarSesion>(respuestaGestor);
			});
			Procesar_ResultadoIniciarSesion(comandoRespuesta); 

			UserDialogs.Instance.HideLoading();
		}

    // ============================================================================================== //

        // Métodos Helper

	// ============================================================================================== //

        // Métodos Procesar

		private async void Procesar_ResultadoIniciarSesion(Comando_ResultadoIniciarSesion Comando)
		{
			switch(Comando.ResultadoIniciarSesion)
			{
				case ResultadosIniciarSesion.Correcto:
				{
					Global.Servidor.EmpezarEscucha();

					Global.UsuarioActual = Comando.UsuarioActual;

					AppShell.ConfigurarPantallasAMostrar(Global.UsuarioActual.Rol);

					if(Global.UsuarioActual.Rol != Roles.Administrador)
					{
						Tareas.RefrescarTareasPersonalesDesdeFuera();

						await Device.InvokeOnMainThreadAsync(async () => 
							await Shell.Current.GoToAsync("//Principal") );
					}
					else
					{
						await Global.Get_Articulos();

						await Device.InvokeOnMainThreadAsync(async () => 
							await Shell.Current.GoToAsync("//Carta") );
					}

					break;
				}
				case ResultadosIniciarSesion.UsuarioNoExiste:
				{
					await UserDialogs.Instance.AlertAsync("El usuario no existe", "Alerta", "Aceptar");

					break;
				}
				case ResultadosIniciarSesion.ContrasenaIncorrecta:
				{
					await UserDialogs.Instance.AlertAsync("Contraseña incorrecta", "Alerta", "Aceptar");

					break;
				}
				case ResultadosIniciarSesion.JornadaEnEstadoNoPermitido:
				{
					await UserDialogs.Instance.AlertAsync
					(
						"Estado de la Jornada no permitido.\n\n" +
							"Los trabajadores no pueden iniciar sesión si la Jornada no ha comenzado.\n\n" +
							"El Administrador no puede iniciar sesión si la Jornada ha comenzado.",
						"Alerta",
						"Aceptar");

					break;
				}
				case ResultadosIniciarSesion.UsuarioYaConectado:
				{
					await UserDialogs.Instance.AlertAsync("Usuario ya conectado", "Alerta", "Aceptar");

					break;
				}
			}
		}

	// ============================================================================================== //
	}
}
