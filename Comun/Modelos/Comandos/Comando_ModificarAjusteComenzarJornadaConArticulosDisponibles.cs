
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

	public class Comando_ModificarAjusteComenzarJornadaConArticulosDisponibles : Comando
	{
		private const TiposComando TipoComandoInit = TiposComando.ModificarAjusteComenzarJornadaConArticulosDisponibles;

		[JsonProperty("1")] public bool EstaActivado { get; private set; }

		private void InicializarPropiedades(bool EstaActivado)
		{
			this.EstaActivado = EstaActivado;
		}

		public Comando_ModificarAjusteComenzarJornadaConArticulosDisponibles(bool EstaActivado)
			: base(TipoComandoInit)
		{
			InicializarPropiedades(EstaActivado);
		}

		[JsonConstructor]
		#pragma warning disable IDE0051
		private Comando_ModificarAjusteComenzarJornadaConArticulosDisponibles(TiposComando TipoComandoJson, bool EstaActivado)
		#pragma warning restore IDE0051
			: base(TipoComandoJson)
		{
			InicializarPropiedades(EstaActivado);
		}
	}
}
