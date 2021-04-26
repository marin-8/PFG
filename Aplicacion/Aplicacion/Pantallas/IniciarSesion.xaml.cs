
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Text.RegularExpressions;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using PFG.Comun;

namespace PFG.Aplicacion.Pantallas
{
	public partial class IniciarSesion : ContentPage
	{
		private readonly Regex FormatoIP = new Regex(@"\b((25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)(\.|$)){4}\b");

		public IniciarSesion()
		{
			InitializeComponent();
		}

		private void Entrar_Clicked(object sender, EventArgs e)
		{
			if(!FormatoIP.IsMatch(IPGestor.Text)) { DisplayAlert("Alerta", "La IP introducida no es válida",              "Aceptar"); return; }
			if(          Usuario.Text.Equals("")) { DisplayAlert("Alerta", "Usuario vacío. Este campo es obligatorio",    "Aceptar"); return; }
			if(       Contrasena.Text.Equals("")) { DisplayAlert("Alerta", "Contraseña vacía. Este campo es obligatorio", "Aceptar"); return; }

			//
		}
	}
}
