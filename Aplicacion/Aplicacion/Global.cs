
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Xamarin.Forms;

using Acr.UserDialogs;

using PFG.Comun;

namespace PFG.Aplicacion
{
	public static class Global
	{
		public static string IPGestor = "";

		public static Usuario UsuarioActual;

		public static ControladorRed Servidor;

		public static AjustesObjeto Ajustes = new();

		public static ObservableCollection<Usuario> Usuarios = new();
		public static readonly object UsuariosLock = new();

		public static byte AnchoMapaMesas { get; private set; }
		public static byte AltoMapaMesas { get; private set; }
		public static Mesa[] Mesas { get; private set; } = new Mesa[0];


		public static ObservableCollection<List<Articulo>> Categorias = new();
		public static readonly object CategoriasLock = new();

		public static ObservableCollection<Tarea> TareasPersonales = new();
		public static readonly object TareasPersonalesLock = new();

		public static async void Procesar_ResultadoGenerico(Comando_ResultadoGenerico Comando, Action FuncionCuandoCorrecto = null, Action FuncionCuandoErroneo = null)
		{
			if(Comando.Correcto)
			{
				FuncionCuandoCorrecto?.Invoke();
			}
			else
			{
				FuncionCuandoErroneo?.Invoke();

				await UserDialogs.Instance.AlertAsync(Comando.Mensaje, "Error", "Aceptar");
			}
		}

		public static async Task<string> PedirAlUsuarioStringCorrecto(string Titulo, int NumeroCaracteresMaximo, bool PermitirEspacios)
		{
			string stringCorrecto = "";
			bool esCorrecto = false;

			while(!esCorrecto)
			{
				var configuracionPrompt = new PromptConfig
				{
					InputType = InputType.Name,
					IsCancellable = true,
					Message = Titulo,
					Text=stringCorrecto,
					MaxLength = NumeroCaracteresMaximo
				};

				var resultado = await UserDialogs.Instance.PromptAsync(configuracionPrompt);

				if(!resultado.Ok) return null;

				stringCorrecto = resultado.Text;

				if(stringCorrecto == "") {
					await UserDialogs.Instance.AlertAsync("No puede estar vacío", "Alerta", "Aceptar"); continue; }

				string cp = Comun.Global.CARACTERES_PERMITIDOS_LOGIN;

				if(!stringCorrecto.All(PermitirEspacios ? (cp+" ").Contains : cp.Contains)) {
					await UserDialogs.Instance.AlertAsync($"Solo se pueden usar letras y números", "Alerta", "Aceptar"); continue; }

				return stringCorrecto;
			}

			return null;
		}

		public static Button GenerarBotonMesa(byte GridX, byte GridY, EventHandler EventoMesaPulsada)
		{
			var buttonMesa = new Button()
			{
				         FontAttributes = FontAttributes.Bold,
							   FontSize = 20,
						     	Padding = new(0),
							     Margin = new(0),
						BackgroundColor = Color.FromRgb(240, 240, 240),
						 BindingContext = $"{GridX}.{GridY}"
			};
			buttonMesa.Clicked += EventoMesaPulsada;

			return buttonMesa;
		}

		public static Frame GenerarFrameLabelMesa(byte GridX, byte GridY)
		{
			var labelMesa = new Label()
			{
				         FontAttributes = FontAttributes.Bold,
							   FontSize = 20,
				HorizontalTextAlignment = TextAlignment.Center,
				  VerticalTextAlignment = TextAlignment.Center,
						     	Padding = new(0),
							     Margin = new(0),
						BackgroundColor = Color.FromRgb(240, 240, 240),
						 BindingContext = $"{GridX}.{GridY}",
						VerticalOptions = LayoutOptions.FillAndExpand,
					  HorizontalOptions = LayoutOptions.FillAndExpand
			};

			return new Frame()
			{
				     Content = labelMesa,
				CornerRadius = 2f,
				     Padding = new(0),
					  Margin = new(0),
				   HasShadow = false
			};
		}

		public static async Task Get_Ajustes()
		{
			UserDialogs.Instance.ShowLoading("Pidiendo ajustes...");

			var comandoRespuesta = await Task.Run(() =>
			{
				string respuestaGestor = new Comando_PedirAjustes().Enviar(IPGestor);
				return Comando.DeJson<Comando_MandarAjustes>(respuestaGestor);
			});
			
			Ajustes = comandoRespuesta.Ajustes;

			UserDialogs.Instance.HideLoading();
		}

		public static async Task Get_Mesas()
		{
			UserDialogs.Instance.ShowLoading("Pidiendo mesas...");

			var comandoRespuesta = await Task.Run(() =>
			{
				string respuestaGestor = new Comando_PedirMesas().Enviar(IPGestor);
				return Comando.DeJson<Comando_MandarMesas>(respuestaGestor);
			});

			AnchoMapaMesas = comandoRespuesta.AnchoMapa;
			AltoMapaMesas = comandoRespuesta.AltoMapa;
			Mesas = comandoRespuesta.Mesas;

			UserDialogs.Instance.HideLoading();
		}

		public static async Task Get_Articulos()
		{
			UserDialogs.Instance.ShowLoading("Pidiendo artículos...");

			var comandoRespuesta = await Task.Run(() =>
			{
				string respuestaGestor = new Comando_PedirArticulos().Enviar(IPGestor);
				return Comando.DeJson<Comando_MandarArticulos>(respuestaGestor);
			});
			
			lock(CategoriasLock)
			{
				Categorias.Clear();

				var categorias =
					comandoRespuesta.Articulos
						.OrderBy(a => a.Categoria)
						.GroupBy(a => a.Categoria)
						.Select(a => a.First().Categoria)
						.ToArray();

				foreach(var categoria in categorias)
				{
					var nuevaCategoria = new GrupoArticuloCategoria(categoria);

					nuevaCategoria.AddRange(
						comandoRespuesta.Articulos
							.Where(a => a.Categoria == categoria)
							.OrderBy(a => a.Nombre));

					Categorias.Add(nuevaCategoria);
				}
			}

			UserDialogs.Instance.HideLoading();
		}

		public static async Task Get_TareasPersonales()
		{
			UserDialogs.Instance.ShowLoading("Actualizando lista de tareas...");

			var comandoRespuesta = await Task.Run(() =>
			{
				string respuestaGestor = new Comando_PedirTareas(UsuarioActual.NombreUsuario).Enviar(IPGestor);
				return Comando.DeJson<Comando_MandarTareas>(respuestaGestor);
			});

			lock(TareasPersonalesLock)
			{
				TareasPersonales.Clear();
			
				foreach(var tarea in comandoRespuesta.Tareas)
					TareasPersonales.Add(tarea);

				TareasPersonales.Ordenar();
			}

			UserDialogs.Instance.HideLoading();
		}
	}
}
