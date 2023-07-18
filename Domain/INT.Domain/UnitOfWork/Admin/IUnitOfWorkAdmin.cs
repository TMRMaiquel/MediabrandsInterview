using INT.Domain.Repository;

namespace INT.Domain
{
    public interface IUnitOfWorkAdmin : IUnitOfWorkBase
    {
        IEmployeeRepository EmployeeRepository { get; }
        IEmployeePositionRepository EmployeePositionRepository { get; }
        IOfficeRepository OfficeRepository { get; }
        IPositionRepository PositionRepository { get; }
        IUsuarioRepository UsuarioRepository { get; }
        IPerfilRepository PerfilRepository { get; }
        IUsuarioPerfilRepository UsuarioPerfilRepository { get; }
    }
}
