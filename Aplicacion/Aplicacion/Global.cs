
using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;

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

		public static ObservableCollection<Usuario> UsuariosLocal = new();

		public static byte ColumnasMesas;
		public static byte FilasMesas;
		public static List<Mesa> MesasLocal = new();

		public static ObservableCollection<List<Articulo>> CategoriasLocal = new();

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

		public static async Task<string> PedirAlUsuarioStringCorrecto(string Titulo, bool PermitirEspacios)
		{
			string stringCorrecto = null;

			while(stringCorrecto == null)
			{
				var configuracionPrompt = new PromptConfig
				{
					InputType = InputType.Name,
					IsCancellable = true,
					Message = Titulo,
					MaxLength = Comun.Global.MAX_CARACTERES_LOGIN
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
	}
}
