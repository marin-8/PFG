
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
		public static Usuario nuevoUsuario;
		public static ObservableCollection<Usuario> UsuariosLocal = new();

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

		private void Refrescar_Clicked(object sender, EventArgs e)
		{
			RefrescarUsuarios();
		}

		private async void NuevoUsuario_Clicked(object sender, EventArgs e)
		{
			nuevoUsuario = new("", "", "", Roles.Ninguno);

			string nombre = await PedirAlUsuarioStringCorrecto("Nombre");
			if(nombre == null) return;
			nuevoUsuario.Nombre = nombre;	
			
			while(true)
			{
				string nombreUsuario = await PedirAlUsuarioStringCorrecto("Nombre de Usuario");
				if(nombreUsuario == null) return;
				nuevoUsuario.NombreUsuario = nombreUsuario;

				RefrescarUsuarios();

				if(UsuariosLocal.Select(u => u.NombreUsuario).Contains(nombreUsuario))
					await DisplayAlert("Alerta", "Ya existe un usuario con este Nombre de Usuario", "Aceptar");
				else
					break;
			}

			string contrasena = await PedirAlUsuarioStringCorrecto("Contraseña");
			if(contrasena == null) return;
			nuevoUsuario.Contrasena = contrasena;	

			List<string> roles = new();
			int i = 0;
			foreach(var rol in Enum.GetValues(typeof(Roles)).Cast<Roles>())
				roles.Add($"{++i} - {rol}");

			string rolString = await UserDialogs.Instance.ActionSheetAsync("Rol", "Cancelar", null, null, roles.ToArray());
			if(rolString.Equals("Cancelar")) return;
			nuevoUsuario.Rol = (Roles)(byte.Parse(rolString[0].ToString())-1);

			UserDialogs.Instance.ShowLoading("Creando usuario...");

			await Task.Run(() =>
			{
				new Comando_IntentarCrearUsuario(nuevoUsuario).Enviar(Global.IPGestor);
			});
		}

		private async void RefrescarUsuarios()
		{
			UserDialogs.Instance.ShowLoading("Actualizando lista de usuarios...");

			await Task.Run(() =>
			{
				new Comando_PedirUsuarios().Enviar(Global.IPGestor);
			});
		}

		private async Task<string> PedirAlUsuarioStringCorrecto(string Titulo)
		{
			string stringCorrecto = null;

			while(stringCorrecto == null)
			{
				var configuracionPrompt = new PromptConfig();
				configuracionPrompt.InputType = InputType.Name;
				configuracionPrompt.IsCancellable = true;
				configuracionPrompt.Message = Titulo;
				configuracionPrompt.MaxLength = Comun.Global.MAX_CARACTERES_LOGIN;

				var resultado = await UserDialogs.Instance.PromptAsync(configuracionPrompt);

				if(!resultado.Ok) return null;

				stringCorrecto = resultado.Text;

				if(stringCorrecto.Equals("")) {
					await DisplayAlert("Alerta", "No puede estar vacío", "Aceptar"); stringCorrecto = null; continue; }

				if(!stringCorrecto.All(Comun.Global.CARACTERES_PERMITIDOS_LOGIN.Contains)) {
					await DisplayAlert("Alerta", $"Solo se pueden usar letras y números", "Aceptar"); stringCorrecto = null; continue; }

				return stringCorrecto;
			}

			return null;
		}
	}
}
