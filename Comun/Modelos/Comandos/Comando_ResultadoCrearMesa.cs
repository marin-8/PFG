
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

	public class Comando_ResultadoCrearMesa : Comando
	{
		private const TiposComando TipoComandoInit = TiposComando.ResultadoCrearMesa;

		[JsonProperty("1")] public ResultadosCrearMesa ResultadoCrearMesa { get; private set; }

		private void InicializarPropiedades(ResultadosCrearMesa ResultadoCrearMesa)
		{
			this.ResultadoCrearMesa = ResultadoCrearMesa;
		}

		public Comando_ResultadoCrearMesa(ResultadosCrearMesa ResultadoCrearMesa)
			: base(TipoComandoInit)
		{
			InicializarPropiedades(ResultadoCrearMesa);
		}

		[JsonConstructor]
		private Comando_ResultadoCrearMesa(TiposComando TipoComandoJson, ResultadosCrearMesa ResultadoCrearMesa)
			: base(TipoComandoJson)
		{
			InicializarPropiedades(ResultadoCrearMesa);
		}
	}
}
