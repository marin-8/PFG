
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
		public AppShell()
		{
			InitializeComponent();
		}

		private async void CerrarSesion(object sender, EventArgs e)
		{
			UserDialogs.Instance.ShowLoading("Cerrando sesión...");

			Current.FlyoutIsPresented = false;

			if(Global.RolActual != Roles.Desarrollador)
			{
				await Task.Run(async () =>
				{
					new Comando_CerrarSesion
					(
						Global.UsuarioActual,
						false
					)
					.Enviar(Global.IPGestor);

					Global.ContrasenaActual = "";

					await Device.InvokeOnMainThreadAsync(async () =>
						await Current.GoToAsync("//IniciarSesion") );
				});
			}

			Global.UsuarioActual = "";
			Global.RolActual = Roles.Ninguno;

			await Current.GoToAsync("//IniciarSesion");

			UserDialogs.Instance.HideLoading();
		}
	}
}
