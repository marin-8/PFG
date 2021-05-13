
using System;
using System.Text;
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
		public static ProcesadorAplicacion ProcesadorMensajesRecibidos;

		public static ObservableCollection<Usuario> Usuarios = new();
		public static readonly object UsuariosLock = new();

		public static byte AnchoMapaMesas { get; private set; }
		public static byte AltoMapaMesas { get; private set; }
		public static Mesa[] Mesas { get; private set; } = new Mesa[0];


		public static ObservableCollection<List<Articulo>> Categorias = new();
		public static readonly object CategoriasLock = new();

		public static ObservableCollection<Tarea> TareasPersonales = new();
		public static readonly object TareasPersonalesLock = new();

		public static async void Procesar_ResultadoGenerico(Comando_ResultadoGenerico Comando, Action FuncionCuandoCorrecto, Action FuncionCuandoErroneo=null)
		{
			if(Comando.Correcto)
			{
				FuncionCuandoCorrecto();
			}
			else
			{
				FuncionCuandoErroneo?.Invoke();

				await UserDialogs.Instance.AlertAsync(Comando.Mensaje, "Error", "Aceptar");
			}
		}

		public static async Task<string> PedirAlUsuarioStringCorrecto(string Titulo, int NumeroCaracteresMaximo, bool PermitirEspacios)
		{
			string stringCorrecto = null;

			while(stringCorrecto == null)
			{
				var configuracionPrompt = new PromptConfig
				{
					InputType = InputType.Name,
					IsCancellable = true,
					Message = Titulo,
					MaxLength = NumeroCaracteresMaximo
				};

				var resultado = await UserDialogs.Instance.PromptAsync(configuracionPrompt);

				if(!resultado.Ok) return null;

				stringCorrecto = resultado.Text;

				if(stringCorrecto.Equals("")) {
					await UserDialogs.Instance.AlertAsync("No puede estar vacío", "Alerta", "Aceptar"); stringCorrecto = null; continue; }

				string cp = Comun.Global.CARACTERES_PERMITIDOS_LOGIN;

				if(!stringCorrecto.All(PermitirEspacios ? (cp+" ").Contains : cp.Contains)) {
					await UserDialogs.Instance.AlertAsync($"Solo se pueden usar letras y números", "Alerta", "Aceptar"); stringCorrecto = null; continue; }

				return stringCorrecto;
			}

			return null;
		}

		public static Button GenerarBotonMesa(byte GridX, byte GridY, EventHandler EventoMesaPulsada)
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
			buttonMesa.Clicked += EventoMesaPulsada;

			return buttonMesa;
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
							.Where(a => a.Categoria.Equals(categoria))
							.OrderBy(a => a.Nombre));

					Categorias.Add(nuevaCategoria);
				}
			}

			UserDialogs.Instance.HideLoading();
		}
	}
}
