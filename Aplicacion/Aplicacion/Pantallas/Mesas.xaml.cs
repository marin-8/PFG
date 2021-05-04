
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
		// TODO - Modificar número mesa
		// TODO - Mover mesa
		// TODO - Eliminar mesa

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

		private void OnNavigatedTo(object sender, ShellNavigatedEventArgs e)
		{
			if(e.Current.Location.OriginalString.Contains(((BaseShellItem)Parent).Route.ToString()))
			{
				PedirMesas();
			}
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

		private void RefrescarMesas(object sender, EventArgs e)
		{
			PedirMesas();
		}

	// ============================================================================================== //

		// Eventos UI -> Contenido

		private static readonly string[] OpcionesMesaExistente = new string[]
		{
			"Cambiar número",
			"Mover",
			"Eliminar",
		};

		private async void MesaPulsada(object sender, EventArgs e)
		{
			PedirMesas();

			var sitioPulsadoString = (string)((Button)sender).BindingContext;
			int indiceDelPunto = sitioPulsadoString.IndexOf('.');
			byte sitioPulsadoX = byte.Parse(sitioPulsadoString.Substring(0, indiceDelPunto));
			byte sitioPulsadoY = byte.Parse(sitioPulsadoString.Substring(indiceDelPunto+1, sitioPulsadoString.Length-indiceDelPunto-1));

			bool condicionMesaSeleccionada(Mesa m) => m.SitioX == sitioPulsadoX && m.SitioY == sitioPulsadoY;

			if (!MesasExistentes.Any(condicionMesaSeleccionada)) // Crear mesa
			{
				CrearMesa(sitioPulsadoX, sitioPulsadoY);
			}
			else
			{
				var mesaSeleccionada = MesasExistentes.Where(condicionMesaSeleccionada).First();

				string opcion = await UserDialogs.Instance.ActionSheetAsync($"Mesa {mesaSeleccionada.Numero}", "Cancelar", null, null, OpcionesMesaExistente);
				if(opcion.Equals("Cancelar")) return;

				if(opcion == OpcionesMesaExistente[0]) // Cambiar número
				{
					//

					return;
				}

				if(opcion == OpcionesMesaExistente[1]) // Mover
				{
					//

					return;
				}

				if(opcion == OpcionesMesaExistente[2]) // Eliminar
				{
					//

					return;
				}
			}
		}

	// ============================================================================================== //

		// Métodos privados

		private async void PedirMesas()
		{
			UserDialogs.Instance.ShowLoading("Actualizando mesas...");

			await Task.Run(() =>
			{
				string respuestaGestor = new Comando_PedirMesas().Enviar(Global.IPGestor);
				var comandoRespuesta = Comando.DeJson<Comando_MandarMesas>(respuestaGestor);
				Procesar_RecibirMesas(comandoRespuesta); 
			});

			UserDialogs.Instance.HideLoading();
		}

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
				new Comando_EditarMapaMesas(TiposEdicionMapaMesas.AnadirColumna).Enviar(Global.IPGestor);
			});
		}

		private async void QuitarColumna()
		{
			UserDialogs.Instance.ShowLoading("Añadiendo columna...");

			await Task.Run(() =>
			{
				new Comando_EditarMapaMesas(TiposEdicionMapaMesas.QuitarColumna).Enviar(Global.IPGestor);
			});
		}

		private async void AnadirFila()
		{
			UserDialogs.Instance.ShowLoading("Añadiendo columna...");

			await Task.Run(() =>
			{
				new Comando_EditarMapaMesas(TiposEdicionMapaMesas.AnadirFila).Enviar(Global.IPGestor);
			});
		}

		private async void QuitarFila()
		{
			UserDialogs.Instance.ShowLoading("Añadiendo columna...");

			await Task.Run(() =>
			{
				new Comando_EditarMapaMesas(TiposEdicionMapaMesas.QuitarFila).Enviar(Global.IPGestor);
			});
		}

		private async void CrearMesa(byte sitioX, byte sitioY)
		{
			byte mcm = Comun.Global.MAXIMO_COLUMNAS_MESAS;
			byte mfm = Comun.Global.MAXIMO_FILAS_MESAS;
			if(mcm*mfm > 255) throw new IndexOutOfRangeException("Hay más de 255 mesas :/");
			byte totalMesas = (byte)(mcm*mfm);

			byte numeroNuevaMesa;

			while(true)
			{
				var configuracionPrompt = new PromptConfig
				{
					InputType = InputType.Number,
					IsCancellable = true,
					Title = "Nueva mesa",
					Message = $"Número de mesa (1-{totalMesas})",
					OkText = "Crear",
					CancelText = "Cancelar",
					MaxLength = (int)Math.Floor(Math.Log10(totalMesas)+1),
				};

				var resultado = await UserDialogs.Instance.PromptAsync(configuracionPrompt);
				if(!resultado.Ok) return;

				if(resultado.Text.Equals("") || resultado.Text.Equals("0") || resultado.Text.Equals("00")) {
					await UserDialogs.Instance.AlertAsync("El número introducido no es válido", "Alerta", "Aceptar"); continue; }

				numeroNuevaMesa = byte.Parse(resultado.Text);

				PedirMesas();

				if(MesasExistentes.Where(m => m.Numero == numeroNuevaMesa).Any())
					await UserDialogs.Instance.AlertAsync("Ya existe una mesa con ese número", "Alerta", "Aceptar");
				else
					break;
			}

			Mesa nuevaMesa = new(numeroNuevaMesa, sitioX, sitioY);

			UserDialogs.Instance.ShowLoading("Creando mesa...");

			await Task.Run(() =>
			{
				new Comando_CrearMesa(nuevaMesa).Enviar(Global.IPGestor);
			});
		}
		
	// ============================================================================================== //

		// Métodos Procesar

		private async void Procesar_RecibirMesas(Comando_MandarMesas Comando)
		{
			ColumnasMesas = Comando.AnchoGrid;
			FilasMesas = Comando.AltoGrid;
			MesasExistentes = Comando.Mesas;

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
									.Equals($"{mesa.SitioX}.{mesa.SitioY}"))
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
	}
}
