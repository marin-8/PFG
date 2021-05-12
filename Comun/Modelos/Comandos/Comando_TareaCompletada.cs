
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

	public class Comando_TareaCompletada : Comando
	{
		private const TiposComando TipoComandoInit = TiposComando.TareaCompletada;

		[JsonProperty("1")] public int ID { get; private set; }

		private void InicializarPropiedades(int ID)
		{
			this.ID = ID;
		}

		public Comando_TareaCompletada(int ID)
			: base(TipoComandoInit)
		{
			InicializarPropiedades(ID);
		}

		[JsonConstructor]
		#pragma warning disable IDE0051
		private Comando_TareaCompletada(TiposComando TipoComandoJson, int ID)
		#pragma warning restore IDE0051
			: base(TipoComandoJson)
		{
			InicializarPropiedades(ID);
		}
	}
}
