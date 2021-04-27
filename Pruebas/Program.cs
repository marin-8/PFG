
using System;
using System.Diagnostics;

using PFG.Comun;

namespace PFG.Pruebas
{
	class Program
	{
		static void Main()
		{
			var b1 = "0".Equals("1") ? true : false;
			var b2 = $"{(false ? "1" : "0")}"
			;
		}
	}
}
