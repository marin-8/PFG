
using System;
using System.Diagnostics;

using PFG.Comun;

namespace PFG.Pruebas
{
	class Program
	{
		static void Main(string[] args)
		{
			Debug.WriteLine(MensajeInicioSesion.ParametrosToString("us,r", "pwd"));
		}
	}
}
