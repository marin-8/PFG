
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PFG.Gestor
{
	public class Procesador
	{
		private ListBox Registro;

		public Procesador(ListBox Registro)
		{
			this.Registro = Registro;
		}

		public void Procesar(string IP, string Mensaje)
		{
			// TODO - Procesar

			Registro.Invoke(new Action(() =>
			{ 
				Registro.Items.Add($"{IP} > {Mensaje}");
			}));
		}
	}
}
