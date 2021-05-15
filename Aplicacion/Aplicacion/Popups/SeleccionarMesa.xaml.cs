
using System;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Acr.UserDialogs;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Extensions;

using PFG.Comun;

namespace PFG.Aplicacion
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SeleccionarMesa : PopupPage
	{
    // ============================================================================================== //

        // Variables y constantes

		private TaskCompletionSource<(bool Correcto, byte NumeroMesaSeleccionada)> _taskCompletionSource;
		public Task<(bool Correcto, byte NumeroMesaSeleccionada)> Resultado => _taskCompletionSource.Task;

		private byte? ResultadoNumeroMesaSeleccionada;

    // ============================================================================================== //

        // Inicialización

		public SeleccionarMesa()
		{
			InitializeComponent();

			InicializarMapaGrid();
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();

			_taskCompletionSource = new TaskCompletionSource<(bool Correcto, byte NumeroMesaSeleccionada)>();
		}

		protected override void OnDisappearing()
        {
            base.OnDisappearing(); 
			
			if(ResultadoNumeroMesaSeleccionada.HasValue)
			{
				_taskCompletionSource.SetResult((true, ResultadoNumeroMesaSeleccionada.Value));
			}
            else
			{
				_taskCompletionSource.SetResult((false, 0));
			}
        }

    // ============================================================================================== //

        // Eventos UI -> Barra navegación

    // ============================================================================================== //

        // Eventos UI -> Contenido

		private async void MesaPulsada(object sender, EventArgs e)
		{
			var botonPulsado = (Button)sender;

			if(botonPulsado.Text != null)
			{
				ResultadoNumeroMesaSeleccionada = byte.Parse(botonPulsado.Text);
				await Navigation.PopPopupAsync();
			}
		}

    // ============================================================================================== //

        // Métodos Helper

		private async void InicializarMapaGrid()
		{
			await Global.Get_Mesas();

			UserDialogs.Instance.ShowLoading("Actualizando mapa mesas...");

			await Device.InvokeOnMainThreadAsync(() =>
			{
				MapaGrid.ColumnDefinitions.Clear();
				MapaGrid.RowDefinitions.Clear();

				for(int c = 0 ; c < Global.AnchoMapaMesas-1; c++) {
					MapaGrid.ColumnDefinitions.Add(new());
					MapaGrid.ColumnDefinitions.Add(new(){Width=new(Mesas.ESPACIO_ENTRE_MESAS)}); }
				MapaGrid.ColumnDefinitions.Add(new());

				for(int f = 0 ; f < Global.AltoMapaMesas-1 ; f++) {
					MapaGrid.RowDefinitions.Add(new());
					MapaGrid.RowDefinitions.Add(new(){Height=new(Mesas.ESPACIO_ENTRE_MESAS)}); }
				MapaGrid.RowDefinitions.Add(new());

				MapaGrid.Children.Clear();

				for(int c = 0 ; c < Global.AnchoMapaMesas; c++)
				{
					for(int f = 0 ; f < Global.AltoMapaMesas ; f++)
					{
						MapaGrid.Children.Add(
							Global.GenerarBotonMesa((byte)(c+1), (byte)(f+1), MesaPulsada),
							c*2,
							f*2);
					}
				}

				foreach(var mesa in Global.Mesas)
				{
					var mesaMapaGrid = (Button)
							MapaGrid.Children
								.Where(c => c.BindingContext.ToString() == $"{mesa.SitioX}.{mesa.SitioY}")
								.First();

					mesaMapaGrid.Text = mesa.Numero.ToString();

					switch(mesa.EstadoMesa)
					{
						case EstadosMesa.Vacia:
						{
							mesaMapaGrid.BackgroundColor = Color.LimeGreen;
							mesaMapaGrid.TextColor = Color.Black;
							break;
						}
						case EstadosMesa.Esperando:
						{
							mesaMapaGrid.BackgroundColor = Color.Red;
							mesaMapaGrid.TextColor = Color.White;
							break;
						}
						case EstadosMesa.Ocupada:
						{
							mesaMapaGrid.BackgroundColor = Color.Black;
							mesaMapaGrid.TextColor = Color.White;
							break;
						}
						case EstadosMesa.Sucia:
						{
							mesaMapaGrid.BackgroundColor = Color.DarkOrange;
							mesaMapaGrid.TextColor = Color.White;
							break;
						}
					}
				}
			});

			UserDialogs.Instance.HideLoading();
		}

    // ============================================================================================== //

        // Métodos Procesar

    // ============================================================================================== //
	}
}