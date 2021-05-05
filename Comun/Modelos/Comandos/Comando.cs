﻿
using System;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PFG.Comun
{
	public abstract class Comando
	{
		[JsonProperty("0")] public TiposComando TipoComando { get; private set; }

		public static TiposComando Get_TipoComando_De_Json(string Json)
		{
			try
			{
				string tipoComando_string = JObject.Parse(Json)["0"].ToString();
				return (TiposComando)Enum.Parse(typeof(TiposComando), tipoComando_string);
			}
			catch
			{
				string tipoComando_string = JObject.Parse(Json)["0"].ToString();
				return (TiposComando)Enum.Parse(typeof(TiposComando), tipoComando_string);
			}
		}

		protected Comando(TiposComando TipoComando)
		{
			this.TipoComando = TipoComando;
		}

		public static T DeJson<T>(string ComandoJson)
		{
			try
			{
				return JsonConvert.DeserializeObject<T>(ComandoJson);
			}
			catch
			{
				return JsonConvert.DeserializeObject<T>(ComandoJson);
			}
		}

		public override string ToString()
		{
			try
			{
				return JsonConvert.SerializeObject(this);
			}
			catch
			{
				return JsonConvert.SerializeObject(this);
			}
		}
	}
}
