
using System.Text;

namespace PFG.Comun
{
	public abstract class Comando
	{
		public TiposComando TipoComando { get; private set; }

		protected Comando(TiposComando CodigoAccion)
		{
			TipoComando = CodigoAccion;
		}

		public override string ToString()
		{
			StringBuilder sb = new($"{((ushort)TipoComando)},");

			var propiedades = GetType().GetProperties();

			for(int i = 0 ; i < propiedades.Length-1 ; i++)
			{
				sb.Append(propiedades[i].GetValue(this, null).ToString());
				sb.Append(',');
			}

			return sb.Remove(sb.Length-1, 1).ToString();
		}

		//public static dynamic DeString(string Mensaje)
		//{
		//	var valoresPropiedades = new List<string>(Mensaje.Split(','));

		//	Type type = Type.GetType(CodigosAccionesTipos.AccionTipo[(CodigosAcciones)Enum.Parse(typeof(CodigosAcciones), valoresPropiedades[0])]); 

		//	valoresPropiedades.RemoveAt(0); 
		//	return Activator.CreateInstance(type, valoresPropiedades.ToArray());
		//}

		//protected Mensaje(string Mensaje)
		//{
		//	var propiedades = GetType().GetProperties();
		//	var valoresPropiedades = Mensaje.Split(',');

		//	CodigoAccion = (CodigosAcciones)ushort.Parse(valoresPropiedades[0]);

		//	for(int i = 0 ; i < propiedades.Length-1 ; i++)
		//	{
		//		propiedades[i].SetValue(this, Convert.ChangeType(valoresPropiedades[i+1], propiedades[i].PropertyType));
		//	}
		//}
	}
}
