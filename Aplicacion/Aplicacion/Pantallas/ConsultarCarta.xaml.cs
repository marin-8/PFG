
using System;

using Xamarin.Forms;

namespace PFG.Aplicacion
{
	public partial class ConsultarCarta : ContentPage
	{
	// ============================================================================================== //

		// Variables y constantes

	// ============================================================================================== //

		// Inicialización

		public ConsultarCarta()
		{
			InitializeComponent();

			lock(Global.CategoriasLock)
				ListaArticulos.ItemsSource = Global.Categorias;
		}

		protected override async void OnAppearing()
		{
			await Global.Get_Articulos();
		}

	// ============================================================================================== //

		// Eventos UI -> Barra navegación 

		private async void Refrescar_Clicked(object sender, EventArgs e)
		{
			await Global.Get_Articulos();
		}

	// ============================================================================================== //

		// Eventos UI -> Contenido

		private async void ListaArticulos_Refresh(object sender, EventArgs e)
		{
			await Global.Get_Articulos();

			ListaArticulos.EndRefresh();
		}

	// ============================================================================================== //

		// Métodos Helper

	// ============================================================================================== //

		// Métodos Procesar

	// ============================================================================================== //
	}
}
