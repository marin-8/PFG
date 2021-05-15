
using System;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Extensions;

using PFG.Comun;

namespace PFG.Aplicacion
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SeleccionarArticulo : PopupPage
	{
    // ============================================================================================== //

        // Variables y constantes

		private TaskCompletionSource<(bool Correcto, Articulo Articulo)> _taskCompletionSource;
		public Task<(bool Correcto, Articulo Articulo)> Resultado => _taskCompletionSource.Task;

		private readonly bool PermitirSeleccionarAcabados;
		private Articulo ResultadoArticulo;

    // ============================================================================================== //

        // Inicialización

		public SeleccionarArticulo(bool PermitirSeleccionarAcabados = false)
		{
			InitializeComponent();

			lock(Global.CategoriasLock)
				ListaArticulos.ItemsSource = Global.Categorias;

			this.PermitirSeleccionarAcabados = PermitirSeleccionarAcabados;
		}

		protected override async void OnAppearing()
		{
			base.OnAppearing();

			_taskCompletionSource = new TaskCompletionSource<(bool Correcto, Articulo Articulo)>();

			await Global.Get_Articulos();
		}

        protected override void OnDisappearing()
        {
            base.OnDisappearing(); 
			
			if(ResultadoArticulo != null)
			{
				_taskCompletionSource.SetResult((true, ResultadoArticulo));
			}
            else
			{
				_taskCompletionSource.SetResult((false, null));
			}
        }

    // ============================================================================================== //

        // Eventos UI -> Barra navegación

    // ============================================================================================== //

        // Eventos UI -> Contenido

		private async void ListaArticulos_Refresh(object sender, EventArgs e)
		{
			await Global.Get_Articulos();

			ListaArticulos.EndRefresh();
		}

		private async void ListaArticulos_ItemTapped(object sender, ItemTappedEventArgs e)
		{
			var articuloPulsado = (Articulo)e.Item;

			if(articuloPulsado.Disponible || PermitirSeleccionarAcabados)
			{
				ResultadoArticulo = (Articulo)e.Item;

				await Navigation.PopPopupAsync();
			}
		}

    // ============================================================================================== //

        // Métodos Helper

    // ============================================================================================== //

        // Métodos Procesar

    // ==============================================================================================
	}
}