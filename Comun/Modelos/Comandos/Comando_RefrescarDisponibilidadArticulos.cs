
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

	public class Comando_RefrescarDisponibilidadArticulos : Comando
	{
		private const TiposComando TipoComandoInit = TiposComando.RefrescarDisponibilidadArticulos;

		//

		//private void InicializarPropiedades()
		//{
			
		//}

		public Comando_RefrescarDisponibilidadArticulos()
			: base(TipoComandoInit)
		{
			//InicializarPropiedades(Mesas);
		}

		[JsonConstructor]
		#pragma warning disable IDE0051
		private Comando_RefrescarDisponibilidadArticulos(TiposComando TipoComandoJson)
		#pragma warning restore IDE0051
			: base(TipoComandoJson)
		{
			//InicializarPropiedades(Mesas);
		}
	}
}
