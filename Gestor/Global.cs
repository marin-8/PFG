
using System;
using System.Collections.Generic;
using System.Text;

using PFG.Comun;

namespace PFG.Gestor
{
	public static class Global
	{
		private static int _nuevoIDTarea = 1;
		public static int NuevoIDTarea
		{
			get => _nuevoIDTarea++;
			private set { _nuevoIDTarea = value; }
		}
	}
}
