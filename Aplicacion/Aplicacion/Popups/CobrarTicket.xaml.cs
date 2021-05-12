
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Acr.UserDialogs;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Extensions;

using PFG.Comun;

namespace PFG.Aplicacion
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CobrarTicket : PopupPage
	{
		// ============================================================================================== //

		// Variables y constantes

		private ItemTicket[] ItemsTicket;

	// ============================================================================================== //

		// Inicialización

		public CobrarTicket(byte NumeroMesaSeleccionada)
		{
			InitializeComponent();

			InicializarInterfaz(NumeroMesaSeleccionada);
		}

	// ============================================================================================== //

		// Eventos UI -> Contenido

		private async void Cancelar_Clicked(object sender, EventArgs e)
		{
			await Navigation.PopPopupAsync();
		}

		private async void Aceptar_Clicked(object sender, EventArgs e)
		{
			// TODO - Cobrar

			//if (!byte.TryParse(SeleccionarMesa.Text, out byte mesaSeleccionada))
			//{
			//	await UserDialogs.Instance.AlertAsync("No se ha seleccionado una mesa", "Alerta", "Aceptar");

			//	return;
			//}
			//else if (ItemsTicket.Count() == 0)
			//{
			//	await UserDialogs.Instance.AlertAsync("No se ha seleccionado ningún artículo", "Alerta", "Aceptar");

			//	return;
			//}

			//UserDialogs.Instance.ShowLoading("Mandando pedido...");

			//await Task.Run(() =>
			//{
			//	new Comando_CobrarTicket(mesaSeleccionada, ItemsTicket.ToArray()).Enviar(Global.IPGestor);
			//});

			//UserDialogs.Instance.HideLoading();

			await Navigation.PopPopupAsync();
		}

	// ============================================================================================== //

		// Métodos privados

		private async void InicializarInterfaz(byte NumeroMesa)
		{
			Titulo.Text = $"Cobrar mesa {NumeroMesa}";

			await PedirTicket(NumeroMesa);

			UserDialogs.Instance.ShowLoading("Construyendo ticket...");

			await Device.InvokeOnMainThreadAsync(() =>
			{
				foreach(var itemTicket in ItemsTicket)
				{
					var nuevaFila = GridItemsTicket.RowDefinitions.Count() -1;

					GridItemsTicket.Children.Add(
						new Label() {
							Text=itemTicket.Unidades.ToString(),
							FontSize=18,
							TextColor=Color.Black,
							HorizontalTextAlignment=TextAlignment.End},
						0, nuevaFila);

					GridItemsTicket.Children.Add(
						new Label() {
							Text=itemTicket.NombreArticulo,
							FontSize=18,
							TextColor=Color.Black},
						1, nuevaFila);

					GridItemsTicket.Children.Add(
						new Label() {
							Text=string.Format("{0:n}", itemTicket.PrecioUnitario),
							FontSize=18,
							TextColor=Color.Black,
							HorizontalTextAlignment=TextAlignment.End},
						2, nuevaFila);

					GridItemsTicket.Children.Add(
						new Label() {
							Text=string.Format("{0:n}", itemTicket.PrecioTotal),
							FontSize=18,
							TextColor=Color.Black,
							HorizontalTextAlignment=TextAlignment.End},
						3, nuevaFila);

					GridItemsTicket.RowDefinitions.Add(new(){Height=new(1,GridUnitType.Auto)});
				}

				GridItemsTicket.RowDefinitions.RemoveAt(
					GridItemsTicket.RowDefinitions.Count-1);

				var total = ItemsTicket.Sum(i => i.PrecioTotal);

				Total.Text = $"TOTAL:    {total} €";
			});

			UserDialogs.Instance.HideLoading();
		}

		private async Task PedirTicket(byte NumeroMesa)
		{
			UserDialogs.Instance.ShowLoading("Pidiendo ticket...");

			var comandoRespuesta = await Task.Run(() =>
			{
				string respuestaGestor = new Comando_PedirTicketMesa(NumeroMesa).Enviar(Global.IPGestor);
				return Comando.DeJson<Comando_MandarTicketMesa>(respuestaGestor);
			});

			ItemsTicket = comandoRespuesta.ItemsTicket;

			UserDialogs.Instance.HideLoading();
		}

	// ============================================================================================== //

		// Métodos Procesar

	// ============================================================================================== //
	}
}