
namespace PFG.Comun
{
	public enum TiposComando : byte
	{
		IniciarSesion = 1,
		ResultadoIniciarSesion = 2,

		CerrarSesion = 3,

		PedirUsuarios = 4,
		MandarUsuarios = 5,

		CrearUsuario = 6,
		ResultadoCrearUsuario = 7,

		ModificarUsuarioNombre = 8,
		ModificarUsuarioNombreUsuario = 9,
		ModificarUsuarioContrasena = 10,
		ModificarUsuarioRol = 11,

		EliminarUsuario = 12,
		ResultadoEliminarUsuario = 13,

		PedirMesas = 14,
		MandarMesas = 15,

		EditarMapaMesas = 16,
		ResultadoEditarMapaMesas = 17,

		CrearMesa = 18,
		ResultadoCrearMesa = 19,

		ModificarMesaNumero = 20,
		ModificarMesaSitio = 21,

		EliminarMesa = 22,
		ResultadoEliminarMesa = 23,
	}
}
