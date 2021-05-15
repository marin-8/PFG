
using Newtonsoft.Json;
using System;

namespace PFG.Comun
{
	public class Tarea
	{
		[JsonProperty("1")] public uint ID { get; private set; }

		[JsonProperty("2")] public bool Completada { get; private set; }

		[JsonProperty("3")] public DateTime FechaHoraCreacion { get; private set; }

		[JsonProperty("4")] public TiposTareas TipoTarea { get; private set; }
		[JsonProperty("5")] public string NombreUsuario { get; private set; }

		[JsonProperty("6")] public Articulo[] Articulos { get; private set; }
		[JsonProperty("7")] public byte NumeroMesa { get; private set; }

		public Tarea(uint ID, DateTime FechaHoraCreacion, TiposTareas TipoTarea, string NombreUsuario, Articulo[] Articulos = null, byte NumeroMesa = 0)
		{
			this.ID = ID;

			Completada = false;

			this.FechaHoraCreacion = FechaHoraCreacion;

			this.TipoTarea = TipoTarea;
			this.NombreUsuario = NombreUsuario;

			this.Articulos = Articulos;
			this.NumeroMesa = NumeroMesa;
		}

		public void CompletarTarea()
		{
			Completada = true;
		}

		public void Reasignar(string NuevoNombreUsuario)
		{
			NombreUsuario = NuevoNombreUsuario;
		}
	}
}
