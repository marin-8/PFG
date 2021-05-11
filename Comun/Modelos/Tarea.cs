
using System;

namespace PFG.Comun
{
	public class Tarea
	{
		public int ID { get; private set; }

		public bool Completada { get; private set; }

		public DateTime FechaHoraCreacion { get; private set; }

		public TiposTareas TipoTarea { get; private set; }
		public string NombreUsuario { get; private set; }

		public Articulo[] Articulos { get; private set; }
		public byte NumeroMesa { get; private set; }

		public Tarea(int ID, DateTime FechaHoraCreacion, TiposTareas TipoTarea, string NombreUsuario, Articulo[] Articulos = null, byte NumeroMesa = 0)
		{
			this.ID = ID;

			Completada = false;

			this.FechaHoraCreacion = FechaHoraCreacion;

			this.TipoTarea = TipoTarea;
			this.NombreUsuario = NombreUsuario;

			this.Articulos = Articulos;
			this.NumeroMesa = NumeroMesa;
		}

		public void CompletadarTarea()
		{
			Completada = true;
		}
	}
}
