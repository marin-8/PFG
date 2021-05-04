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
	// ============================================================================================== //

		// Variables y constantes

		public ObservableCollection<Usuario> UsuariosLocal = new();

	// ============================================================================================== //

		// Inicialización

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

	// ============================================================================================== //

		// Eventos UI -> Barra navegación 

		private async void NuevoUsuario_Clicked(object sender, EventArgs e)
		{
			Usuario nuevoUsuario = new("", "", "", Roles.Ninguno);

			string nombre = await PedirAlUsuarioStringCorrecto("Nombre", true);
			if(nombre == null) return;
			nuevoUsuario.Nombre = nombre;	
			
			while(true)
			{
				string nombreUsuario = await PedirAlUsuarioStringCorrecto("Nombre de Usuario", false);
				if(nombreUsuario == null) return;
				nuevoUsuario.NombreUsuario = nombreUsuario;

				RefrescarUsuarios();

				if(UsuariosLocal.Any(u => u.NombreUsuario.Equals(nombreUsuario)))
					await DisplayAlert("Alerta", "Ya existe un usuario con este Nombre de Usuario", "Aceptar");
				else
					break;
			}

			string contrasena = await PedirAlUsuarioStringCorrecto("Contraseña", false);
			if(contrasena == null) return;
			nuevoUsuario.Contrasena = contrasena;	

			List<string> roles = new();
			int i = 0;
			foreach(var rol in Enum.GetValues(typeof(Roles)).Cast<Roles>())
				if((byte)rol < (byte)Roles.Administrador)
					roles.Add($"{++i} - {rol}");

			string rolString = await UserDialogs.Instance.ActionSheetAsync("Rol", "Cancelar", null, null, roles.ToArray());
			if(rolString.Equals("Cancelar")) return;
			nuevoUsuario.Rol = (Roles)(byte.Parse(rolString[0].ToString())-1);

			UserDialogs.Instance.ShowLoading("Creando usuario...");

			await Task.Run(() =>
			{
				string respuestaGestor = new Comando_CrearUsuario(nuevoUsuario).Enviar(Global.IPGestor);
				var comandoRespuesta = Comando.DeJson<Comando_ResultadoCrearUsuario>(respuestaGestor);
				Procesar_ResultadoCrearUsuario(comandoRespuesta); 
			});

			UserDialogs.Instance.HideLoading();
		}

	// ============================================================================================== //

		// Eventos UI -> Contenido

		private void ListaUsuarios_Refresh(object sender, EventArgs e)
		{
			RefrescarUsuarios();
		}

		private static readonly string[] OpcionesUsuario = new string[]
		{
			"Cambiar Nombre",
			"Cambiar Nombre de Usuario",
			"Cambiar Contraseña",
			"Cambiar Rol",
			"Eliminar"
		};

		private async void ListaUsuarios_ItemTapped(object sender, ItemTappedEventArgs e)
		{
			var usuarioPulsado = (Usuario)e.Item;

			string[] opcionesUsuario;

			// Si el pulsado es el mismo que el actual => todo menos eliminar
			if(usuarioPulsado.NombreUsuario == Global.UsuarioActual.NombreUsuario) {
				opcionesUsuario = new string[4];
				Array.Copy(OpcionesUsuario, opcionesUsuario, 4); }

			// Si el pulsado es el Admin => solo cambiar contraseña
			else if(usuarioPulsado.Rol == Roles.Administrador)
				opcionesUsuario = new string[] { OpcionesUsuario[2] };
			
			else
				opcionesUsuario = OpcionesUsuario;

			string opcion = await UserDialogs.Instance.ActionSheetAsync($"{usuarioPulsado.Nombre}", "Cancelar", null, null, opcionesUsuario);
			if(opcion.Equals("Cancelar")) return;

			RefrescarUsuarios();

			if(opcion == OpcionesUsuario[0]) // Cambiar Nombre
			{
				string nuevoNombre;

				while(true)
				{
					nuevoNombre = await PedirAlUsuarioStringCorrecto($"Nuevo Nombre\n(actual = {usuarioPulsado.Nombre})", true);
					if(nuevoNombre == null) return;
					if(nuevoNombre != usuarioPulsado.Nombre) break;

					await DisplayAlert("Alerta", $"El nuevo Nombre no puede ser igual que el anterior", "Aceptar");
				}

				UserDialogs.Instance.ShowLoading("Modificando Nombre...");

				await Task.Run(() =>
				{
					new Comando_ModificarUsuarioNombre(usuarioPulsado.NombreUsuario, nuevoNombre).Enviar(Global.IPGestor);
				});

				if(usuarioPulsado.NombreUsuario == Global.UsuarioActual.NombreUsuario)
					Global.UsuarioActual.Nombre = nuevoNombre;

				UserDialogs.Instance.HideLoading();

				RefrescarUsuarios();

				return;
			}

			if(opcion == OpcionesUsuario[1]) // Cambiar Nombre de Usuario
			{
				string nuevoNombreUsuario;

				while(true)
				{
					nuevoNombreUsuario = await PedirAlUsuarioStringCorrecto($"Nuevo Nombre de Usuario\n(actual = {usuarioPulsado.NombreUsuario})", true);
					if(nuevoNombreUsuario == null) return;
					if(nuevoNombreUsuario != usuarioPulsado.NombreUsuario) break;

					RefrescarUsuarios();

					if(UsuariosLocal.Any(u => u.NombreUsuario.Equals(nuevoNombreUsuario)))
						await DisplayAlert("Alerta", "Ya existe un usuario con este Nombre de Usuario", "Aceptar");

					if(nuevoNombreUsuario.Equals(usuarioPulsado.NombreUsuario))
						await DisplayAlert("Alerta", $"El nuevo Nombre de Usuario no puede ser igual que el anterior", "Aceptar");
				}

				UserDialogs.Instance.ShowLoading("Modificando Nombre de Usuario...");

				await Task.Run(() =>
				{
					new Comando_ModificarUsuarioNombreUsuario(usuarioPulsado.NombreUsuario, nuevoNombreUsuario).Enviar(Global.IPGestor);
				});

				if(usuarioPulsado.NombreUsuario == Global.UsuarioActual.NombreUsuario)
					Global.UsuarioActual.NombreUsuario = nuevoNombreUsuario;

				UserDialogs.Instance.HideLoading();

				RefrescarUsuarios();

				return;
			}

			if(opcion == OpcionesUsuario[2]) // Cambiar Contraseña
			{
				string nuevaContrasena;

				while(true)
				{
					nuevaContrasena = await PedirAlUsuarioStringCorrecto($"Nueva Contraseña\n(actual = {usuarioPulsado.Contrasena})", true);
					if(nuevaContrasena == null) return;
					if(nuevaContrasena != usuarioPulsado.Contrasena) break;

					await DisplayAlert("Alerta", $"La nueva Contraseña no puede ser igual que la anterior", "Aceptar");
				}

				UserDialogs.Instance.ShowLoading("Modificando Contraseña...");

				await Task.Run(() =>
				{
					new Comando_ModificarUsuarioContrasena(usuarioPulsado.NombreUsuario, nuevaContrasena).Enviar(Global.IPGestor);
				});

				if(usuarioPulsado.NombreUsuario == Global.UsuarioActual.NombreUsuario)
					Global.UsuarioActual.Contrasena = nuevaContrasena;

				UserDialogs.Instance.HideLoading();

				RefrescarUsuarios();

				return;
			}
		
			if(opcion == OpcionesUsuario[3]) // Cambiar Rol
			{
				Roles nuevoRol;

				List<string> roles = new();
				int i = 0;
				foreach(var rol in Enum.GetValues(typeof(Roles)).Cast<Roles>())
					if((byte)rol < (byte)Roles.Administrador)
						roles.Add($"{++i} - {rol}");

				while(true)
				{
					string rolString = await UserDialogs.Instance.ActionSheetAsync($"Nuevo Rol (actual = {usuarioPulsado.Rol})", "Cancelar", null, null, roles.ToArray());
					if(rolString.Equals("Cancelar")) return;
					nuevoRol = (Roles)(byte.Parse(rolString[0].ToString())-1);

					if(nuevoRol != usuarioPulsado.Rol) break;

					await DisplayAlert("Alerta", $"El nuevo Rol no puede ser igual que el anterior", "Aceptar");
				}

				UserDialogs.Instance.ShowLoading("Modificando Rol...");

				await Task.Run(() =>
				{
					new Comando_ModificarUsuarioRol(usuarioPulsado.NombreUsuario, nuevoRol).Enviar(Global.IPGestor);
				});

				if(usuarioPulsado.NombreUsuario == Global.UsuarioActual.NombreUsuario)
					Global.UsuarioActual.Rol = nuevoRol;

				UserDialogs.Instance.HideLoading();

				RefrescarUsuarios();

				return;
			}

			if(opcion == OpcionesUsuario[4]) // Eliminar
			{
				if(await UserDialogs.Instance.ConfirmAsync($"¿Eliminar el usuario '{usuarioPulsado.NombreUsuario}'?", "Confirmar eliminación", "Eliminar", "Cancelar"))
				{
					UserDialogs.Instance.ShowLoading("Eliminando usuario...");

					await Task.Run(() =>
					{
						new Comando_EliminarUsuario(usuarioPulsado.NombreUsuario).Enviar(Global.IPGestor);
					});
				}
			}
		}

	// ============================================================================================== //

		// Métodos privados

		private async void RefrescarUsuarios()
		{
			UserDialogs.Instance.ShowLoading("Actualizando lista de usuarios...");

			await Task.Run(() =>
			{
				string respuestaGestor = new Comando_PedirUsuarios().Enviar(Global.IPGestor);
				var comandoRespuesta = Comando.DeJson<Comando_MandarUsuarios>(respuestaGestor);
				Procesar_RecibirUsuarios(comandoRespuesta); 
			});

			UserDialogs.Instance.HideLoading();
		}

		private async Task<string> PedirAlUsuarioStringCorrecto(string Titulo, bool PermitirEspacios)
		{
			string stringCorrecto = null;

			while(stringCorrecto == null)
			{
				var configuracionPrompt = new PromptConfig
				{
					InputType = InputType.Name,
					IsCancellable = true,
					Message = Titulo,
					MaxLength = Comun.Global.MAX_CARACTERES_LOGIN
				};

				var resultado = await UserDialogs.Instance.PromptAsync(configuracionPrompt);

				if(!resultado.Ok) return null;

				stringCorrecto = resultado.Text;

				if(stringCorrecto.Equals("")) {
					await DisplayAlert("Alerta", "No puede estar vacío", "Aceptar"); stringCorrecto = null; continue; }

				string cp = Comun.Global.CARACTERES_PERMITIDOS_LOGIN;

				if(!stringCorrecto.All(PermitirEspacios ? (cp+" ").Contains : cp.Contains)) {
					await DisplayAlert("Alerta", $"Solo se pueden usar letras y números", "Aceptar"); stringCorrecto = null; continue; }

				return stringCorrecto;
			}

			return null;
		}

	// ============================================================================================== //

		// Métodos Procesar

		private void Procesar_RecibirUsuarios(Comando_MandarUsuarios Comando)
		{
			UsuariosLocal.Clear();
			
			foreach(var usuario in Comando.Usuarios)
				UsuariosLocal.Add(usuario);

			ListaUsuarios.EndRefresh();
		}

		private void Procesar_ResultadoCrearUsuario(Comando_ResultadoCrearUsuario Comando)
		{
			if(Comando.ResultadoCrearUsuario == ResultadosCrearUsuario.Correcto)
			{
				RefrescarUsuarios();

				UserDialogs.Instance.Alert("Usuario creado correctamente", "Información", "Aceptar");
			}
			else
			{
				UserDialogs.Instance.Alert("Alguien ha creado un usuario con el mismo Nombre de Usuario antes de que se haya introducido el que has creado, por lo que no se ha añadido el tuyo", "Error", "Aceptar");
			}
		}

	// ============================================================================================== //
	}
}
