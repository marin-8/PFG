
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PFG.Aplicacion
{
	public partial class App : Application
	{
		public App()
		{
			InitializeComponent();

			MainPage = new AppShell();
		}

		protected override void OnStart()
		{
			try
			{
				Global.ProcesadorMensajesRecibidos = new();
				Global.Servidor = new(
					Comun.Global.Get_MiIP_Xamarin(),
					Global.ProcesadorMensajesRecibidos.Procesar,
					true);
			}
			catch { }
		}

		protected override void OnSleep()
		{
		}

		protected override void OnResume()
		{
		}
	}
}
