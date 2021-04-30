
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

	public class Comando_CerrarSesion : Comando
	{
		private const TiposComando TipoComandoInit = TiposComando.CerrarSesion;

		[JsonProperty("1")] public string Usuario { get; private set; }

		private void InicializarPropiedades(string Usuario)
		{
			this.Usuario = Usuario;
		}

		public Comando_CerrarSesion(string Usuario)
			: base(TipoComandoInit)
		{
			InicializarPropiedades(Usuario);
		}

		[JsonConstructor]
		private Comando_CerrarSesion(TiposComando TipoComandoJson, string Usuario)
			: base(TipoComandoJson)
		{
			InicializarPropiedades(Usuario);
		}
	}
}
