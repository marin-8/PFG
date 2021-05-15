
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

		private static readonly string[] OpcionesTarea = new string[]
		{
			"Completada",
			"Reasignar",
		};

		private async void ListaTareas_ItemTapped(object sender, ItemTappedEventArgs e)
		{
			var tareaPulsada = (Tarea)e.Item;

			string opcion = await UserDialogs.Instance.ActionSheetAsync("Opciones tarea", "Cancelar", null, null, OpcionesTarea);
			if(opcion == "Cancelar") return;

			if(opcion == OpcionesTarea[0]) // Completada
			{
				if(await UserDialogs.Instance.ConfirmAsync("Confirmar tarea completada", "¿Tarea completada?", "Completada", "Cancelar"))
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

				return;
			}

			if(opcion == OpcionesTarea[1]) // Reasignar
			{
				if(await UserDialogs.Instance.ConfirmAsync("Confirmar reasignación de tarea", "¿Reasignar tarea?", "Reasignar", "Cancelar"))
				{
					var comandoRespuesta = await Task.Run(() =>
					{
						string respuestaGestor = new Comando_ReasignarTarea(tareaPulsada.ID).Enviar(Global.IPGestor);
						return Comando.DeJson<Comando_ResultadoGenerico>(respuestaGestor);
					});

					Global.Procesar_ResultadoGenerico(comandoRespuesta, () =>
					{
						lock(Global.TareasPersonalesLock)
						{
							Global.TareasPersonales.Remove(tareaPulsada);
							Global.TareasPersonales.Ordenar();
						}
					});
				}

				return;
			}
		}

	// ============================================================================================== //

		// Métodos Helper

		private void RefrescarTareasPersonales()
		{
			Global.Get_TareasPersonales();

			ListaTareas.EndRefresh();
		}

	// ============================================================================================== //

		// Métodos Procesar

	// ============================================================================================== //
	}
}
