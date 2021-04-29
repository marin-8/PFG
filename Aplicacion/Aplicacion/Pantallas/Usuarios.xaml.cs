﻿
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
		public readonly ObservableCollection<Usuario> UsuariosLocal = new() { new("N", "C", Roles.Ninguno) };

		public Usuarios()
		{
			InitializeComponent();
		}

		private void NuevoUsuario_Clicked(object sender, EventArgs e)
		{
			UsuariosLocal.Add(new("N", "C", Roles.Ninguno));
		}

		private void Refrescar_Clicked(object sender, EventArgs e)
		{

		}
	}
}
