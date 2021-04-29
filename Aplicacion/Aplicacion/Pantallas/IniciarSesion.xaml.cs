
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
		private readonly Regex FormatoIP = new(@"\b((25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)(\.|$)){4}\b");

		public IniciarSesion()
		{
			InitializeComponent();
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
			});
		}

		private async void EntrarOfflineDev_Clicked(object sender, EventArgs e)
		{
			Global.UsuarioActual = "dev";
			Global.RolActual = Roles.Desarrollador;

			await Shell.Current.GoToAsync("//Principal");
		}
	}
}
