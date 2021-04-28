
using System;
using System.Collections.Generic;
using System.Text;

namespace PFG.Comun
{
	public class Usuario
	{
		public string Nombre { get; set; }
		public string Contrasena { get; set; }

		public string IP { get; set; }
		public Roles Rol { get; set; }

		public bool Conectado { get; set; }

		public Usuario(string Nombre, string Contrasena, Roles Rol, string IP = "", bool Conectado = false)
		{
			this.Nombre = Nombre;
			this.Contrasena = Contrasena;
			this.Rol = Rol;

			this.IP = IP;
			this.Conectado = Conectado;
		}
	}
}
