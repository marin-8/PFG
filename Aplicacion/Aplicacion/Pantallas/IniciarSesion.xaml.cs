
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

		private void Entrar_Clicked(object sender, EventArgs e)
		{
			string ipGestor = IPGestor.Text;
			string usuario = Usuario.Text;
			string contrasena = Contrasena.Text;

			if(         ipGestor.Equals("")) { DisplayAlert("Alerta", "IP del Gestor vacía. Este campo es obligatorio",    "Aceptar"); return; }
			if(          usuario.Equals("")) { DisplayAlert("Alerta", "Usuario vacío. Este campo es obligatorio",          "Aceptar"); return; }
			if(       contrasena.Equals("")) { DisplayAlert("Alerta", "Contraseña vacía. Este campo es obligatorio",       "Aceptar"); return; }

			if(								  !FormatoIP.IsMatch(ipGestor)) { DisplayAlert("Alerta", "La IP introducida no es válida",                                "Aceptar"); return; }
			if(         usuario.Length > Comun.Global.MAX_CARACTERES_LOGIN) { DisplayAlert("Alerta", "El Usuario no puede estar formado por más de 20 caracteres",    "Aceptar"); return; }
			if(      contrasena.Length > Comun.Global.MAX_CARACTERES_LOGIN) { DisplayAlert("Alerta", "La Contraseña no puede estar formada por más de 20 caracteres", "Aceptar"); return; }

			UserDialogs.Instance.ShowLoading("Inciando sesión...");

			Task.Run(() =>
			{
				Global.UsuarioActual = usuario;
				Global.ContrasenaActual = contrasena;

				GestionSesionApp.IniciarSesion
				(
					ipGestor,
					usuario,
					contrasena
				);

				UserDialogs.Instance.HideLoading();
			});
		}
	}
}
