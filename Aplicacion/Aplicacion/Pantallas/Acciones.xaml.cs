
using System;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;

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

			var accionesAMostrar =
				Comun.Global.RolesAcciones
					//.Where(ra => ra.Key == Global.UsuarioActual.Rol) // esto para filtrar acciones por rol
					.Select(ra => ra.Value)
					.Distinct();									   // esto para todas

			int row = 0;
			foreach (var accion in accionesAMostrar)
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

						#pragma warning disable IDE0042
						var resultado = await popupSeleccionarMesa.Resultado;
						#pragma warning restore IDE0042

						if (resultado.Correcto)
						{
							var estadoMesaSeleccionada =
								Global.Mesas
									.Single(m => m.Numero == resultado.NumeroMesaSeleccionada)
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
					var popupSeleccionarArticulo = new SeleccionarArticulo(true);
					await Navigation.PushPopupAsync(popupSeleccionarArticulo);
					var resultado = await popupSeleccionarArticulo.Resultado;

					if(resultado.Correcto)
					{
						string nuevoEstadoString;

						if(resultado.Articulo.Disponible)
							nuevoEstadoString = "Acabado";
						else
							nuevoEstadoString = "Disponible";

						if(await UserDialogs.Instance.ConfirmAsync($"Artículo: {resultado.Articulo.Nombre}",
							$"¿Marcar artículo como {nuevoEstadoString}?", "Si", "Cancelar"))
						{
							await Task.Run(() =>
							{
								new Comando_CambiarDisponibilidadArticulo(resultado.Articulo.Nombre).Enviar(Global.IPGestor);
							});
						}
					}					

					return;
				}
			}
		}

	// ============================================================================================== //

		// Métodos Helper

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
