
using System;
using System.Diagnostics;

using PFG.Comun;

namespace PFG.Pruebas
{
	class Program
	{
		static void Main(string[] args)
		{
			MensajeInicioSesion m1 = new("usr", "pwd");
			MensajeInicioSesion m2 = new(m1.ToString());
			Debug.WriteLine(m2.ToString());
		}
	}
}
