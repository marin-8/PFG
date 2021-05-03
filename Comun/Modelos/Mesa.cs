
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace PFG.Comun
{
	public class Mesa
	{
		[JsonProperty("1")] public byte Numero { get; set; }

		[JsonProperty("2")] public byte GridX { get; set; }
		[JsonProperty("3")] public byte GridY { get; set; }

		[JsonProperty("4")] public EstadosMesa EstadoMesa { get; set; }

		public Mesa(byte Numero, byte GridX, byte GridY, EstadosMesa EstadoMesa = EstadosMesa.Vacia)
		{
			this.Numero = Numero;

			this.GridX = GridX;
			this.GridY = GridY;

			this.EstadoMesa = EstadoMesa;
		}
	}
}
