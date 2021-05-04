
namespace PFG.Comun
{
	public enum TiposComando : byte
	{
		ResultadoGenerico = 0,

		IniciarSesion = 1,
		ResultadoIniciarSesion = 2,
		CerrarSesion = 3,

		PedirUsuarios = 4,
		MandarUsuarios = 5,
		CrearUsuario = 6,
		ModificarUsuarioNombre = 7,
		ModificarUsuarioNombreUsuario = 8,
		ModificarUsuarioContrasena = 9,
		ModificarUsuarioRol = 10,
		EliminarUsuario = 11,

		PedirMesas = 12,
		MandarMesas = 13,
		EditarMapaMesas = 14,
		CrearMesa = 15,
		ModificarMesaNumero = 16,
		ModificarMesaSitio = 17,
		EliminarMesa = 18,
	}
}
