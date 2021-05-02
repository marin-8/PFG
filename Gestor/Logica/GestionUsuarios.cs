
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
	public static class GestionUsuarios
	{
		const string RUTA_ARCHIVO_JSON = @".\Archivos\Usuarios.json";

		public static List<Usuario> Usuarios { get; private set; } = new();

		public static void Cargar()
		{
			string usuariosJsonString = File.ReadAllText(RUTA_ARCHIVO_JSON);
			Usuarios = JsonConvert.DeserializeObject<List<Usuario>>(usuariosJsonString);

			foreach(var usuario in Usuarios)
				usuario.IP = "";
		}

		public static void Guardar()
		{
			using StreamWriter archivo = File.CreateText(RUTA_ARCHIVO_JSON);
			new JsonSerializer().Serialize(archivo, Usuarios);
		}
	}
}
