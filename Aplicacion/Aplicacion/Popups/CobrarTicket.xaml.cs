
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

		private ItemTicket[] ItemsTicket = new ItemTicket[]
		{
			new(69, "OOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOO", 4.5f),
			new(69, "asdsadsadsadsadsadas", 2.2f),
			new(69, "sadsadsadsadasdsadasdsadsadsadasdas", 2.2f),
			new(69, "asdsadsadasdasdas", 2.2f),
			new(69, "asdsadsadsa", 2.2f),
			new(69, "asdasdasdasdad", 2.2f),
			new(69, "asdsadsadsadsadasdsad", 2.2f),
			new(69, "asdasdasd", 2.2f),
			new(69, "asdsadasdsadsadsadsadasdasd", 2.2f),
			new(69, "asdsadsadsadsadsadsadsadsadasdasdasd", 2.2f),
			new(69, "asdasdasdasdasdasd", 2.2f),
			new(69, "adasdasdsasd", 2.2f),
			new(69, "asdasdasdasas", 2.2f),
			new(69, "asdasdasdas", 2.2f),
			new(69, "asdas", 2.2f),
			new(69, "asdasd", 2.2f),
			new(69, "asdasdsadsadsaa", 2.2f),
			new(69, "asdsadasd", 2.2f),
			new(69, "asdasdasdsadasdasdasdasdas", 2.2f),
			new(69, "asdasasdasdaasdas", 2.2f),
			new(69, "asdasdasdasdas", 2.2f),
			new(69, "asdasdasdasdasdasdasda", 2.2f),
		};

	// ============================================================================================== //

		// Inicialización

		public CobrarTicket(byte NumeroMesaSeleccionada)
		{
			InitializeComponent();

			Titulo.Text = $"Cobrar mesa {NumeroMesaSeleccionada}";

			PedirTicket(NumeroMesaSeleccionada);

			foreach(var itemTicket in ItemsTicket)
			{
				var nuevaFila = GridItemsTicket.RowDefinitions.Count() -1;

				GridItemsTicket.Children.Add(
					new Label() {
						Text=itemTicket.Unidades.ToString(),
						FontSize=18,
						TextColor=Color.Black},
					0, nuevaFila);

				GridItemsTicket.Children.Add(
					new Label() {
						Text=itemTicket.NombreArticulo,
						FontSize=18,
						TextColor=Color.Black},
					1, nuevaFila);

				GridItemsTicket.Children.Add(
					new Label() {
						Text=itemTicket.PrecioUnitario.ToString("0.00"),
						FontSize=18,
						TextColor=Color.Black},
					2, nuevaFila);

				GridItemsTicket.Children.Add(
					new Label() {
						Text=itemTicket.PrecioTotal.ToString("0.00"),
						FontSize=18,
						TextColor=Color.Black},
					3, nuevaFila);

				GridItemsTicket.RowDefinitions.Add(new(){Height=new(1,GridUnitType.Auto)});
			}

			GridItemsTicket.RowDefinitions.RemoveAt(
				GridItemsTicket.RowDefinitions.Count-1);

			var total = ItemsTicket.Sum(i => i.PrecioTotal);

			Total.Text = $"TOTAL:    {total} €";
		}

	// ============================================================================================== //

		// Eventos UI -> Contenido

		private async void Cancelar_Clicked(object sender, EventArgs e)
		{
			await Navigation.PopPopupAsync();
		}

		private async void Aceptar_Clicked(object sender, EventArgs e)
		{
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

		private async void PedirTicket(byte NumeroMesa)
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