
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

	// ============================================================================================== //

		// Inicialización

		public Tareas()
		{
			InitializeComponent();

			Shell.Current.Navigated += OnNavigatedTo;

			ListaTareas.ItemsSource = Global.TareasPersonales;
		}

		private void OnNavigatedTo(object sender, ShellNavigatedEventArgs e)
		{
			if(e.Current.Location.OriginalString.Contains(((BaseShellItem)Parent).Route.ToString()))
			{
				RefrescarTareasPersonales();
			}
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
			// TODO - ESTO

			await UserDialogs.Instance.AlertAsync("hey", "Sin implementar", "Aceptar");
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
