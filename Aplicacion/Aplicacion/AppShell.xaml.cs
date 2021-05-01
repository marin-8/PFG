
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
