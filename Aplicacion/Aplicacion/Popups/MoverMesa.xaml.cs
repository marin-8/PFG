
using System;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Acr.UserDialogs;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Extensions;

using PFG.Comun;

namespace PFG.Aplicacion
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MoverMesa : PopupPage
	{
    // ============================================================================================== //

        // Variables y constantes

		private readonly byte NumeroMesaSeleccionada;
		private readonly Action<byte,byte,byte> EventoNuevoSitioSeleccionado;

    // ============================================================================================== //

        // Inicialización

		public MoverMesa(byte NumeroMesaSeleccionada, Action<byte,byte,byte> EventoNuevoSitioSeleccionado)
		{
			InitializeComponent();

			this.NumeroMesaSeleccionada = NumeroMesaSeleccionada;
			this.EventoNuevoSitioSeleccionado = EventoNuevoSitioSeleccionado;

			InicializarMapaGrid(NumeroMesaSeleccionada);
		}

    // ============================================================================================== //

        // Eventos UI -> Barra navegación

    // ============================================================================================== //

        // Eventos UI -> Contenido

		private async void MesaPulsada(object sender, EventArgs e)
		{
			var botonPulsado = (Button)sender;

			if(botonPulsado.BackgroundColor == Color.Silver &&
			   botonPulsado.TextColor == Color.White)
			{
				await UserDialogs.Instance.AlertAsync("Selecciona un sitio destino distinto al de origen", "Alerta", "Aceptar");
			}
			else
			{
				var sitioPulsadoString = (string)botonPulsado.BindingContext;
				int indiceDelPunto = sitioPulsadoString.IndexOf('.');
				byte sitioPulsadoX = byte.Parse(sitioPulsadoString.Substring(0, indiceDelPunto));
				byte sitioPulsadoY = byte.Parse(sitioPulsadoString.Substring(indiceDelPunto+1, sitioPulsadoString.Length-indiceDelPunto-1));

				await Navigation.PopPopupAsync();

				EventoNuevoSitioSeleccionado.Invoke(NumeroMesaSeleccionada, sitioPulsadoX, sitioPulsadoY);
			}
		}

    // ============================================================================================== //

        // Métodos helper

		private async void InicializarMapaGrid(byte NumeroMesaSeleccionada)
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

					if(mesa.Numero == NumeroMesaSeleccionada)
					{
						mesaMapaGrid.BackgroundColor = Color.Silver;
						mesaMapaGrid.TextColor = Color.White;
					}
					else
					{
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
				}
			});

			UserDialogs.Instance.HideLoading();
		}

	// ============================================================================================== //

        // Métodos Procesar

	// ============================================================================================== 
	}
}