
using System;
using System.Collections.Generic;

namespace PFG.Comun
{
	public enum ResultadosIntentoEditarMapaMesas : byte
	{
		Correcto = 1,
		MinimoColumnas = 2,
		MaximoColumnas = 3,
		MinimoFilas = 4,
		MaximoFilas = 5
	}
}
