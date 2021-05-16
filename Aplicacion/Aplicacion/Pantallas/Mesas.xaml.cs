
using System;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;

using Acr.UserDialogs;
using Rg.Plugins.Popup.Extensions;

using PFG.Comun;

namespace PFG.Aplicacion
{
	public partial class Mesas : ContentPage
	{
	// ============================================================================================== //

		// Variables y constantes

		public const int ESPACIO_ENTRE_MESAS = 10;

	// ============================================================================================== //

		// Inicialización

		public Mesas()
		{
			InitializeComponent();

			Shell.Current.Navigated += OnNavigatedTo;
		}

		private void OnNavigatedTo(object sender, ShellNavigatedEventArgs e)
		{
			if(e.Current.Location.OriginalString.Contains(((BaseShellItem)Parent).Route.ToString()))
			{
				ActualizarMapaMesas();
			}
		}

	// ============================================================================================== //

		// Eventos UI -> Barra navegación 

		private static readonly string[] OpcionesEditarMapa = new string[]
		{
			"+ Añadir columna",
			"- Quitar columna",
			"+ Añadir fila",
			"- Quitar fila",
		};

		private async void EditarMapa(object sender, EventArgs e)
		{
			string opcion = await UserDialogs.Instance.ActionSheetAsync("Editar Mapa", "Cancelar", null, null, OpcionesEditarMapa);
			if(opcion == "Cancelar") return;

			if(opcion == OpcionesEditarMapa[0]) { AnadirColumna(); return; }
			if(opcion == OpcionesEditarMapa[1]) { QuitarColumna(); return; }
			if(opcion == OpcionesEditarMapa[2]) { AnadirFila(); return; }
			if(opcion == OpcionesEditarMapa[3]) { QuitarFila(); return; }
		}

		private void RefrescarMesas(object sender, EventArgs e)
		{
			ActualizarMapaMesas();
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
			var sitioPulsadoString = (string)((Button)sender).BindingContext;
			int indiceDelPunto = sitioPulsadoString.IndexOf('.');
			byte sitioPulsadoX = byte.Parse(sitioPulsadoString.Substring(0, indiceDelPunto));
			byte sitioPulsadoY = byte.Parse(sitioPulsadoString.Substring(indiceDelPunto+1, sitioPulsadoString.Length-indiceDelPunto-1));

			bool condicionMesaSeleccionada(Mesa m) => m.SitioX == sitioPulsadoX && m.SitioY == sitioPulsadoY;

			if (!Global.Mesas.Any(condicionMesaSeleccionada)) // Crear mesa
			{
				CrearMesa(sitioPulsadoX, sitioPulsadoY);
			}
			else
			{
				var mesaSeleccionada = Global.Mesas.Where(condicionMesaSeleccionada).Single();

				string opcion = await UserDialogs.Instance.ActionSheetAsync($"Mesa {mesaSeleccionada.Numero}", "Cancelar", null, null, OpcionesMesaExistente);
				if(opcion == "Cancelar") return;

				if(opcion == OpcionesMesaExistente[0]) // Cambiar número
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
							Title = "Cambiar número",
							Message = $"Número de mesa (1-{totalMesas})\n(Actual = {mesaSeleccionada.Numero})",
							OkText = "Cambiar",
							CancelText = "Cancelar",
							MaxLength = (int)Math.Floor(Math.Log10(totalMesas)+1),
						};

						var resultado = await UserDialogs.Instance.PromptAsync(configuracionPrompt);
						if(!resultado.Ok) return;

						if(resultado.Text == "" || resultado.Text == "0" || resultado.Text == "00") {
							await UserDialogs.Instance.AlertAsync("El número introducido no es válido", "Alerta", "Aceptar"); continue; }

						numeroNuevaMesa = byte.Parse(resultado.Text);

						if(Global.Mesas.Where(m => m.Numero == numeroNuevaMesa).Any())
							await UserDialogs.Instance.AlertAsync("Ya existe una mesa con ese número", "Alerta", "Aceptar");
						else
							break;
					}

					UserDialogs.Instance.ShowLoading("Cambiando número de mesa...");

					var comandoRespuesta = await Task.Run(() =>
					{
						string respuestaGestor = new Comando_ModificarMesaNumero(mesaSeleccionada.Numero, numeroNuevaMesa).Enviar(Global.IPGestor);
						return Comando.DeJson<Comando_ResultadoGenerico>(respuestaGestor);
					});
					Global.Procesar_ResultadoGenerico(comandoRespuesta, ActualizarMapaMesas);

					UserDialogs.Instance.HideLoading();

					return;
				}

				if(opcion == OpcionesMesaExistente[1]) // Mover
				{
					await Navigation.PushPopupAsync(new MoverMesa(mesaSeleccionada.Numero, MoverMesa));
					return;
				}

