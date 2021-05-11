
using Newtonsoft.Json;

namespace PFG.Comun
{
/*
	COSAS QUE HACER AL CREAR UN NUEVO COMANDO:

	- Renombrar Clase (CTRL+H)
	- Cambiar TipoComandoInit
	- Poner Propiedades
	- Enumerar JsonProperty's
	- Método InicializarPropiedades
	- Constructor public
	- Constructor private
*/

	public class Comando_MandarTareas : Comando
	{
		private const TiposComando TipoComandoInit = TiposComando.MandarTareas;

		[JsonProperty("1")] public Tarea[] Tareas { get; private set; }

		private void InicializarPropiedades(Tarea[] Tareas)
		{
			this.Tareas = Tareas;
		}

		public Comando_MandarTareas(Tarea[] Tareas)
			: base(TipoComandoInit)
		{
			InicializarPropiedades(Tareas);
		}

		[JsonConstructor]
		#pragma warning disable IDE0051
		private Comando_MandarTareas(TiposComando TipoComandoJson, Tarea[] Tareas)
		#pragma warning restore IDE0051
			: base(TipoComandoJson)
		{
			InicializarPropiedades(Tareas);
		}
	}
}
