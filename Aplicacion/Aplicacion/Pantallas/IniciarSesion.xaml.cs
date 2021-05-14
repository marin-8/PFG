
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;
using System.Text.RegularExpressions;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

using Acr.UserDialogs;

using PFG.Comun;

namespace PFG.Aplicacion
{
	public partial class IniciarSesion : ContentPage
	{
		private readonly Regex FormatoIP = new(@"\b((25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)(\.|$)){4}\b");

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

		private async void IniciarSesion_Clicked(object sender, EventArgs e)
		{
			string ipGestor = IPGestor.Text;
			string usuario = Usuario.Text;
			string contrasena = Contrasena.Text;

			if(         ipGestor.Equals("")) { await DisplayAlert("Alerta", "IP del Gestor vacía. Este campo es obligatorio",    "Aceptar"); return; }
			if(          usuario.Equals("")) { await DisplayAlert("Alerta", "Usuario vacío. Este campo es obligatorio",          "Aceptar"); return; }
			if(       contrasena.Equals("")) { await DisplayAlert("Alerta", "Contraseña vacía. Este campo es obligatorio",       "Aceptar"); return; }

			if(								  !FormatoIP.IsMatch(ipGestor)) { await DisplayAlert("Alerta", "La IP introducida no es válida",                                "Aceptar"); return; }
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

		private async void Procesar_ResultadoIniciarSesion(Comando_ResultadoIniciarSesion Comando)
		{
			switch(Comando.ResultadoIniciarSesion)
			{
				case ResultadosIniciarSesion.Correcto:
				{
					Global.UsuarioActual = Comando.UsuarioActual;

					AppShell.ConfigurarPantallasAMostrar(Global.UsuarioActual.Rol);

					await Device.InvokeOnMainThreadAsync(async () => 
						await Shell.Current.GoToAsync("//Principal") );

					Tareas.RefrescarTareasPersonalesDesdeFuera();

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
				case ResultadosIniciarSesion.JornadaNoComenzada:
				{
					await UserDialogs.Instance.AlertAsync("La Jornada no ha comenzado", "Alerta", "Aceptar");

					break;
				}
				case ResultadosIniciarSesion.UsuarioYaConectado:
				{
					await UserDialogs.Instance.AlertAsync("Usuario ya conectado", "Alerta", "Aceptar");

					break;
				}
			}
		}
	}
}
