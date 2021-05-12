
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

		[JsonProperty("4")] public SitioPreparacionArticulo SitioPreparacionArticulo { get; set; }

		[JsonProperty("5")] public byte Unidades { get; set; }
		[JsonProperty("6")] public bool Disponible { get; set; }

		public Articulo(string Nombre, string Categoria, float Precio, byte Unidades=0, bool Disponible=true)
		{
			this.Nombre = Nombre;

			this.Categoria = Categoria;

			this.Precio = Precio;

			this.Unidades = Unidades;
			this.Disponible = Disponible;
		}
	}
}
