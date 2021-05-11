
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
	public partial class TomarNota : PopupPage
	{
		// ============================================================================================== //

		// Variables y constantes

		public readonly ObservableCollection<Articulo> ArticulosSeleccionados = new();

	// ============================================================================================== //

		// Inicialización

		public TomarNota()
		{
			InitializeComponent();

			ListaArticulos.ItemsSource = ArticulosSeleccionados;
		}

	// ============================================================================================== //

		// Eventos UI -> Contenido

		private async void SeleccionarMesa_Clicked(object sender, EventArgs e)
		{
			var popupSeleccionarMesa = new SeleccionarMesa();
			await Navigation.PushPopupAsync(popupSeleccionarMesa);
			var resultado = await popupSeleccionarMesa.Resultado;

			if(resultado.Correcto)
			{
				SeleccionarMesa.Text = resultado.NumeroMesaSeleccionada.ToString();
				SeleccionarMesa.BackgroundColor = Color.LimeGreen;
			}
		}

		private async void AnadirArticulo_Clicked(object sender, EventArgs e)
		{
			var popupAnadirArticulo = new AnadirArticulo();
			await Navigation.PushPopupAsync(popupAnadirArticulo);
			var resultado = await popupAnadirArticulo.Resultado;

			if(resultado.Correcto)
			{
				bool articuloYaSeleccionado(Articulo a) => a.Nombre == resultado.Articulo.Nombre;

				if(ArticulosSeleccionados.ToList().Any(articuloYaSeleccionado))
				{
					ArticulosSeleccionados
						.First(articuloYaSeleccionado)
							.Unidades += 1;

					// Sin esto, no se actualiza el ListView, por alguna razón (biende raro)
					ListaArticulos.ItemsSource = null;
					ListaArticulos.ItemsSource = ArticulosSeleccionados;
				}
				else
				{
					resultado.Articulo.Unidades = 1;
					ArticulosSeleccionados.Add(resultado.Articulo);
				}
			}
		}

		private void QuitarUnidadArticulo_Clicked(object sender, EventArgs e)
		{
			var nombreArticulo = (string)((Button)sender).BindingContext;

			bool articuloYaSeleccionado(Articulo a) => a.Nombre.Equals(nombreArticulo);

			var articuloAModificar = ArticulosSeleccionados.First(articuloYaSeleccionado);

			if(articuloAModificar.Unidades == 1)
				ArticulosSeleccionados.Remove(articuloAModificar);
			else
			{
				articuloAModificar.Unidades -= 1;

				// Sin esto, no se actualiza el ListView, por alguna razón (biende raro)
				ListaArticulos.ItemsSource = null;
				ListaArticulos.ItemsSource = ArticulosSeleccionados;
			}
		}

		private void AnadirUnidadArticulo_Clicked(object sender, EventArgs e)
		{
			var nombreArticulo = (string)((Button)sender).BindingContext;

			bool articuloYaSeleccionado(Articulo a) => a.Nombre.Equals(nombreArticulo);

			var articuloAModificar = ArticulosSeleccionados.First(articuloYaSeleccionado);

			if(articuloAModificar.Unidades < 255)
			{
				articuloAModificar.Unidades += 1;
				// TODO - PROBAR ESTO
				ArticulosSeleccionados.Ordenar((a,b) => a.Nombre.CompareTo(b.Nombre) );

				// Sin esto, no se actualiza el ListView, por alguna razón (biende raro)
				ListaArticulos.ItemsSource = null;
				ListaArticulos.ItemsSource = ArticulosSeleccionados;
			}
		}

		private async void Cancelar_Clicked(object sender, EventArgs e)
		{
			await Navigation.PopPopupAsync();
		}

		private async void Aceptar_Clicked(object sender, EventArgs e)
		{
			if(!byte.TryParse(SeleccionarMesa.Text, out byte mesaSeleccionada))
			{
				await UserDialogs.Instance.AlertAsync("No se ha seleccionado una mesa", "Alerta", "Aceptar");

				return;
			}
			else if(ArticulosSeleccionados.Count() == 0)
			{
				await UserDialogs.Instance.AlertAsync("No se ha seleccionado ningún artículo", "Alerta", "Aceptar");

				return;
			}

			UserDialogs.Instance.ShowLoading("Mandando pedido...");

			await Task.Run(() =>
			{
				new Comando_TomarNota(mesaSeleccionada, ArticulosSeleccionados.ToArray()).Enviar(Global.IPGestor);
			});

			UserDialogs.Instance.HideLoading();

			await Navigation.PopPopupAsync();
		}

	// ============================================================================================== //

		// Métodos privados

	// ============================================================================================== //

		// Métodos Procesar

	// ============================================================================================== //
	}
}