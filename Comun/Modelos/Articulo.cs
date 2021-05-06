
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace PFG.Comun
{
	public class Articulo
	{
		[JsonProperty("1")] public string Nombre { get; set; }

		[JsonProperty("2")] public Categoria Categoria { get; set; }

		[JsonProperty("3")] public float Precio { get; set; }

		public Articulo(string Nombre, Categoria Categoria, float Precio)
		{
			this.Nombre = Nombre;

			this.Categoria = Categoria;

			this.Precio = Precio;
		}
	}
}
