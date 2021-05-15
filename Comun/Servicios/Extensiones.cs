
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace PFG.Comun
{
	public static class Extensiones
	{
		public static string  Enviar(this Comando comando, string IP)
		{
			return ControladorRed.Enviar(IP, comando.ToString());
		}

		public static bool EsIPValida(this string str)
		{
			return Regex.IsMatch
			(
				str, 
				@"\b(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\b"
			);
		}

		public static void Ordenar<T>(this ObservableCollection<T> coleccion, Comparison<T> comparacion)
		{
			var listaOrdenable = new List<T>(coleccion);
			listaOrdenable.Sort(comparacion);

			for(int i = 0 ; i < listaOrdenable.Count; i++)
				coleccion.Move(coleccion.IndexOf(listaOrdenable[i]), i);
		}

		public static void Ordenar(this ObservableCollection<Tarea> coleccion)
		{
			Tarea[] listaOrdenable =
				coleccion
					.OrderBy(t => t.TipoTarea)
					.ThenBy(t => t.FechaHoraCreacion)
					.ToArray();

			for(int i = 0 ; i < listaOrdenable.Length; i++)
				coleccion.Move(coleccion.IndexOf(listaOrdenable[i]), i);
		}
	}
}
