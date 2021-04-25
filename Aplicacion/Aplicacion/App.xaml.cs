
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using PFG.Aplicacion.Pantallas;

namespace PFG.Aplicacion
{
	public partial class App : Application
	{
		public App()
		{
			InitializeComponent();

			MainPage = new IniciarSesion();
		}

		protected override void OnStart()
		{
		}

		protected override void OnSleep()
		{
		}

		protected override void OnResume()
		{
		}
	}
}
