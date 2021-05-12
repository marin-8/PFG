
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;
using System.Text.RegularExpressions;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

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

				Global.TareasPersonales.Remove(tareaPulsada);
				Global.TareasPersonales.Ordenar();
			}
		}

	// ============================================================================================== //

		// Métodos privados

		private async void RefrescarTareasPersonales()
		{
			UserDialogs.Instance.ShowLoading("Actualizando lista de tareas...");

			var comandoRespuesta = await Task.Run(() =>
			{
				string respuestaGestor = new Comando_PedirTareas(Global.UsuarioActual.NombreUsuario).Enviar(Global.IPGestor);
				return Comando.DeJson<Comando_MandarTareas>(respuestaGestor);
			});
			Procesar_RecibirTareas(comandoRespuesta); 

			UserDialogs.Instance.HideLoading();
		}

	// ============================================================================================== //

		// Métodos Procesar

		private void Procesar_RecibirTareas(Comando_MandarTareas Comando)
		{
			Global.TareasPersonales.Clear();
			
			foreach(var tarea in Comando.Tareas)
				Global.TareasPersonales.Add(tarea);

			Global.TareasPersonales.Ordenar();

			ListaTareas.EndRefresh();
		}

	// ============================================================================================== //
	}
}
