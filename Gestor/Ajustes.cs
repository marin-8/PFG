
using System.IO;
using System.Linq;
using System.Reflection;

using Newtonsoft.Json;

namespace PFG.Gestor
{
	public static class Ajustes
	{
		const string RUTA_ARCHIVO_JSON = @".\Guardado\Ajustes.json";

		public static bool ComenzarJornadaConArticulosDisponibles { get; set; }

		public static void Cargar()
		{
			string usuariosJsonString = File.ReadAllText(RUTA_ARCHIVO_JSON);
			var ajustesCarga  = JsonConvert.DeserializeObject<AjustesCargaGuardado>(usuariosJsonString);

			MapearObjetoAClaseEstatica(ajustesCarga);
		}

		public static void Guardar()
		{
			var ajustesGuardado = MapearClaseEstaticaAObjeto();

			using StreamWriter archivo = File.CreateText(RUTA_ARCHIVO_JSON);
			new JsonSerializer().Serialize(archivo, ajustesGuardado);
		}

		private class AjustesCargaGuardado
		{
			[JsonProperty("1")] public static bool ComenzarJornadaConArticulosDisponibles { get; set; }
		}

		private static void MapearObjetoAClaseEstatica(AjustesCargaGuardado AjustesCarga)
		{
			var propiedadesOrigen = AjustesCarga.GetType().GetProperties();

			var propiedadesDestino =
				typeof(Ajustes)
					.GetProperties(BindingFlags.Public | BindingFlags.Static);

			foreach(var propiedadOrigen in propiedadesOrigen)
			{
				var propiedadDestino =
					propiedadesDestino
						.Single(p => p.Name == propiedadOrigen.Name);

				propiedadDestino.SetValue(null, propiedadOrigen.GetValue(AjustesCarga));
			}
		}

		private static AjustesCargaGuardado MapearClaseEstaticaAObjeto()
		{
			AjustesCargaGuardado ajustesGuardado = new();

			var propiedadesOrigen =
				typeof(Ajustes)
					.GetProperties(BindingFlags.Public | BindingFlags.Static);

			var propiedadesDestino = ajustesGuardado.GetType().GetProperties();

			foreach(var propiedadOrigen in propiedadesOrigen)
			{
				var propiedadDestino =
					propiedadesDestino
						.Single(p => p.Name == propiedadOrigen.Name);

				var valor = propiedadOrigen.GetValue(null, null);

				propiedadDestino.SetValue(ajustesGuardado, valor);
			}

			return ajustesGuardado;
		}
	}
}
