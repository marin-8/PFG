
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using PFG.Comun;

namespace PFG.Gestor
{
	public static class Ajustes
	{
		public static bool ComenzarJornadaConArticulosDisponibles { get; set; }

		public static void Cargar()
		{

		}

		public static void Guardar()
		{

		}

		private class AjustesCargaGuardado
		{
			public static bool ComenzarJornadaConArticulosDisponibles { get; set; }
		}

		private static void MapearObjetoAClaseEstatica(AjustesCargaGuardado Origen)
		{
			var propiedadesOrigen = Origen.GetType().GetProperties();

			var propiedadesDestino =
				typeof(Ajustes)
					.GetProperties(BindingFlags.Public | BindingFlags.Static);

			foreach(var propiedadOrigen in propiedadesOrigen)
			{
				var propiedadDestino =
					propiedadesDestino
						.Single(p => p.Name == propiedadOrigen.Name);

				propiedadDestino.SetValue(null, propiedadOrigen.GetValue(Origen));
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
