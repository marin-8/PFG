
using System;
using System.Threading.Tasks;

using Xamarin.Forms;

using Acr.UserDialogs;

using PFG.Comun;

namespace PFG.Aplicacion
{
	public partial class Tareas : ContentPage
	{
	// ============================================================================================== //

		// Variables y constantes

		private static Tareas _instancia;

	// ============================================================================================== //

		// Inicialización

		public Tareas()
		{
			InitializeComponent();

			Shell.Current.Navigated += OnNavigatedTo;

			lock(Global.TareasPersonalesLock)
				ListaTareas.ItemsSource = Global.TareasPersonales;

			_instancia = this;
		}

		private void OnNavigatedTo(object sender, ShellNavigatedEventArgs e)
		{
			if(e.Current.Location.OriginalString.Contains(((BaseShellItem)Parent).Route.ToString()))
			{
				RefrescarTareasPersonales();
			}
		}

	// ============================================================================================== //

		// Métodos públicos

		public static async void RefrescarTareasPersonalesDesdeFuera()
		{
			UserDialogs.Instance.ShowLoading("Cargando pantalla de tareas");

			await Task.Run(async () => {
				while(_instancia == null)
					await Task.Delay(25); });

			UserDialogs.Instance.HideLoading();

			_instancia.RefrescarTareasPersonales();
		}

	// ============================================================================================== //

		// Eventos UI -> Barra navegación 

		private void ListaTareas_Refresh(object sender, EventArgs e)
		{
			RefrescarTareasPersonales();
		}

	// ============================================================================================== //

		// Eventos UI -> Contenido

		private async void ListaTareas_ItemTapped(object sender, ItemTappedEventArgs e)
		{
			var tareaPulsada = (Tarea)e.Item;

			if(await UserDialogs.Instance.ConfirmAsync("¿Tarea completada?", "Confirmar", "Si", "Cancelar"))
			{
				await Task.Run(() =>
				{
					new Comando_TareaCompletada(tareaPulsada.ID).Enviar(Global.IPGestor);
				});

				lock(Global.TareasPersonalesLock)
				{
					Global.TareasPersonales.Remove(tareaPulsada);
					Global.TareasPersonales.Ordenar();
				}
			}
		}

	// ============================================================================================== //

		// Métodos Helper

		private async void RefrescarTareasPersonales()
		{
			Global.Get_TareasPersonales();

			ListaTareas.EndRefresh();
		}

	// ============================================================================================== //

		// Métodos Procesar

	// ============================================================================================== //
	}
}
