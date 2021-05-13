
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
using Rg.Plugins.Popup.Pages;
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

			// TODO - Ajuste para acciones por rol o siempre todas

			var accionesRolActual =
				Comun.Global.RolesAcciones
					//.Where(ra => ra.Key == Global.UsuarioActual.Rol) // o esto
					.Select(ra => ra.Value)
					.Distinct();									   // o esto

			int row = 0;
			foreach (var accion in accionesRolActual)
			{
				AccionesGrid.RowDefinitions.Add(new());

				var botonaAccion = GenerarBotonAccion
				(
					Comun.Global.AccionesTitulos[accion],
					accion,
					AccionPulsada
				);
				AccionesGrid.Children.Add(botonaAccion, 0, row); row += 2;

				AccionesGrid.RowDefinitions.Add(new() { Height = 20 });
			}

			if(AccionesGrid.RowDefinitions.Count > 0)
				AccionesGrid.RowDefinitions.RemoveAt(
					AccionesGrid.RowDefinitions.Count - 1);
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

		private async void AccionPulsada(object sender, EventArgs e)
		{
			var accionPulsada = (TiposAcciones)((Button)sender).BindingContext;

			switch(accionPulsada)
			{
				case TiposAcciones.TomarNota:
				{
					await Navigation.PushPopupAsync(new TomarNota());
					return;
				}

				case TiposAcciones.Cobrar:
				{
					while(true)
					{
						var popupSeleccionarMesa = new SeleccionarMesa();
						await Navigation.PushPopupAsync(popupSeleccionarMesa);
						var resultado = await popupSeleccionarMesa.Resultado;

						if(resultado.Correcto)
						{
							var estadoMesaSeleccionada =
								Global.Mesas
									.First(m => m.Numero == resultado.NumeroMesaSeleccionada)
										.EstadoMesa;

							if(estadoMesaSeleccionada == EstadosMesa.Vacia)
								await UserDialogs.Instance.AlertAsync("No se puede cobrar a una mesa vacía", "Alerta", "Aceptar");
							
							else if(estadoMesaSeleccionada == EstadosMesa.Esperando)
								await UserDialogs.Instance.AlertAsync("No se puede cobrar a una mesa que está esperando ser servida", "Alerta", "Aceptar");

							else if(estadoMesaSeleccionada == EstadosMesa.Sucia)
								await UserDialogs.Instance.AlertAsync("No se puede cobrar a una mesa vacía (además está sucia)", "Alerta", "Aceptar");

							else {
								await Navigation.PushPopupAsync(new CobrarTicket(resultado.NumeroMesaSeleccionada));
								return; }
						}
						else
							return;
					}
				}

				case TiposAcciones.CambiarDisponibilidadArticulo:
				{
					//await Navigation.PushPopupAsync(new XXXXX());
					return;
				}
			}
		}

	// ============================================================================================== //

		// Métodos privados

		private Button GenerarBotonAccion(string TituloAccion, TiposAcciones TipoAccion, EventHandler EventoMesaPulsada)
		{
			double tamanoFuente;

			switch(TipoAccion)
			{
				case TiposAcciones.TomarNota:

				case TiposAcciones.Cobrar:
					{ tamanoFuente = 48; break; }

				case TiposAcciones.CambiarDisponibilidadArticulo:
					{ tamanoFuente = 32; break; }

				default:
					{ tamanoFuente = 12; break; }
			}

			var buttonMesa = new Button()
			{
				Text=TituloAccion,
				FontAttributes=FontAttributes.Bold,
				FontSize=tamanoFuente,
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
