
namespace PFG.Comun
{
	public class Comando_CerrarSesion : Comando
	{
		public string Usuario { get; private set; }

		public Comando_CerrarSesion(string Usuario, bool DIF)
			: base(TiposComando.CerrarSesion)
		{
			this.Usuario = Usuario;
		}

		public Comando_CerrarSesion(string ComandoString)
			: base(ComandoString) { }
	}
}
