
using System;
using System.Collections.Generic;

namespace PFG.Comun
{
	public enum ResultadosIntentoIniciarSesion : byte
	{
		Correcto = 1,
		UsuarioNoExiste = 2,
		ContrasenaIncorrecta = 3,
		UsuarioYaConectado = 4
	}
}
