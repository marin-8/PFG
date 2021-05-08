
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

		public readonly ObservableCollection<Articulo> ArticulosSeleccionados = new()
		{
			new("Cerveza Franziskaner", "Bebidas", 0.01f, 2),
			new("Marianito", "Bebidas", 0.01f, 3),
			new("Patatas asadas con salsa de champiñones y queso", "Entrantes", 0.01f, 255),
			new("Cerveza Franziskaner", "Bebidas", 0.01f, 2),
			new("Marianito", "Bebidas", 0.01f, 3),
			new("Patatas asadas con salsa de champiñones y queso", "Entrantes", 0.01f, 255),
			new("Cerveza Franziskaner", "Bebidas", 0.01f, 2),
			new("Marianito", "Bebidas", 0.01f, 3),
			new("Patatas asadas con salsa de champiñones y queso", "Entrantes", 0.01f, 255),
		};

	// ============================================================================================== //

		// Inicialización

		public TomarNota()
		{
			InitializeComponent();

			ListaArticulos.ItemsSource = ArticulosSeleccionados;
		}

	// ============================================================================================== //

		// Eventos UI -> Contenido

		private void SeleccionarMesa_Clicked(object sender, EventArgs e)
		{

		}

		private void AnadirArticulo_Clicked(object sender, EventArgs e)
		{

		}

		private void QuitarUnidadArticulo_Clicked(object sender, EventArgs e)
		{

		}

		private void AnadirUnidadArticulo_Clicked(object sender, EventArgs e)
		{

		}

		private void Cancelar_Clicked(object sender, EventArgs e)
		{

		}

		private void Aceptar_Clicked(object sender, EventArgs e)
		{

		}

	// ============================================================================================== //

		// Métodos privados

	// ============================================================================================== //

		// Métodos Procesar

	// ============================================================================================== //
	}
}