
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
	public partial class AnadirArticulo : PopupPage
	{
		private TaskCompletionSource<(bool Correcto, Articulo Articulo)> _taskCompletionSource;
		public Task<(bool Correcto, Articulo Articulo)> Resultado => _taskCompletionSource.Task;

		private Articulo ResultadoArticulo;

		public AnadirArticulo()
		{
			InitializeComponent();

			lock(Global.CategoriasLock)
				ListaArticulos.ItemsSource = Global.Categorias;
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

		private async void ListaArticulos_Refresh(object sender, EventArgs e)
		{
			await Global.Get_Articulos();

			ListaArticulos.EndRefresh();
		}

		private async void ListaArticulos_ItemTapped(object sender, ItemTappedEventArgs e)
		{
			var articuloPulsado = (Articulo)e.Item;

			if(articuloPulsado.Disponible)
			{
				ResultadoArticulo = (Articulo)e.Item;

				await Navigation.PopPopupAsync();
			}
		}
	}
}