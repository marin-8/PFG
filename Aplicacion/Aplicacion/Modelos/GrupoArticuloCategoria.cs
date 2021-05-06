
using System.Collections.Generic;

using PFG.Comun;

namespace PFG.Aplicacion
{
	public class GrupoArticuloCategoria : List<Articulo>
    {
        public string Categoria { get; set; }

        public GrupoArticuloCategoria(string Categoria)
        {
            this.Categoria = Categoria;
        }
    }
}
