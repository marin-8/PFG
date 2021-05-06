
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace PFG.Comun
{
	public class Categoria
	{
		[JsonProperty("1")] public string Nombre { get; set; }

		[JsonProperty("2")] public List<Articulo> Articulos { get; set; }

		public Categoria(string Nombre, List<Articulo> Articulos)
		{
			this.Nombre = Nombre;

			this.Articulos = Articulos;
		}
	}
}
