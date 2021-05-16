
using System;
using System.Linq;

using Xamarin.Forms;

using Acr.UserDialogs;

using PFG.Comun;

namespace PFG.Aplicacion
{
	public partial class ConsultarMesas : ContentPage
	{
	// ============================================================================================== //

		// Variables y constantes

		public const int ESPACIO_ENTRE_MESAS = 10;

	// ============================================================================================== //

		// Inicialización

		public ConsultarMesas()
		{
			InitializeComponent();
		}

		protected override void OnAppearing()
		{
			ActualizarMapaMesas();
		}

	// ============================================================================================== //

		// Eventos UI -> Barra navegación 

		private void Refrescar_Clicked(object sender, EventArgs e)
		{
			ActualizarMapaMesas();
		}

	// ============================================================================================== //

		// Eventos UI -> Contenido

	// ============================================================================================== //

		// Métodos Helper

		private async void ActualizarMapaMesas()
		{
			await Global.Get_Mesas();

			UserDialogs.Instance.ShowLoading("Actualizando mapa mesas...");

			await Device.InvokeOnMainThreadAsync(() =>
			{
				MapaGrid.ColumnDefinitions.Clear();
				MapaGrid.RowDefinitions.Clear();

				for(int c = 0 ; c < Global.AnchoMapaMesas-1; c++) {
					MapaGrid.ColumnDefinitions.Add(new());
					MapaGrid.ColumnDefinitions.Add(new(){Width=new(ESPACIO_ENTRE_MESAS)}); }
				MapaGrid.ColumnDefinitions.Add(new());

				for(int f = 0 ; f < Global.AltoMapaMesas-1 ; f++) {
					MapaGrid.RowDefinitions.Add(new());
					MapaGrid.RowDefinitions.Add(new(){Height=new(ESPACIO_ENTRE_MESAS)}); }
				MapaGrid.RowDefinitions.Add(new());

				MapaGrid.Children.Clear();

				for(int c = 0 ; c < Global.AnchoMapaMesas; c++)
				{
					for(int f = 0 ; f < Global.AltoMapaMesas ; f++)
					{
						MapaGrid.Children.Add(
							Global.GenerarFrameLabelMesa((byte)(c+1), (byte)(f+1)),
							c*2,
							f*2);
					}
				}

				foreach(var mesa in Global.Mesas)
				{
					var mesaMapaGrid = (Label)
							MapaGrid.Children
								.Where(c => ((Frame)c).Content.BindingContext.ToString() == $"{mesa.SitioX}.{mesa.SitioY}")
								.Select(c => ((Frame)c).Content)
								.Single();

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
