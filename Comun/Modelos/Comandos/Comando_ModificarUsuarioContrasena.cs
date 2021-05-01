
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

	public class Comando_ModificarUsuarioContrasena : Comando
	{
		private const TiposComando TipoComandoInit = TiposComando.ModificarUsuarioContrasena;

		[JsonProperty("1")] public string Usuario { get; private set; }
		[JsonProperty("2")] public string NuevaContrasena { get; private set; }

		private void InicializarPropiedades(string Usuario, string NuevaContrasena)
		{
			this.Usuario = Usuario;
			this.NuevaContrasena = NuevaContrasena;
		}

		public Comando_ModificarUsuarioContrasena(string Usuario, string NuevaContrasena)
			: base(TipoComandoInit)
		{
			InicializarPropiedades(Usuario, NuevaContrasena);
		}

		[JsonConstructor]
		private Comando_ModificarUsuarioContrasena(TiposComando TipoComandoJson, string Usuario, string NuevaContrasena)
			: base(TipoComandoJson)
		{
			InicializarPropiedades(Usuario, NuevaContrasena);
		}
	}
}
	