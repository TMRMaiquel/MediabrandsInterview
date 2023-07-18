using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INT.Domain.Entity
{
    public class UsuarioPerfil
    {
        #region Constructor

        public UsuarioPerfil()
        {
            
        }

        #endregion

        #region Miembros
        public int IdUsuario { get; set; }
        public int IdPerfil { get; set; }
        public bool State { get; set; }

        //PROPIEDADES DE NAVEGACION
        public Usuario Usuario { get; set; }
        public Perfil Perfil { get; set; }

        #endregion
    }
}
