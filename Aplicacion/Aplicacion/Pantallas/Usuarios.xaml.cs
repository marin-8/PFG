
using System;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;

using Acr.UserDialogs;

using PFG.Comun;

namespace PFG.Aplicacion
{
	public partial class Usuarios : ContentPage
	{
	// ============================================================================================== //

		// Variables y constantes

	// ============================================================================================== //

		// Inicialización

		public Usuarios()
		{
			InitializeComponent();

			Shell.Current.Navigated += OnNavigatedTo;

			lock(Global.UsuariosLock)
				ListaUsuarios.ItemsSource = Global.Usuarios;
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

			string nombre = await Global.PedirAlUsuarioStringCorrecto("Nombre", Comun.Global.MAX_CARACTERES_LOGIN, true);
			if(nombre == null) return;
			nuevoUsuario.Nombre = nombre;	
			
			while(true)
			{
				string nombreUsuario = await Global.PedirAlUsuarioStringCorrecto("Nombre de Usuario", Comun.Global.MAX_CARACTERES_LOGIN, false);
				if(nombreUsuario == null) return;
				nuevoUsuario.NombreUsuario = nombreUsuario;

				bool algunUsuarioConMismoNombre;

				lock(Global.UsuariosLock)
					algunUsuarioConMismoNombre =
						Global.Usuarios
							.Any(u => u.NombreUsuario == nombreUsuario);

				if(algunUsuarioConMismoNombre)
					await DisplayAlert("Alerta", "Ya existe un usuario con este Nombre de Usuario", "Aceptar");
				else
					break;
			}

			string contrasena = await Global.PedirAlUsuarioStringCorrecto("Contraseña", Comun.Global.MAX_CARACTERES_LOGIN, false);
			if(contrasena == null) return;
			nuevoUsuario.Contrasena = contrasena;	

			string rolString = await UserDialogs.Instance.ActionSheetAsync("Rol", "Cancelar", null, null, RolesBaseToStringArray());
			if(rolString == "Cancelar") return;
			nuevoUsuario.Rol = (Roles)Enum.Parse(typeof(Roles), rolString);

			UserDialogs.Instance.ShowLoading("Creando usuario...");

			var comandoRespuesta = await Task.Run(() =>
			{
				string respuestaGestor = new Comando_CrearUsuario(nuevoUsuario).Enviar(Global.IPGestor);
				return Comando.DeJson<Comando_ResultadoGenerico>(respuestaGestor);
			});
			Global.Procesar_ResultadoGenerico(comandoRespuesta, RefrescarUsuarios);

			UserDialogs.Instance.HideLoading();
		}

		private void ListaUsuarios_Refresh(object sender, EventArgs e)
		{
			RefrescarUsuarios();
		}

	// ============================================================================================== //

		// Eventos UI -> Contenido

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

			// Si el pulsado es el Admin => solo cambiar contraseña
			if(usuarioPulsado.Rol == Roles.Administrador)
				opcionesUsuario = new string[] { OpcionesUsuario[2] };
			else
				opcionesUsuario = OpcionesUsuario;

			string opcion = await UserDialogs.Instance.ActionSheetAsync($"{usuarioPulsado.Nombre}", "Cancelar", null, null, opcionesUsuario);
			if(opcion == "Cancelar") return;

			if(opcion == OpcionesUsuario[0]) // Cambiar Nombre
			{
				string nuevoNombre;

				while(true)
				{
					nuevoNombre = await Global.PedirAlUsuarioStringCorrecto($"Nuevo Nombre\n(actual = {usuarioPulsado.Nombre})", Comun.Global.MAX_CARACTERES_LOGIN, true);
					if(nuevoNombre == null) return;
					if(nuevoNombre != usuarioPulsado.Nombre) break;

					await DisplayAlert("Alerta", $"El nuevo Nombre no puede ser igual que el anterior", "Aceptar");
				}

				UserDialogs.Instance.ShowLoading("Modificando Nombre...");

				if(usuarioPulsado.NombreUsuario == Global.UsuarioActual.NombreUsuario)
					Global.UsuarioActual.Nombre = nuevoNombre;

				var comandoRespuesta = await Task.Run(() =>
				{
					string respuestaGestor = new Comando_ModificarUsuarioNombre(usuarioPulsado.NombreUsuario, nuevoNombre).Enviar(Global.IPGestor);
					return Comando.DeJson<Comando_ResultadoGenerico>(respuestaGestor);
				});
				Global.Procesar_ResultadoGenerico(comandoRespuesta, RefrescarUsuarios);

				UserDialogs.Instance.HideLoading();

				return;
			}

			if(opcion == OpcionesUsuario[1]) // Cambiar Nombre de Usuario
			{
				string nuevoNombreUsuario;

				while(true)
				{
					nuevoNombreUsuario = await Global.PedirAlUsuarioStringCorrecto($"Nuevo Nombre de Usuario\n(actual = {usuarioPulsado.NombreUsuario})", Comun.Global.MAX_CARACTERES_LOGIN, true);
					if(nuevoNombreUsuario == null) return;

					bool algunUsuarioConMismoNombre;

					lock(Global.UsuariosLock)
						algunUsuarioConMismoNombre =
							Global.Usuarios
								.Any(u => u.NombreUsuario == nuevoNombreUsuario);

					if(algunUsuarioConMismoNombre)
						await DisplayAlert("Alerta", "Ya existe un usuario con este Nombre de Usuario", "Aceptar");

					else if(nuevoNombreUsuario == usuarioPulsado.NombreUsuario)
						await DisplayAlert("Alerta", $"El nuevo Nombre de Usuario no puede ser igual que el anterior", "Aceptar");
				
					else
						break;
				}

				UserDialogs.Instance.ShowLoading("Modificando Nombre de Usuario...");

				if(usuarioPulsado.NombreUsuario == Global.UsuarioActual.NombreUsuario)
					Global.UsuarioActual.NombreUsuario = nuevoNombreUsuario;

				var comandoRespuesta = await Task.Run(() =>
				{
					string respuestaGestor = new Comando_ModificarUsuarioNombreUsuario(usuarioPulsado.NombreUsuario, nuevoNombreUsuario).Enviar(Global.IPGestor);
					return Comando.DeJson<Comando_ResultadoGenerico>(respuestaGestor);
				});
				Global.Procesar_ResultadoGenerico(comandoRespuesta, RefrescarUsuarios);

				UserDialogs.Instance.HideLoading();

				return;
			}

