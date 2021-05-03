
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

	public class Comando_ResultadoIntentoCrearMesa : Comando
	{
		private const TiposComando TipoComandoInit = TiposComando.ResultadoIntentoCrearMesa;

		[JsonProperty("1")] public ResultadosIntentoCrearMesa ResultadoIntentoCrearMesa { get; private set; }

		private void InicializarPropiedades(ResultadosIntentoCrearMesa ResultadoIntentoCrearMesa)
		{
			this.ResultadoIntentoCrearMesa = ResultadoIntentoCrearMesa;
		}

		public Comando_ResultadoIntentoCrearMesa(ResultadosIntentoCrearMesa ResultadoIntentoCrearMesa)
			: base(TipoComandoInit)
		{
			InicializarPropiedades(ResultadoIntentoCrearMesa);
		}

		[JsonConstructor]
		private Comando_ResultadoIntentoCrearMesa(TiposComando TipoComandoJson, ResultadosIntentoCrearMesa ResultadoIntentoCrearMesa)
			: base(TipoComandoJson)
		{
			InicializarPropiedades(ResultadoIntentoCrearMesa);
		}
	}
}
