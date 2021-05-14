
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Acr.UserDialogs;

using PFG.Comun;

namespace PFG.Aplicacion
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AppShell : Shell
	{
		private static AppShell Instancia;

		public AppShell()
		{
			InitializeComponent();

			Instancia = this;
		}

		public static void ConfigurarPantallasAMostrar(Roles Rol)
		{
			bool mostrarOno = Rol == Roles.Administrador;

			Instancia.PantallaPrincipal.FlyoutItemIsVisible = !mostrarOno;
			Instancia.PantallaCarta.FlyoutItemIsVisible = mostrarOno;
			Instancia.PantallaMesas.FlyoutItemIsVisible = mostrarOno;
			Instancia.PantallaUsuarios.FlyoutItemIsVisible = mostrarOno;
			Instancia.PantallaAjustes.FlyoutItemIsVisible = mostrarOno;

			Instancia.PantallaDesarrollo.FlyoutItemIsVisible = Rol == Roles.Desarrollador;
		}

		private async void CerrarSesion(object sender, EventArgs e)
		{
			UserDialogs.Instance.ShowLoading("Cerrando sesión...");

			Current.FlyoutIsPresented = false;

			await Task.Run(() =>
			{
				new Comando_CerrarSesion
				(
					Global.UsuarioActual.NombreUsuario
				)
				.Enviar(Global.IPGestor);
			});

			Global.UsuarioActual = null;

			await Current.GoToAsync("//IniciarSesion");

			await Task.Delay(200);

			UserDialogs.Instance.HideLoading();
		}
	}
}
