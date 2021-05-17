
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

	public class Comando_MandarAjustes : Comando
	{
		private const TiposComando TipoComandoInit = TiposComando.MandarAjustes;

		[JsonProperty("1")] public AjustesObjeto Ajustes { get; private set; }

		private void InicializarPropiedades(AjustesObjeto Ajustes)
		{
			this.Ajustes = Ajustes;
		}

		public Comando_MandarAjustes(AjustesObjeto Ajustes)
			: base(TipoComandoInit)
		{
			InicializarPropiedades(Ajustes);
		}

		[JsonConstructor]
		#pragma warning disable IDE0051
		private Comando_MandarAjustes(TiposComando TipoComandoJson, AjustesObjeto Ajustes)
		#pragma warning restore IDE0051
			: base(TipoComandoJson)
		{
			InicializarPropiedades(Ajustes);
		}
	}
}
