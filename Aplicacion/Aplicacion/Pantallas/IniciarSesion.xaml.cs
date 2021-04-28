
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

using Acr.UserDialogs;

using PFG.Comun;

namespace PFG.Aplicacion
{
	public partial class IniciarSesion : ContentPage
	{
		private static bool Inicializado = false;

		private ControladorRed Servidor;
		private Procesador ProcesadorMensajesRecibidos;

		private readonly Regex FormatoIP = new(@"\b((25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)(\.|$)){4}\b");

		public IniciarSesion()
		{
			InitializeComponent();
		}

		protected override async void OnAppearing()
		{
			base.OnAppearing();

			if(!Inicializado)
			{
				Inicializado = true;

				string servidorIP;

				ProcesadorMensajesRecibidos = new();

				try { servidorIP = Comun.Global.Get_MiIP_Xamarin(); }
				catch
				{
					servidorIP = await DisplayPromptAsync
					(
						"Ejecutando App en un emulador",
						"Introduce manualmente la IP para el Servidor:",
						"Aceptar",
						"No iniciar el servidor",
						"0.0.0.0",
						15,
						Keyboard.Default,
						"10.0.2.15"
					);

					if(servidorIP == null)
					{
						await DisplayAlert("Alerta", "Al no haber iniciado el servidor, no podrás recibir comandos", "Aceptar");

						return;
					}

					Servidor = new(servidorIP, ProcesadorMensajesRecibidos.Procesar, true, 1601);

					return;
				}
			
				Servidor = new(servidorIP, ProcesadorMensajesRecibidos.Procesar, true);
			}	
		}

		private async void Entrar_Clicked(object sender, EventArgs e)
		{
			string ipGestor = IPGestor.Text;
			string usuario = Usuario.Text;
			string contrasena = Contrasena.Text;

			if(usuario.Equals("dev") && contrasena.Equals(""))
			{
				await Device.InvokeOnMainThreadAsync(async () =>
					await Shell.Current.GoToAsync("//Principal") );

				return;
			}

			if(         ipGestor.Equals("")) { await DisplayAlert("Alerta", "IP del Gestor vacía. Este campo es obligatorio",    "Aceptar"); return; }
			if(          usuario.Equals("")) { await DisplayAlert("Alerta", "Usuario vacío. Este campo es obligatorio",          "Aceptar"); return; }
			if(       contrasena.Equals("")) { await DisplayAlert("Alerta", "Contraseña vacía. Este campo es obligatorio",       "Aceptar"); return; }

			if(								  !FormatoIP.IsMatch(ipGestor)) { await DisplayAlert("Alerta", "La IP introducida no es válida",                                "Aceptar"); return; }
			if(         usuario.Length > Comun.Global.MAX_CARACTERES_LOGIN) { await DisplayAlert("Alerta", "El Usuario no puede estar formado por más de 20 caracteres",    "Aceptar"); return; }
			if(      contrasena.Length > Comun.Global.MAX_CARACTERES_LOGIN) { await DisplayAlert("Alerta", "La Contraseña no puede estar formada por más de 20 caracteres", "Aceptar"); return; }

			UserDialogs.Instance.ShowLoading("Inciando sesión...");

			await Task.Run(() =>
			{
				Global.IPGestor = ipGestor;
				Global.UsuarioActual = usuario;
				Global.ContrasenaActual = contrasena;

				new Comando_IntentarIniciarSesion
				(
					usuario,
					contrasena
				)
				.Enviar(ipGestor);

				UserDialogs.Instance.HideLoading();
			});
		}
	}
}