			if(opcion == OpcionesUsuario[2]) // Cambiar Contraseña
			{
				string nuevaContrasena;

				while(true)
				{
					nuevaContrasena = await Global.PedirAlUsuarioStringCorrecto($"Nueva Contraseña\n(actual = {usuarioPulsado.Contrasena})", Comun.Global.MAX_CARACTERES_LOGIN, true);
					if(nuevaContrasena == null) return;
					if(nuevaContrasena != usuarioPulsado.Contrasena) break;

					await DisplayAlert("Alerta", $"La nueva Contraseña no puede ser igual que la anterior", "Aceptar");
				}

				UserDialogs.Instance.ShowLoading("Modificando Contraseña...");

				if(usuarioPulsado.NombreUsuario == Global.UsuarioActual.NombreUsuario)
					Global.UsuarioActual.Contrasena = nuevaContrasena;

				var comandoRespuesta = await Task.Run(() =>
				{
					string respuestaGestor = new Comando_ModificarUsuarioContrasena(usuarioPulsado.NombreUsuario, nuevaContrasena).Enviar(Global.IPGestor);
					return Comando.DeJson<Comando_ResultadoGenerico>(respuestaGestor);
				});
				Global.Procesar_ResultadoGenerico(comandoRespuesta, RefrescarUsuarios);

				UserDialogs.Instance.HideLoading();

				return;
			}
		
			if(opcion == OpcionesUsuario[3]) // Cambiar Rol
			{
				Roles nuevoRol;

				while(true)
				{
					string rolString = await UserDialogs.Instance.ActionSheetAsync($"Nuevo Rol (actual = {usuarioPulsado.Rol})", "Cancelar", null, null, RolesBaseToStringArray());
					if(rolString == "Cancelar") return;
					nuevoRol = (Roles)Enum.Parse(typeof(Roles), rolString);

					if(nuevoRol != usuarioPulsado.Rol) break;

					await DisplayAlert("Alerta", $"El nuevo Rol no puede ser igual que el anterior", "Aceptar");
				}

				UserDialogs.Instance.ShowLoading("Modificando Rol...");

				if(usuarioPulsado.NombreUsuario == Global.UsuarioActual.NombreUsuario)
					Global.UsuarioActual.Rol = nuevoRol;

				var comandoRespuesta = await Task.Run(() =>
				{
					string respuestaGestor = new Comando_ModificarUsuarioRol(usuarioPulsado.NombreUsuario, nuevoRol).Enviar(Global.IPGestor);
					return Comando.DeJson<Comando_ResultadoGenerico>(respuestaGestor);
				});
				Global.Procesar_ResultadoGenerico(comandoRespuesta, RefrescarUsuarios);

				UserDialogs.Instance.HideLoading();

				return;
			}

			if(opcion == OpcionesUsuario[4]) // Eliminar
			{
				if(await UserDialogs.Instance.ConfirmAsync($"¿Eliminar el usuario '{usuarioPulsado.NombreUsuario}'?", "Confirmar eliminación", "Eliminar", "Cancelar"))
				{
					UserDialogs.Instance.ShowLoading("Eliminando usuario...");

					var comandoRespuesta = await Task.Run(() =>
					{
						string respuestaGestor = new Comando_EliminarUsuario(usuarioPulsado.NombreUsuario).Enviar(Global.IPGestor);
						return Comando.DeJson<Comando_ResultadoGenerico>(respuestaGestor);
					});
					Global.Procesar_ResultadoGenerico(comandoRespuesta, RefrescarUsuarios);

					UserDialogs.Instance.HideLoading();
				}
			}
		}

	// ============================================================================================== //

		// Métodos Helper

		private async void RefrescarUsuarios()
		{
			UserDialogs.Instance.ShowLoading("Actualizando lista de usuarios...");

			var comandoRespuesta = await Task.Run(() =>
			{
				string respuestaGestor = new Comando_PedirUsuarios().Enviar(Global.IPGestor);
				return Comando.DeJson<Comando_MandarUsuarios>(respuestaGestor);
			});
			Procesar_RecibirUsuarios(comandoRespuesta); 

			UserDialogs.Instance.HideLoading();
		}

		private static string[] RolesBaseToStringArray()
		{
			var roles =
				Enum.GetValues(typeof(Roles))
					.Cast<Roles>()
						.Where(r => (byte)r > (byte)Roles.Administrador)
						.Select(r => r.ToString());

			return roles.ToArray();
		}

	// ============================================================================================== //

		// Métodos Procesar

		private void Procesar_RecibirUsuarios(Comando_MandarUsuarios Comando)
		{
			lock(Global.UsuariosLock)
			{
				Global.Usuarios.Clear();
			
				foreach(var usuario in Comando.Usuarios)
					Global.Usuarios.Add(usuario);
			}

			ListaUsuarios.EndRefresh();
		}

	// ============================================================================================== //
	}
}
