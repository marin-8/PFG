﻿
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

	public class Comando_IntentarCrearMesa : Comando
	{
		private const TiposComando TipoComandoInit = TiposComando.IntentarCrearMesa;

		[JsonProperty("1")] public Mesa NuevaMesa { get; private set; }

		private void InicializarPropiedades(Mesa NuevaMesa)
		{
			this.NuevaMesa = NuevaMesa;
		}

		public Comando_IntentarCrearMesa(Mesa NuevaMesa)
			: base(TipoComandoInit)
		{
			InicializarPropiedades(NuevaMesa);
		}

		[JsonConstructor]
		private Comando_IntentarCrearMesa(TiposComando TipoComandoJson,  Mesa NuevaMesa)
			: base(TipoComandoJson)
		{
			InicializarPropiedades(NuevaMesa);
		}
	}
}
