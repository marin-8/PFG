
using System;
using System.Text;
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

		public static List<Articulo> ArticulosLocal = new();
		public static List<Categoria> CategoriasLocal = new();

		public static void Procesar_ResultadoGenerico(Comando_ResultadoGenerico Comando, Action FuncionCuandoCorrecto, Action FuncionCuandoErroneo=null)
		{
			if(Comando.Correcto)
			{
				FuncionCuandoCorrecto();
			}
			else
			{
				FuncionCuandoErroneo?.Invoke();

				UserDialogs.Instance.Alert(Comando.Mensaje, "Error", "Aceptar");
			}
		}
	}
}