				if(opcion == OpcionesMesaExistente[2]) // Eliminar
				{
					if(await UserDialogs.Instance.ConfirmAsync($"¿Eliminar la Mesa '{mesaSeleccionada.Numero}'?", "Confirmar eliminación", "Eliminar", "Cancelar"))
					{
						UserDialogs.Instance.ShowLoading("Eliminando mesa...");

						var comandoRespuesta = await Task.Run(() =>
						{
							string respuestaGestor = new Comando_EliminarMesa(mesaSeleccionada.Numero).Enviar(Global.IPGestor);
							return Comando.DeJson<Comando_ResultadoGenerico>(respuestaGestor);
						});
						Global.Procesar_ResultadoGenerico(comandoRespuesta, ActualizarMapaMesas);

						UserDialogs.Instance.HideLoading();

						return;
					}
				}
			}
		}

	// ============================================================================================== //

		// Métodos Helper

		private void AnadirColumna()
		{
			EditarMapaMesas("Añadiendo columna...", TiposEdicionMapaMesas.AnadirColumna);
		}

		private void QuitarColumna()
		{
			EditarMapaMesas("Quitando columna...", TiposEdicionMapaMesas.QuitarColumna);
		}

		private void AnadirFila()
		{
			EditarMapaMesas("Añadiendo fila...", TiposEdicionMapaMesas.AnadirFila);
		}

		private void QuitarFila()
		{
			EditarMapaMesas("Quitando fila...", TiposEdicionMapaMesas.QuitarFila);
		}

		private async void EditarMapaMesas(string TituloLoading, TiposEdicionMapaMesas TipoEdicionMapaMesas)
		{
			UserDialogs.Instance.ShowLoading(TituloLoading);

			var comandoRespuesta = await Task.Run(() =>
			{
				string respuestaGestor = new Comando_EditarMapaMesas(TipoEdicionMapaMesas).Enviar(Global.IPGestor);
				return Comando.DeJson<Comando_ResultadoGenerico>(respuestaGestor);
			});
			Global.Procesar_ResultadoGenerico(comandoRespuesta, ActualizarMapaMesas);

			UserDialogs.Instance.HideLoading();
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

				if(resultado.Text == "" || resultado.Text == "0" || resultado.Text == "00") {
					await UserDialogs.Instance.AlertAsync("El número introducido no es válido", "Alerta", "Aceptar"); continue; }

				numeroNuevaMesa = byte.Parse(resultado.Text);

				if(Global.Mesas.Where(m => m.Numero == numeroNuevaMesa).Any())
					await UserDialogs.Instance.AlertAsync("Ya existe una mesa con ese número", "Alerta", "Aceptar");
				else
					break;
			}

			Mesa nuevaMesa = new(numeroNuevaMesa, sitioX, sitioY);

			UserDialogs.Instance.ShowLoading("Creando mesa...");

			var comandoRespuesta = await Task.Run(() =>
			{
				string respuestaGestor = new Comando_CrearMesa(nuevaMesa).Enviar(Global.IPGestor);
				return Comando.DeJson<Comando_ResultadoGenerico>(respuestaGestor);
			});
			Global.Procesar_ResultadoGenerico(comandoRespuesta, ActualizarMapaMesas);

			UserDialogs.Instance.HideLoading();
		}

		private async void MoverMesa(byte numeroMesaOrigen, byte nuevoSitioX, byte nuevoSitioY)
		{
			UserDialogs.Instance.ShowLoading("Moviendo mesa...");

			var comandoRespuesta = await Task.Run(() =>
			{
				string respuestaGestor = new Comando_ModificarMesaSitio(numeroMesaOrigen, nuevoSitioX, nuevoSitioY).Enviar(Global.IPGestor);
				return Comando.DeJson<Comando_ResultadoGenerico>(respuestaGestor);
			});
			Global.Procesar_ResultadoGenerico(comandoRespuesta, ActualizarMapaMesas);

			UserDialogs.Instance.HideLoading();
		}
		
	// ============================================================================================== //

		// Métodos Procesar

		private async void ActualizarMapaMesas()
		{
			await Global.Get_Mesas();

			UserDialogs.Instance.ShowLoading("Actualizando mapa mesas...");

			await Device.InvokeOnMainThreadAsync(() =>
			{
				MapaGrid.ColumnDefinitions.Clear();
				MapaGrid.RowDefinitions.Clear();

				for(int c = 0 ; c < Global.AnchoMapaMesas-1; c++) {
					MapaGrid.ColumnDefinitions.Add(new());
					MapaGrid.ColumnDefinitions.Add(new(){Width=new(ESPACIO_ENTRE_MESAS)}); }
				MapaGrid.ColumnDefinitions.Add(new());

				for(int f = 0 ; f < Global.AltoMapaMesas-1 ; f++) {
					MapaGrid.RowDefinitions.Add(new());
					MapaGrid.RowDefinitions.Add(new(){Height=new(ESPACIO_ENTRE_MESAS)}); }
				MapaGrid.RowDefinitions.Add(new());

				MapaGrid.Children.Clear();

				for(int c = 0 ; c < Global.AnchoMapaMesas; c++)
				{
					for(int f = 0 ; f < Global.AltoMapaMesas ; f++)
					{
						MapaGrid.Children.Add(
							Global.GenerarBotonMesa((byte)(c+1), (byte)(f+1), MesaPulsada),
							c*2,
							f*2);
					}
				}

				foreach(var mesa in Global.Mesas)
				{
					var mesaMapaGrid = (Button)
							MapaGrid.Children
								.Where(c => c.BindingContext.ToString() == $"{mesa.SitioX}.{mesa.SitioY}")
								.Single();

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
							mesaMapaGrid.BackgroundColor = Color.DarkOrange;
							mesaMapaGrid.TextColor = Color.White;
							break;
						}
					}
				}
			});

			UserDialogs.Instance.HideLoading();
		}

	// ============================================================================================== //
	}
}
