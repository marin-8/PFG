
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
	public partial class Mesas : ContentPage
	{
		public static Mesas Instancia { get; private set; }

		private const int ESPACIO_ENTRE_MESAS = 10;

		private byte ColumnasMesas;
		private byte FilasMesas;
		private Mesa[] MesasExistentes;

		public Mesas()
		{
			InitializeComponent();

			Instancia = this;

			Shell.Current.Navigated += OnNavigatedTo;
		}

		private void OnNavigatedTo(object sender, ShellNavigatedEventArgs e)
		{
			if(e.Current.Location.OriginalString.Contains(((BaseShellItem)Parent).Route.ToString()))
			{
				PedirMesas();
			}
		}

		private async void PedirMesas()
		{
			UserDialogs.Instance.ShowLoading("Actualizando mesas...");

			await Task.Run(() =>
			{
				new Comando_PedirMesas().Enviar(Global.IPGestor);
			});
		}

		public async void RefrescarMesas(byte ColumnasMesas, byte FilasMesas, Mesa[] MesasExistentes)
		{
			this.ColumnasMesas = ColumnasMesas;
			this.FilasMesas = FilasMesas;
			this.MesasExistentes = MesasExistentes;

			await Device.InvokeOnMainThreadAsync(async () =>
			{
				MapaGrid.ColumnDefinitions.Clear();
				MapaGrid.RowDefinitions.Clear();

				for(int c = 0 ; c < ColumnasMesas-1; c++) {
					MapaGrid.ColumnDefinitions.Add(new());
					MapaGrid.ColumnDefinitions.Add(new(){Width=new(ESPACIO_ENTRE_MESAS)}); }
				MapaGrid.ColumnDefinitions.Add(new());

				for(int f = 0 ; f < FilasMesas-1 ; f++) {
					MapaGrid.RowDefinitions.Add(new());
					MapaGrid.RowDefinitions.Add(new(){Height=new(ESPACIO_ENTRE_MESAS)}); }
				MapaGrid.RowDefinitions.Add(new());

				MapaGrid.Children.Clear();

				for(int c = 0 ; c < ColumnasMesas; c++)
				{
					for(int f = 0 ; f < FilasMesas ; f++)
					{
						MapaGrid.Children.Add(
							GenerarBotonMesa((byte)(c+1), (byte)(f+1)),
							c*2,
							f*2);
					}
				}
			});
		}

		private Button GenerarBotonMesa(byte GridX, byte GridY)
		{
			var buttonMesa = new Button()
			{
				FontAttributes=FontAttributes.Bold,
				TextColor=Color.Silver, 
				FontSize=16,
				BackgroundColor=Color.FromRgb(224, 224, 224),
				BindingContext=$"{GridX}.{GridY}"
			};
			buttonMesa.Clicked += MesaPulsada;

			return buttonMesa;
		}

		private void Refrescar_Clicked(object sender, EventArgs e)
		{
			PedirMesas();
		}

		private void MesaPulsada(object sender, EventArgs e)
		{
			var mesa = (string)((Button)sender).BindingContext;
		}

		private void AnadirFila_Clicked(object sender, EventArgs e)
		{
			MapaGrid.RowDefinitions.Add(new(){Height=new(ESPACIO_ENTRE_MESAS)});
			MapaGrid.RowDefinitions.Add(new());

			int columnasGrid = MapaGrid.ColumnDefinitions.Count;
			int filasGrid = MapaGrid.RowDefinitions.Count;

			for (int c = 0; c < (columnasGrid+1)/2; c++)
			{
				MapaGrid.Children.Add(
					GenerarBotonMesa((byte)(c+1), (byte)((filasGrid+1)/2)),
					c*2,
					filasGrid-1);
			}
		}

		private void EliminarFila_Clicked(object sender, EventArgs e)
		{

		}

		private void AnadirColumna_Clicked(object sender, EventArgs e)
		{
			MapaGrid.ColumnDefinitions.Add(new(){Width=new(ESPACIO_ENTRE_MESAS)});
			MapaGrid.ColumnDefinitions.Add(new());

			int columnas = MapaGrid.ColumnDefinitions.Count;
			int filas = MapaGrid.RowDefinitions.Count;

			for (int f = 0 ; f < (filas+1)/2; f++)
			{
				MapaGrid.Children.Add(
					GenerarBotonMesa((byte)(columnas+1), (byte)((f+1)/2)),
					columnas-1,
					f*2);
			}
		}

		private void EliminarColumna_Clicked(object sender, EventArgs e)
		{

		}
	}
}
