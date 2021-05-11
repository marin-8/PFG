
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

using PFG.Comun;

namespace PFG.Gestor
{
	public static class GestionTareas
	{
		public static List<Tarea> Tareas { get; private set; } = new();
	}
}
