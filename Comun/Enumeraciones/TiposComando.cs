
namespace PFG.Comun
{
	public enum TiposComando : byte
	{
		IntentarIniciarSesion = 1,
		ResultadoIntentoIniciarSesion = 2,
		CerrarSesion = 3,
		PedirUsuarios = 4,
		MandarUsuarios = 5,
		IntentarCrearUsuario = 6,
		ResultadoIntentoCrearUsuario = 7
	}
}
