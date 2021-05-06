
namespace PFG.Comun
{
	public enum TiposComando : byte
	{
		ResultadoGenerico = 1,

		IniciarSesion = 2,
		ResultadoIniciarSesion = 3,
		CerrarSesion = 4,

		PedirUsuarios = 5,
		MandarUsuarios = 6,
		CrearUsuario = 7,
		ModificarUsuarioNombre = 8,
		ModificarUsuarioNombreUsuario = 9,
		ModificarUsuarioContrasena = 10,
		ModificarUsuarioRol = 11,
		EliminarUsuario = 12,

		PedirMesas = 13,
		MandarMesas = 14,
		EditarMapaMesas = 15,
		CrearMesa = 16,
		ModificarMesaNumero = 17,
		ModificarMesaSitio = 18,
		EliminarMesa = 19,

		PedirArticulos = 20,
		MandarArticulos = 21,
		CrearArticulo = 22,
		ModificarArticuloNombre = 23,
		ModificarArticuloPrecio = 24,
		ModificarArticuloCategoria = 25,
		EliminarArticulo = 26,
	}
}
