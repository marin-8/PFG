
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PFG.Aplicacion
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AppShell : Shell
	{
		public AppShell()
		{
			InitializeComponent();
		}

		private void CerrarSesion(object sender, EventArgs e)
		{
			// TODO - Cerrar Sesión

			DisplayAlert("hey", "wow", "Aceptar");
		}
	}
}
