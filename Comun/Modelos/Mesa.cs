
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace PFG.Comun
{
	public class Mesa
	{
		[JsonProperty("1")] public byte Numero { get; set; }

		[JsonProperty("2")] public byte SitioX { get; set; }
		[JsonProperty("3")] public byte SitioY { get; set; }

		[JsonProperty("4")] public EstadosMesa EstadoMesa { get; set; }

		public Mesa(byte Numero, byte SitioX, byte SitioY, EstadosMesa EstadoMesa = EstadosMesa.Vacia)
		{
			this.Numero = Numero;

			this.SitioX = SitioX;
			this.SitioY = SitioY;

			this.EstadoMesa = EstadoMesa;
		}
	}
}
