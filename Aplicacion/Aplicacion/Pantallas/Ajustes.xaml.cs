
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

		// TODO - CARGAR AJUSTES

		private async void ComenzarJornadaConArticulosDisponibles_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if(e.PropertyName == "On")
			{
				UserDialogs.Instance.ShowLoading("Modificando Nombre de Usuario...");

				bool estaActivado = ((AiForms.Renderers.SwitchCell)sender).On;

				await Task.Run(() =>
				{
					new Comando_ModificarAjusteComenzarJornadaConArticulosDisponibles(estaActivado).Enviar(Global.IPGestor);
				});

				UserDialogs.Instance.HideLoading();
			}
		}
	}
}
