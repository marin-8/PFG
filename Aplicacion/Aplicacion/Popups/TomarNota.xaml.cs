
using System;
using System.Linq;
using System.Threading.Tasks;
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
		public readonly object ArticulosSeleccionadosLock = new();

	// ============================================================================================== //

		// Inicialización

		public TomarNota()
		{
			InitializeComponent();

			lock(ArticulosSeleccionadosLock)
				ListaArticulos.ItemsSource = ArticulosSeleccionados;
		}

    // ============================================================================================== //

        // Eventos UI -> Barra navegación

	// ============================================================================================== //

		// Eventos UI -> Contenido

		private async void SeleccionarMesa_Clicked(object sender, EventArgs e)
		{
			while(true)
			{
				var popupSeleccionarMesa = new SeleccionarMesa();
				await Navigation.PushPopupAsync(popupSeleccionarMesa);
				var resultado = await popupSeleccionarMesa.Resultado;

				if(resultado.Correcto)
				{
					var estadoMesaSeleccionada =
						Global.Mesas
							.First(m => m.Numero == resultado.NumeroMesaSeleccionada)
								.EstadoMesa;

					if(estadoMesaSeleccionada == EstadosMesa.Sucia)
						await UserDialogs.Instance.AlertAsync("No se puede tomar nota a una mesa que está sucia", "Alerta", "Aceptar");
					
					else {
						SeleccionarMesa.Text = resultado.NumeroMesaSeleccionada.ToString();
						SeleccionarMesa.BackgroundColor = Color.LimeGreen;;
						return; }
				}
				else
					return;
			}
		}

		private async void AnadirArticulo_Clicked(object sender, EventArgs e)
		{
			var popupSeleccionarArticulo = new SeleccionarArticulo();
			await Navigation.PushPopupAsync(popupSeleccionarArticulo);
			var resultado = await popupSeleccionarArticulo.Resultado;

			if(resultado.Correcto)
			{
				bool articuloYaSeleccionado(Articulo a) => a.Nombre == resultado.Articulo.Nombre;

				lock(ArticulosSeleccionadosLock)
				{
					if(ArticulosSeleccionados.Any(articuloYaSeleccionado))
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
						ArticulosSeleccionados.Ordenar((a,b) => a.Nombre.CompareTo(b.Nombre));
					}
				}
			}
		}

		private void QuitarUnidadArticulo_Clicked(object sender, EventArgs e)
		{
			var nombreArticulo = (string)((Button)sender).BindingContext;

			bool articuloYaSeleccionado(Articulo a) => a.Nombre == nombreArticulo;

			lock(ArticulosSeleccionadosLock)
			{
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
		}

		private void AnadirUnidadArticulo_Clicked(object sender, EventArgs e)
		{
			var nombreArticulo = (string)((Button)sender).BindingContext;

			bool articuloYaSeleccionado(Articulo a) => a.Nombre == nombreArticulo;

			lock(ArticulosSeleccionadosLock)
			{
				var articuloAModificar = ArticulosSeleccionados.First(articuloYaSeleccionado);

				if(articuloAModificar.Unidades < 255)
				{
					articuloAModificar.Unidades += 1;

					ArticulosSeleccionados.Ordenar((a,b) => a.Nombre.CompareTo(b.Nombre));

					// Sin esto, no se actualiza el ListView, por alguna razón (biende raro)
					ListaArticulos.ItemsSource = null;
					ListaArticulos.ItemsSource = ArticulosSeleccionados;
				}
			}
		}

		private async void Cancelar_Clicked(object sender, EventArgs e)
		{
			await Navigation.PopPopupAsync();
		}

		private async void Aceptar_Clicked(object sender, EventArgs e)
		{
			int articulosSeleccionadosCount;
			lock(ArticulosSeleccionadosLock)
				articulosSeleccionadosCount = ArticulosSeleccionados.Count();

			if(!byte.TryParse(SeleccionarMesa.Text, out byte mesaSeleccionada))
			{
				await UserDialogs.Instance.AlertAsync("No se ha seleccionado una mesa", "Alerta", "Aceptar");

				return;
			}
			else if(articulosSeleccionadosCount == 0)
			{
				await UserDialogs.Instance.AlertAsync("No se ha seleccionado ningún artículo", "Alerta", "Aceptar");

				return;
			}

			UserDialogs.Instance.ShowLoading("Mandando pedido...");

			Articulo[] articulosSeleccionados;
			lock(ArticulosSeleccionadosLock)
				articulosSeleccionados = ArticulosSeleccionados.ToArray();

			await Task.Run(() =>
			{
				new Comando_TomarNota(mesaSeleccionada, articulosSeleccionados).Enviar(Global.IPGestor);
			});

			UserDialogs.Instance.HideLoading();

			await Navigation.PopPopupAsync();
		}

	// ============================================================================================== //

		// Métodos Helper

	// ============================================================================================== //

		// Métodos Procesar

	// ============================================================================================== //
	}
}