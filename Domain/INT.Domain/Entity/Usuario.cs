using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INT.Domain.Entity
{
    public class Usuario
    {
        #region Constructor

        public Usuario()
        {
            this.ListaUsuarioPerfil = new HashSet<UsuarioPerfil>();
        }

        #endregion

        #region Miembros

        public int Id { get; set; }
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public bool State { get; set; }

        //PROPIEDADES DE NAVEGACION
        public ICollection<UsuarioPerfil> ListaUsuarioPerfil { get; set; }

        #endregion

    }
}
