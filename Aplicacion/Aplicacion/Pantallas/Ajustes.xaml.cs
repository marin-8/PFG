
using System.Threading.Tasks;
using System.ComponentModel;

using Xamarin.Forms;

using Acr.UserDialogs;

using PFG.Comun;

namespace PFG.Aplicacion
{
	public partial class Ajustes : ContentPage
	{
		public Ajustes()
		{
			InitializeComponent();
		}

		private async void ComenzarJornadaConArticulosDisponibles_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			UserDialogs.Instance.ShowLoading("Modificando Nombre de Usuario...");

			bool estaActivado = ((SwitchCell)sender).On;

			await Task.Run(() =>
			{
				new Comando_ModificarAjusteComenzarJornadaConArticulosDisponibles(estaActivado).Enviar(Global.IPGestor);
			});

			UserDialogs.Instance.HideLoading();
		}
	}
}
