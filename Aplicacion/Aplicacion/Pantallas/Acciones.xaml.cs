
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
using Rg.Plugins.Popup.Extensions;

using PFG.Comun;

namespace PFG.Aplicacion
{
	public partial class Acciones : ContentPage
	{
	// ============================================================================================== //

		// Variables y constantes

	// ============================================================================================== //

		// Inicialización

		public Acciones()
		{
			InitializeComponent();

			var accionesRolActual =
				Comun.Global.RolesAcciones
					.Where(ra => ra.Key == Global.UsuarioActual.Rol)
					.Select(ra => ra.Value);

			foreach(var accion in accionesRolActual)
			{
				AccionesGrid.RowDefinitions.Add(new());
				AccionesGrid.Children.Add(GenerarBotonAccion(Comun.Global.AccionesTitulos[accion], accion, AccionPulsada));

				AccionesGrid.RowDefinitions.Add(new() { Height=20 });
			}

			AccionesGrid.RowDefinitions.RemoveAt(AccionesGrid.RowDefinitions.Count-1);
		}

		//private void OnNavigatedTo(object sender, ShellNavigatedEventArgs e)
		//{
		//	if(e.Current.Location.OriginalString.Contains(((BaseShellItem)Parent).Route.ToString()))
		//	{
		//		RefrescarArticulos();
		//	}
		//}

	// ============================================================================================== //

		// Eventos UI -> Barra navegación 

	// ============================================================================================== //

		// Eventos UI -> Contenido

		private void AccionPulsada(object sender, EventArgs e)
		{

		}

	// ============================================================================================== //

		// Métodos privados

		private Button GenerarBotonAccion(string TituloAccion, TiposAcciones TipoAccion, EventHandler EventoMesaPulsada)
		{
			var buttonMesa = new Button()
			{
				Text=TituloAccion,
				FontAttributes=FontAttributes.Bold,
				FontSize=20,
				Padding=new(0),
				Margin=new(0),
				BackgroundColor=Color.LimeGreen,
				BindingContext=TipoAccion
			};
			buttonMesa.Clicked += EventoMesaPulsada;

			return buttonMesa;
		}

	// ============================================================================================== //

		// Métodos Procesar

	// ============================================================================================== //
	}
}
