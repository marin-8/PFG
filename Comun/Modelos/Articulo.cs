
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace PFG.Comun
{
	public class Articulo
	{
		[JsonProperty("1")] public string Nombre { get; set; }

		[JsonProperty("2")] public string Categoria { get; set; }

		[JsonProperty("3")] public float Precio { get; set; }

		public Articulo(string Nombre, string Categoria, float Precio)
		{
			this.Nombre = Nombre;

			this.Categoria = Categoria;

			this.Precio = Precio;
		}
	}
}
