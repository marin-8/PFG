
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace PFG.Comun
{
	public static class Extensiones
	{
		public static string  Enviar(this Comando comando, string IP)
		{
			return ControladorRed.Enviar(IP, comando.ToString());
		}

		public static void Ordenar<T>(this ObservableCollection<T> coleccion, Comparison<T> comparacion)
		{
			var listaOrdenable = new List<T>(coleccion);
			listaOrdenable.Sort(comparacion);

			for(int i = 0 ; i < listaOrdenable.Count; i++)
				coleccion.Move(coleccion.IndexOf(listaOrdenable[i]), i);
		}
	}
}
