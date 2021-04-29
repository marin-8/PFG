
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
	public partial class Usuarios : ContentPage
	{
		public readonly ObservableCollection<Usuario> UsuariosLocal = new() { new("Administrador", "admin", "admin", Roles.Administrador) };

		public Usuarios()
		{
			InitializeComponent();

			Shell.Current.Navigated += OnNavigatedTo;

			ListaUsuarios.ItemsSource = UsuariosLocal;
		}

		private void OnNavigatedTo(object sender, ShellNavigatedEventArgs e)
		{
			if(e.Current.Location.OriginalString.Contains(((BaseShellItem)Parent).Route.ToString()))
			{
				RefrescarUsuarios();
			}
		}

		private void NuevoUsuario_Clicked(object sender, EventArgs e)
		{
			// TODO - NuevoUsuario
		}

		private void RefrescarUsuarios()
		{
			// TODO - RefrescarUsuarios
		}

		private void Refrescar_Clicked(object sender, EventArgs e)
		{
			RefrescarUsuarios();
		}
	}
}
