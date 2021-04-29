
using System;
using System.Collections.Generic;
using System.Text;

using PFG.Comun;

namespace PFG.Gestor
{
	public static class Global
	{
		public static readonly List<Usuario> Usuarios = new() { new("admin","admin", Roles.Administrador) };
	}
}
