
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
	public static class GestionArticulos
	{
		const string RUTA_ARCHIVO_JSON_ARTICULOS = @".\Guardado\Articulos.json";

		public static List<Articulo> Articulos { get; private set; } = new();

		public static void Cargar()
		{
			string articulosJsonString = File.ReadAllText(RUTA_ARCHIVO_JSON_ARTICULOS);
			Articulos = JsonConvert.DeserializeObject<List<Articulo>>(articulosJsonString);
		}

		public static void Guardar()
		{
			using StreamWriter archivo = File.CreateText(RUTA_ARCHIVO_JSON_ARTICULOS);
			new JsonSerializer().Serialize(archivo, Articulos);
		}
	}
}
