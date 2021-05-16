
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;

using Newtonsoft.Json;

using PFG.Comun;

namespace PFG.Gestor
{
	public static class GestionArticulos
	{
		const string RUTA_ARCHIVO_JSON_ARTICULOS = @".\Guardado\Articulos.json";

		private static readonly object GuardadoLock = new();

		public static List<Articulo> Articulos { get; private set; } = new();

		public static void Cargar()
		{
			string articulosJsonString = File.ReadAllText(RUTA_ARCHIVO_JSON_ARTICULOS);
			Articulos = JsonConvert.DeserializeObject<List<Articulo>>(articulosJsonString);
		}

		public static void Guardar()
		{
			Task.Run(() =>
			{
				lock(GuardadoLock)
				{
					using StreamWriter archivo = File.CreateText(RUTA_ARCHIVO_JSON_ARTICULOS);
					new JsonSerializer().Serialize(archivo, Articulos);
				}
			});
		}
	}
}
