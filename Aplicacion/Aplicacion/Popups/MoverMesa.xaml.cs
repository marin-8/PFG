
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
	public partial class MoverMesa : PopupPage
	{
		private byte NumeroMesaSeleccionada;
		private Action<byte,byte,byte> EventoNuevoSitioSeleccionado;

		public MoverMesa(
			byte ColumnasMesas,
			byte FilasMesas,
			List<Mesa> MesasExistentes,
			byte NumeroMesaSeleccionada,
			Func<byte, byte, EventHandler, Button> GenerarBotonMesa,
			Action<byte,byte,byte> EventoNuevoSitioSeleccionado)
		{
			InitializeComponent();

			this.NumeroMesaSeleccionada = NumeroMesaSeleccionada;
			this.EventoNuevoSitioSeleccionado = EventoNuevoSitioSeleccionado;

			InicializarMapaGrid(ColumnasMesas, FilasMesas, MesasExistentes, NumeroMesaSeleccionada, GenerarBotonMesa);
		}

		private async void MesaPulsada(object sender, EventArgs e)
		{
			var botonPulsado = (Button)sender;

			if(botonPulsado.BackgroundColor == Color.Silver &&
			   botonPulsado.TextColor == Color.White)
			{
				await UserDialogs.Instance.AlertAsync("Selecciona un sitio destino distinto al de origen", "Alerta", "Aceptar");
			}
			else
			{
				var sitioPulsadoString = (string)botonPulsado.BindingContext;
				int indiceDelPunto = sitioPulsadoString.IndexOf('.');
				byte sitioPulsadoX = byte.Parse(sitioPulsadoString.Substring(0, indiceDelPunto));
				byte sitioPulsadoY = byte.Parse(sitioPulsadoString.Substring(indiceDelPunto+1, sitioPulsadoString.Length-indiceDelPunto-1));

				await Navigation.PopPopupAsync();

				EventoNuevoSitioSeleccionado.Invoke(NumeroMesaSeleccionada, sitioPulsadoX, sitioPulsadoY);
			}
		}

		private async void InicializarMapaGrid(
			byte ColumnasMesas,
			byte FilasMesas,
			List<Mesa> MesasExistentes,
			byte NumeroMesaSeleccionada,
			Func<byte, byte, EventHandler, Button> GenerarBotonMesa)
		{
			await Device.InvokeOnMainThreadAsync(() =>
			{
				MapaGrid.ColumnDefinitions.Clear();
				MapaGrid.RowDefinitions.Clear();

				for(int c = 0 ; c < ColumnasMesas-1; c++) {
					MapaGrid.ColumnDefinitions.Add(new());
					MapaGrid.ColumnDefinitions.Add(new(){Width=new(Mesas.ESPACIO_ENTRE_MESAS)}); }
				MapaGrid.ColumnDefinitions.Add(new());

				for(int f = 0 ; f < FilasMesas-1 ; f++) {
					MapaGrid.RowDefinitions.Add(new());
					MapaGrid.RowDefinitions.Add(new(){Height=new(Mesas.ESPACIO_ENTRE_MESAS)}); }
				MapaGrid.RowDefinitions.Add(new());

				MapaGrid.Children.Clear();

				for(int c = 0 ; c < ColumnasMesas; c++)
				{
					for(int f = 0 ; f < FilasMesas ; f++)
					{
						MapaGrid.Children.Add(
							GenerarBotonMesa((byte)(c+1), (byte)(f+1), MesaPulsada),
							c*2,
							f*2);
					}
				}

				foreach(var mesa in MesasExistentes)
				{
					var mesaMapaGrid = (Button)
							MapaGrid.Children
								.Where(c =>
									c.BindingContext
									.Equals($"{mesa.SitioX}.{mesa.SitioY}"))
								.First();

					mesaMapaGrid.Text = mesa.Numero.ToString();

					if(mesa.Numero == NumeroMesaSeleccionada)
					{
						mesaMapaGrid.BackgroundColor = Color.Silver;
						mesaMapaGrid.TextColor = Color.White;
					}
					else
					{
						switch(mesa.EstadoMesa)
						{
							case EstadosMesa.Vacia:
							{
								mesaMapaGrid.BackgroundColor = Color.LimeGreen;
								mesaMapaGrid.TextColor = Color.Black;
								break;
							}
							case EstadosMesa.Esperando:
							{
								mesaMapaGrid.BackgroundColor = Color.Red;
								mesaMapaGrid.TextColor = Color.White;
								break;
							}
							case EstadosMesa.Ocupada:
							{
								mesaMapaGrid.BackgroundColor = Color.Black;
								mesaMapaGrid.TextColor = Color.White;
								break;
							}
							case EstadosMesa.Sucia:
							{
								mesaMapaGrid.BackgroundColor = Color.DarkGreen;
								mesaMapaGrid.TextColor = Color.White;
								break;
							}
						}
					}
				}
			});
		}
	}
}