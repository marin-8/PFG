
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using PFG.Comun;

namespace PFG.Aplicacion
{
	public partial class App : Application
	{
		private ControladorRed Servidor;
		private Procesador ProcesadorMensajesRecibidos;

		public App()
		{
			InitializeComponent();

			ProcesadorMensajesRecibidos = new();
			string servidorIP = Comun.Global.Get_MiIP_Windows();
			Servidor = new(servidorIP, ProcesadorMensajesRecibidos.Procesar, true);

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
