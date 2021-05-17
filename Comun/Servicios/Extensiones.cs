
using System.Linq;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace PFG.Comun
{
	public static class Extensiones
	{
		public static string Enviar(this Comando comando, string IP, bool LimitarIntentos = false)
		{
			return ControladorRed.Enviar(IP, comando.ToString(), LimitarIntentos);
		}

		public static bool EsIPValida(this string str)
		{
			return Regex.IsMatch
			(
				str, 
				@"\b(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\b"
			);
		}

		public static void Ordenar(this ObservableCollection<Articulo> coleccion)
		{
			Articulo[] listaOrdenable =
				coleccion
					.OrderBy(t => t.Categoria)
					.ThenBy(t => t.Nombre)
					.ToArray();

			for(int i = 0 ; i < listaOrdenable.Length; i++)
				coleccion.Move(coleccion.IndexOf(listaOrdenable[i]), i);
		}

		public static void Ordenar(this ObservableCollection<Tarea> coleccion)
		{
			Tarea[] listaOrdenable =
				coleccion
					.OrderBy(t => t.FechaHoraCreacion)
					.ThenBy(t => t.TipoTarea)
					.ToArray();

			for(int i = 0 ; i < listaOrdenable.Length; i++)
				coleccion.Move(coleccion.IndexOf(listaOrdenable[i]), i);
		}
	}
}
