
using System;
using System.Diagnostics;

using PFG.Comun;

namespace PFG.Pruebas
{
	class Program
	{
		static void Main()
		{
			Debug.WriteLine(Comando_IniciarSesion.ParametrosToString("us,r", "pwd"));
		}
	}
}
