
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace PFG.Comun
{
	public class Mesa
	{
		[JsonProperty("1")] public string Nombre { get; set; }

		[JsonProperty("2")] public byte GridX { get; set; }
		[JsonProperty("3")] public byte GridY { get; set; }

		[JsonIgnore] public EstadosMesa EstadoMesa { get; set; }

		public Mesa(string Nombre, byte GridX, byte GridY)
		{
			this.Nombre = Nombre;

			this.GridX = GridX;
			this.GridY = GridY;

			EstadoMesa = EstadosMesa.Vacia;
		}
	}
}
