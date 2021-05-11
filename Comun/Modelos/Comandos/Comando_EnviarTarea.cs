
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

	public class Comando_EnviarTarea : Comando
	{
		private const TiposComando TipoComandoInit = TiposComando.EnviarTarea;

		[JsonProperty("1")] public Tarea Tarea { get; private set; }

		private void InicializarPropiedades(Tarea Tarea)
		{
			this.Tarea = Tarea;
		}

		public Comando_EnviarTarea(Tarea Tarea)
			: base(TipoComandoInit)
		{
			InicializarPropiedades(Tarea);
		}

		[JsonConstructor]
		#pragma warning disable IDE0051
		private Comando_EnviarTarea(TiposComando TipoComandoJson,  Tarea Tarea)
		#pragma warning restore IDE0051
			: base(TipoComandoJson)
		{
			InicializarPropiedades(Tarea);
		}
	}
}
