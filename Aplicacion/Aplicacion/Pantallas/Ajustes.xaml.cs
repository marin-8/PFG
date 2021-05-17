
using System.Threading.Tasks;
using System.ComponentModel;

using Xamarin.Forms;

using Acr.UserDialogs;

using PFG.Comun;

namespace PFG.Aplicacion
{
	public partial class Ajustes : ContentPage
	{
		private bool CargandoAjustes = false;

		public Ajustes()
		{
			InitializeComponent();

			Shell.Current.Navigated += OnNavigatedTo;
		}

		private async void OnNavigatedTo(object sender, ShellNavigatedEventArgs e)
		{
			if(e.Current.Location.OriginalString.Contains(((BaseShellItem)Parent).Route.ToString()))
			{
				await Global.Get_Ajustes();

				CargandoAjustes = true;

				ComenzarJornadaConArticulosDisponibles.On = Global.Ajustes.ComenzarJornadaConArticulosDisponibles;

				CargandoAjustes = false;
			}
		}

		private async void ComenzarJornadaConArticulosDisponibles_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if(!CargandoAjustes)
			{
				if(e.PropertyName == AiForms.Renderers.SwitchCell.OnProperty.PropertyName)
				{
					UserDialogs.Instance.ShowLoading("Modificando ajuste...");

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
}
