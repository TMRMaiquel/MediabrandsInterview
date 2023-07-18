using INT.Domain;
using INT.Domain.Repository;
using INT.Infraestructure.Data.Context;
using INT.Infraestructure.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INT.Infraestructure.Data.UnitOfWork
{
    public class UnitOfWorkAdmin : UnitOfWorkBase, IUnitOfWorkAdmin
    {
        #region Constructor

        public UnitOfWorkAdmin()
        {
            this.context = new AdminContext();
            this.contextBase = this.context;
        }

        #endregion

        #region Miembros

        private readonly AdminContext context;
        private IEmployeeRepository employeeRepository;
        private IEmployeePositionRepository employeePositionRepository;
        private IOfficeRepository officeRepository;
        private IPositionRepository positionRepository;
        private IUsuarioRepository usuarioRepository;
        private IPerfilRepository perfilRepository;
        private IUsuarioPerfilRepository usuarioPerfilRepository;

        #endregion

        #region Propiedades

        public IEmployeeRepository EmployeeRepository
        {
            get
            {
                if (this.employeeRepository == null)
                {
                    this.employeeRepository = new EmployeeRepository(this.context.Employee, this.context);
                }
                return this.employeeRepository;
            }
        }
        public IEmployeePositionRepository EmployeePositionRepository
        {
            get
            {
                if (this.employeePositionRepository == null)
                {
                    this.employeePositionRepository = new EmployeePositionRepository(this.context.EmployeePosition, this.context);
                }
                return this.employeePositionRepository;
            }
        }
        public IOfficeRepository OfficeRepository
        {
            get
            {
                if (this.officeRepository == null)
                {
                    this.officeRepository = new OfficeRepository(this.context.Office, this.context);
                }
                return this.officeRepository;
            }
        }
        public IPositionRepository PositionRepository
        {
            get
            {
                if (this.positionRepository == null)
                {
                    this.positionRepository = new PositionRepository(this.context.Position, this.context);
                }
                return this.positionRepository;
            }
        }
        public IUsuarioRepository UsuarioRepository
        {
            get
            {
                if (this.usuarioRepository == null)
                {
                    this.usuarioRepository = new UsuarioRepository(this.context.Usuario, this.context);
                }
                return this.usuarioRepository;
            }
        }
        public IPerfilRepository PerfilRepository
        {
            get
            {
                if (this.perfilRepository == null)
                {
                    this.perfilRepository = new PerfilRepository(this.context.Perfil, this.context);
                }
                return this.perfilRepository;
            }
        }
        public IUsuarioPerfilRepository UsuarioPerfilRepository
        {
            get
            {
                if (this.usuarioPerfilRepository == null)
                {
                    this.usuarioPerfilRepository = new UsuarioPerfilRepository(this.context.UsuarioPerfil, this.context);
                }
                return this.usuarioPerfilRepository;
            }
        }

        #endregion

    }
}
