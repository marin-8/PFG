
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PFG.Aplicacion
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Shell_Principal : Shell
	{
		public Shell_Principal()
		{
			InitializeComponent();

			// Routing.RegisterRoute("NavegacionEnd", typeof(NavegacionEnd));
		}
	}
}
