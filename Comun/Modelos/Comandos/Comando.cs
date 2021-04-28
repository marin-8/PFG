
using System;
using System.Text;

namespace PFG.Comun
{
	public abstract class Comando
	{
		public TiposComando TipoComando { get; private set; }

		protected Comando(TiposComando TipoComando)
		{
			this.TipoComando = TipoComando;
		}

		protected Comando(string ComandoString)
		{
			var propiedades = GetType().GetProperties();
			var valoresPropiedades = ComandoString.Split(',');

			TipoComando = (TiposComando)ushort.Parse(valoresPropiedades[0]);

			for (int i = 0; i < propiedades.Length - 1; i++)
			{
				if(propiedades[i].PropertyType.IsEnum)
				{
					propiedades[i].SetValue(this, 
						Convert.ChangeType(
							Enum.Parse(propiedades[i].PropertyType, valoresPropiedades[i + 1]),
							propiedades[i].PropertyType));
				}
				else
				{
					propiedades[i].SetValue(this,
						Convert.ChangeType(
							valoresPropiedades[i + 1],
							propiedades[i].PropertyType));
				}
			}
		}

		public override string ToString()
		{
			StringBuilder sb = new($"{(ushort)TipoComando},");

			var propiedades = GetType().GetProperties();

			for(int i = 0 ; i < propiedades.Length-1 ; i++)
			{
				sb.Append(propiedades[i].GetValue(this, null).ToString());
				sb.Append(',');
			}

			return sb.Remove(sb.Length-1, 1).ToString();
		}
	}
}
