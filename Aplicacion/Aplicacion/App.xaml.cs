
using Xamarin.Forms;

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
			Global.Servidor = new(
				Comun.Global.Get_MiIP_Xamarin(),
				ProcesadorAplicacion.ProcesarComandosRecibidos);
		}

		protected override void OnSleep()
		{
		}

		protected override void OnResume()
		{
		}
	}
}
