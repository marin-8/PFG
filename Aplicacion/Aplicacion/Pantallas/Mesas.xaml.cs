
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
	// ============================================================================================== //

		// Variables y constantes

		public static Mesas Instancia { get; private set; }

		private const int ESPACIO_ENTRE_MESAS = 10;

		private byte ColumnasMesas;
		private byte FilasMesas;
		private Mesa[] MesasExistentes;

	// ============================================================================================== //

		// Inicialización

		public Mesas()
		{
			InitializeComponent();

			Instancia = this;

			Shell.Current.Navigated += OnNavigatedTo;
		}

		private async void OnNavigatedTo(object sender, ShellNavigatedEventArgs e)
		{
			if(e.Current.Location.OriginalString.Contains(((BaseShellItem)Parent).Route.ToString()))
			{
				await PedirMesas();
			}
		}

	// ============================================================================================== //

		// Métodos públicos

		public async Task PedirMesas()
		{
			UserDialogs.Instance.ShowLoading("Actualizando mesas...");

			await Task.Run(() =>
			{
				new Comando_PedirMesas().Enviar(Global.IPGestor);
			});
		}

		public async Task ActualizarMesas(byte ColumnasMesas, byte FilasMesas, Mesa[] MesasExistentes)
		{
			this.ColumnasMesas = ColumnasMesas;
			this.FilasMesas = FilasMesas;
			this.MesasExistentes = MesasExistentes;

			await Device.InvokeOnMainThreadAsync(() =>
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

				foreach(var mesa in MesasExistentes)
				{
					var mesaMapaGrid = (Button)
							MapaGrid.Children
								.Where(c =>
									c.BindingContext
									.Equals($"{mesa.GridX}.{mesa.GridY}"))
								.First();

					mesaMapaGrid.Text = mesa.Numero.ToString();

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
			});
		}

	// ============================================================================================== //

		// Eventos UI -> Barra navegación 

		private static readonly string[] OpcionesEditarMapa = new string[]
		{
			"+ Añadir columna",
			"	- Quitar columna",
			"+ Añadir fila",
			"	- Quitar fila",
		};

		private async void EditarMapa(object sender, EventArgs e)
		{
			string opcion = await UserDialogs.Instance.ActionSheetAsync("Editar Mapa", "Cancelar", null, null, OpcionesEditarMapa);
			if(opcion.Equals("Cancelar")) return;

			if(opcion == OpcionesEditarMapa[0]) { AnadirColumna(); return; }
			if(opcion == OpcionesEditarMapa[1]) { QuitarColumna(); return; }
			if(opcion == OpcionesEditarMapa[2]) { AnadirFila(); return; }
			if(opcion == OpcionesEditarMapa[3]) { QuitarFila(); return; }
		}

		private async void RefrescarMesas(object sender, EventArgs e)
		{
			await PedirMesas();
		}

	// ============================================================================================== //

		// Eventos UI -> Contenido

		private async void MesaPulsada(object sender, EventArgs e)
		{
			// TODO - MesaPulsada

			await PedirMesas();

			var mesaString = (string)((Button)sender).BindingContext;
			int indiceDelPunto = mesaString.IndexOf('.');
			byte mesaX = byte.Parse(mesaString.Substring(0, indiceDelPunto));
			byte mesaY = byte.Parse(mesaString.Substring(indiceDelPunto+1, mesaString.Length-indiceDelPunto-1));

			var consultaMesa = MesasExistentes.Where(m => m.GridX == mesaX && m.GridY == mesaY);

			if(consultaMesa.Count() == 0)
			{
				byte mcm = Comun.Global.MAXIMO_COLUMNAS_MESAS;
				byte mfm = Comun.Global.MAXIMO_FILAS_MESAS;
				if(mcm*mfm > 255) throw new IndexOutOfRangeException("Hay más de 255 mesas :/");
				byte totalMesas = (byte)(mcm*mfm);

				byte numeroNuevaMesa;

				var configuracionPrompt = new PromptConfig
				{
					InputType = InputType.Number,
					IsCancellable = true,
					Title = $"Número de mesa (1-{totalMesas})",
					Message = "Nueva mesa",
					OkText = "Crear",
					CancelText = "Cancelar",
					MaxLength = (int)Math.Floor(Math.Log10(totalMesas)+1)
				};

				while(true)
				{
					var resultado = await UserDialogs.Instance.PromptAsync(configuracionPrompt);
					if(!resultado.Ok) return;
					numeroNuevaMesa = byte.Parse(resultado.Text);

					await PedirMesas();

					if(MesasExistentes.Where(m => m.Numero == numeroNuevaMesa).Any())
						await UserDialogs.Instance.AlertAsync("Ya existe una mesa con ese número", "Alerta", "Aceptar");
					else
						break;
				}

				Mesa nuevaMesa = new(numeroNuevaMesa, mesaX, mesaY);

				UserDialogs.Instance.ShowLoading("Creando mesa...");

				await Task.Run(() =>
				{
					new Comando_IntentarCrearMesa(nuevaMesa).Enviar(Global.IPGestor);
				});
			}
			else
			{
				var mesa = consultaMesa.First();
			}
		}

	// ============================================================================================== //

		// Métodos privados

		private Button GenerarBotonMesa(byte GridX, byte GridY)
		{
			var buttonMesa = new Button()
			{
				FontAttributes=FontAttributes.Bold,
				FontSize=20,
				Padding=new(0),
				Margin=new(0),
				BackgroundColor=Color.FromRgb(240, 240, 240),
				BindingContext=$"{GridX}.{GridY}"
			};
			buttonMesa.Clicked += MesaPulsada;

			return buttonMesa;
		}

		private async void AnadirColumna()
		{
			UserDialogs.Instance.ShowLoading("Añadiendo columna...");

			await Task.Run(() =>
			{
				new Comando_IntentarEditarMapaMesas(TiposEdicionMapaMesas.AnadirColumna).Enviar(Global.IPGestor);
			});
		}

		private async void QuitarColumna()
		{
			UserDialogs.Instance.ShowLoading("Añadiendo columna...");

			await Task.Run(() =>
			{
				new Comando_IntentarEditarMapaMesas(TiposEdicionMapaMesas.QuitarColumna).Enviar(Global.IPGestor);
			});
		}

		private async void AnadirFila()
		{
			UserDialogs.Instance.ShowLoading("Añadiendo columna...");

			await Task.Run(() =>
			{
				new Comando_IntentarEditarMapaMesas(TiposEdicionMapaMesas.AnadirFila).Enviar(Global.IPGestor);
			});
		}

		private async void QuitarFila()
		{
			UserDialogs.Instance.ShowLoading("Añadiendo columna...");

			await Task.Run(() =>
			{
				new Comando_IntentarEditarMapaMesas(TiposEdicionMapaMesas.QuitarFila).Enviar(Global.IPGestor);
			});
		}

	// ============================================================================================== //
	}
}
