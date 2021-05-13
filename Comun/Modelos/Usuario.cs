
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace PFG.Comun
{
	public class Usuario
	{
		[JsonProperty("1")] public string Nombre { get; set; }
		[JsonProperty("2")] public string NombreUsuario { get; set; }
		[JsonProperty("3")] public string Contrasena { get; set; }

		[JsonProperty("4")] public Roles Rol { get; set; }

			   [JsonIgnore] public string IP { get; set; }
			   [JsonIgnore] public bool Conectado { get; set; }

		public Usuario(string Nombre, string NombreUsuario, string Contrasena, Roles Rol, string IP = "", bool Conectado = false)
		{
			this.Nombre = Nombre;
			this.NombreUsuario = NombreUsuario;
			this.Contrasena = Contrasena;
			this.Rol = Rol;

			this.IP = IP;
			this.Conectado = Conectado;
		}
	}
}
