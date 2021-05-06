
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
	public partial class Carta : ContentPage
	{
	// ============================================================================================== //

		// Variables y constantes

	// ============================================================================================== //

		// Inicialización

		public Carta()
		{
			InitializeComponent();

			Shell.Current.Navigated += OnNavigatedTo;

			ListaArticulos.ItemsSource = Global.CategoriasLocal;
		}

		private void OnNavigatedTo(object sender, ShellNavigatedEventArgs e)
		{
			if(e.Current.Location.OriginalString.Contains(((BaseShellItem)Parent).Route.ToString()))
			{
				RefrescarArticulos();
			}
		}

	// ============================================================================================== //

		// Eventos UI -> Barra navegación 

		private async void NuevoArticulo_Clicked(object sender, EventArgs e)
		{

		}

		private void Refrescar_Clicked(object sender, EventArgs e)
		{
			RefrescarArticulos();
		}

	// ============================================================================================== //

		// Eventos UI -> Contenido

		private void ListaArticulos_Refresh(object sender, EventArgs e)
		{
			RefrescarArticulos();
		}

		private async void ListaArticulos_ItemTapped(object sender, EventArgs e)
		{

		}

	// ============================================================================================== //

		// Métodos privados

		private async void RefrescarArticulos()
		{
			UserDialogs.Instance.ShowLoading("Actualizando lista de artículos...");

			var comandoRespuesta = await Task.Run(() =>
			{
				string respuestaGestor = new Comando_PedirArticulos().Enviar(Global.IPGestor);
				return Comando.DeJson<Comando_MandarArticulos>(respuestaGestor);
			});
			Procesar_RecibirArticulos(comandoRespuesta); 

			UserDialogs.Instance.HideLoading();
		}

	// ============================================================================================== //

		// Métodos Procesar

		private void Procesar_RecibirArticulos(Comando_MandarArticulos Comando)
		{
			Global.CategoriasLocal.Clear();

			var categorias =
				Comando.Articulos
					.OrderBy(a => a.Categoria)
					.GroupBy(a => a.Categoria)
					.Select(a => a.First().Categoria);

			foreach(var categoria in categorias)
			{
				var nuevaCategoria = new GrupoArticuloCategoria(categoria);

				nuevaCategoria.AddRange(
					Comando.Articulos
						.Where(a => a.Categoria.Equals(categoria))
						.OrderBy(a => a.Nombre));

				Global.CategoriasLocal.Add(nuevaCategoria);
			}

			ListaArticulos.EndRefresh();
		}

	// ============================================================================================== //
	}
}
