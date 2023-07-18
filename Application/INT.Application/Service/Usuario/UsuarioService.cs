using INT.Domain;
using INT.Domain.Entity;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace INT.Application.Service
{
    public class UsuarioService : ServiceBase, IUsuarioService
    {
        #region Constructor

        public UsuarioService(IUnitOfWorkAdmin unitOfWork)
        {
            this.UnitOfWork = unitOfWork;
        }

        #endregion

        #region Métodos

        public async Task<dynamic> GetDependencies(int Id)
        {
            try
            {
                dynamic response = new ExpandoObject();

                Usuario usuario = (await this.UnitOfWork.UsuarioRepository.GetAsync(x => x.State && x.Id == Id)).FirstOrDefault() ?? new Usuario();
                List<Perfil> listaPerfil = (await this.UnitOfWork.PerfilRepository.GetAsync(x => x.State)).ToList() ?? new List<Perfil>();
                List<UsuarioPerfil> listaUsuarioPerfil = (await this.UnitOfWork.UsuarioPerfilRepository.GetAsync(x => x.State && x.IdUsuario == Id)).ToList() ?? new List<UsuarioPerfil>();

                response.usuario = usuario;
                response.listaPerfil = listaPerfil;
                response.listaUsuarioPerfil = listaUsuarioPerfil;

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Usuario> Insert(JObject objectJSON)
        {
            try
            {
                using (var transaction = this.UnitOfWork.BeginTransaction())
                {
                    try
                    {
                        Usuario usuario = objectJSON["DTOUsuario"].ToObject<Usuario>();
                        List<Perfil> listaPerfil = objectJSON["DTODetailForm"]["listaPerfil"].ToObject<List<Perfil>>();

                        //SE GUARDA EL USUARIO
                        await this.UnitOfWork.UsuarioRepository.ChangeStateTracking(usuario, EntityTracking.Added);
                        await this.UnitOfWork.SaveChangesAsync();

                        //SE GUARDA LOS PERFILES ASOCIADOS AL USUARIO
                        //SE MAPEA LA INFORMACION DEL USUARIO CON SUS PERFILES
                        foreach (Perfil perfil in listaPerfil)
                        {
                            UsuarioPerfil usuarioPerfil = new UsuarioPerfil
                            {
                                IdUsuario = usuario.Id,
                                IdPerfil = perfil.Id,
                                State = true
                            };

                            await this.UnitOfWork.UsuarioPerfilRepository.ChangeStateTracking(usuarioPerfil, EntityTracking.Added);
                            await this.UnitOfWork.UsuarioPerfilRepository.NoTrackingObject(usuarioPerfil, x => x.Usuario);
                            await this.UnitOfWork.UsuarioPerfilRepository.NoTrackingObject(usuarioPerfil, x => x.Perfil);
                        }

                        await this.UnitOfWork.SaveChangesAsync();

                        usuario = (await this.UnitOfWork.UsuarioRepository.GetAsync(x => x.State && x.Id == usuario.Id, null, 0, true)).FirstOrDefault();

                        transaction.Commit();
                        return usuario;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Usuario> Update(JObject objectJSON)
        {
            try
            {
                using (var transaction = this.UnitOfWork.BeginTransaction())
                {
                    try
                    {
                        Usuario usuario = objectJSON["DTOUsuario"].ToObject<Usuario>();
                        List<Perfil> listaPerfil = objectJSON["DTODetailForm"]["listaPerfil"].ToObject<List<Perfil>>();
                        List<UsuarioPerfil> listaUsuarioPerfilRegistrado = null;
                        List<Perfil> listaPerfilParaRegistro = null;
                        List<UsuarioPerfil> listaUsuarioPerfilParaModificacion = null;
                        List<UsuarioPerfil> listaUsuarioPerfilParaEliminacion = null;

                        //OBTENEMOS LA LISTA DE PERFILES YA REGISTRADOS EN LA BASE DE DATOS
                        listaUsuarioPerfilRegistrado = (await this.UnitOfWork.UsuarioPerfilRepository.GetAsync((x => x.IdUsuario == usuario.Id), null, 0, true)).ToList() ?? new List<UsuarioPerfil>();

                        //SI EL USUARIO NO TIENE PERFILES PROCEDEMOS A REGISTRARSELOS
                        if (listaUsuarioPerfilRegistrado.Count == default(int))
                        {
                            foreach (Perfil perfil in listaPerfil)
                            {
                                UsuarioPerfil usuarioPerfil = new UsuarioPerfil
                                {
                                    IdUsuario = usuario.Id,
                                    IdPerfil = perfil.Id,
                                    State = true
                                };

                                await this.UnitOfWork.UsuarioPerfilRepository.ChangeStateTracking(usuarioPerfil, EntityTracking.Added);
                                await this.UnitOfWork.UsuarioPerfilRepository.NoTrackingObject(usuarioPerfil, x => x.Usuario);
                                await this.UnitOfWork.UsuarioPerfilRepository.NoTrackingObject(usuarioPerfil, x => x.Perfil);
                            }
                        }
                        else
                        {
                            //OBTENEMOS LA NUEVA LISTA DE PERFILES 
                            listaPerfilParaRegistro = listaPerfil.Where(x => !listaUsuarioPerfilRegistrado.Select(p => p.IdPerfil).Contains(x.Id)).ToList() ?? new List<Perfil>();

                            //OBTENEMOS LA LISTA DE PERFILES PARA SU MODIFICACION
                            listaUsuarioPerfilParaModificacion = listaUsuarioPerfilRegistrado.Where(x => listaPerfil.Select(p => p.Id).Contains(x.IdPerfil)).ToList() ?? new List<UsuarioPerfil>();

                            //OBTENEMOS LA LISTA DE PERFILES PARA SU ELIMINACION
                            listaUsuarioPerfilParaEliminacion = listaUsuarioPerfilRegistrado.Where(x => !listaUsuarioPerfilParaModificacion.Select(p => p.IdPerfil).Contains(x.IdPerfil)).ToList() ?? new List<UsuarioPerfil>();

                            //REGISTRAMOS LOS NUEVOS PERFILES
                            foreach (var perfil in listaPerfilParaRegistro)
                            {
                                UsuarioPerfil nuevoUsuarioPerfil = new UsuarioPerfil
                                {
                                    IdUsuario = usuario.Id,
                                    IdPerfil = perfil.Id,
                                    State = true
                                };

                                await this.UnitOfWork.UsuarioPerfilRepository.ChangeStateTracking(nuevoUsuarioPerfil, EntityTracking.Added);
                                await this.UnitOfWork.UsuarioPerfilRepository.NoTrackingObject(nuevoUsuarioPerfil, x => x.Usuario);
                                await this.UnitOfWork.UsuarioPerfilRepository.NoTrackingObject(nuevoUsuarioPerfil, x => x.Perfil);
                            }

                            //ACTUALIZAMOS LOS PERFILES (HABILITAMOS EL PERFIL SI ESTE YA SE ENCONTRABA ASOCIADO, PERO TAL VEZ FUE ELIMINADO)
                            foreach (var perfilUsuario in listaUsuarioPerfilParaModificacion)
                            {
                                perfilUsuario.State = true;
                                await this.UnitOfWork.UsuarioPerfilRepository.ChangeStateTracking(perfilUsuario, EntityTracking.Modified);
                                await this.UnitOfWork.UsuarioPerfilRepository.NoTrackingObject(perfilUsuario, x => x.Usuario);
                                await this.UnitOfWork.UsuarioPerfilRepository.NoTrackingObject(perfilUsuario, x => x.Perfil);
                            }

                            //ELIMINAMOS LOS PERFILES
                            foreach (var perfilUsuario in listaUsuarioPerfilParaEliminacion)
                            {
                                perfilUsuario.State = false;
                                await this.UnitOfWork.UsuarioPerfilRepository.ChangeStateTracking(perfilUsuario, EntityTracking.Modified);
                                await this.UnitOfWork.UsuarioPerfilRepository.NoTrackingObject(perfilUsuario, x => x.Usuario);
                                await this.UnitOfWork.UsuarioPerfilRepository.NoTrackingObject(perfilUsuario, x => x.Perfil);
                            }
                        }

                        await this.UnitOfWork.SaveChangesAsync();
                        usuario = (await this.UnitOfWork.UsuarioRepository.GetAsync(x => x.State && x.Id == usuario.Id, null, 0, true)).FirstOrDefault();

                        transaction.Commit();
                        return usuario;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> Delete(JObject objectJSON)
        {
            try
            {
                using (var transaction = this.UnitOfWork.BeginTransaction())
                {
                    try
                    {
                        bool response = default(bool);
                        Usuario usuario = objectJSON["DTOUsuario"].ToObject<Usuario>();
                        List<UsuarioPerfil> listaUsuarioPerfilRegistrado = null;

                        //OBTENEMOS LA LISTA DE PERFILES YA REGISTRADOS EN LA BASE DE DATOS
                        listaUsuarioPerfilRegistrado = (await this.UnitOfWork.UsuarioPerfilRepository.GetAsync(x => x.IdUsuario == usuario.Id)).ToList() ?? new List<UsuarioPerfil>();

                        //DESHABILITAMOS EL REGISTRO
                        usuario.State = false;
                        await this.UnitOfWork.UsuarioRepository.ChangeStateTracking(usuario, EntityTracking.Modified);

                        //DESHABILITAMOS SUS PERFILES
                        foreach (var perfilUsuario in listaUsuarioPerfilRegistrado)
                        {
                            perfilUsuario.State = false;
                            await this.UnitOfWork.UsuarioPerfilRepository.ChangeStateTracking(perfilUsuario, EntityTracking.Modified);
                            await this.UnitOfWork.UsuarioPerfilRepository.NoTrackingObject(perfilUsuario, x => x.Usuario);
                            await this.UnitOfWork.UsuarioPerfilRepository.NoTrackingObject(perfilUsuario, x => x.Perfil);
                        }

                        response = (await this.UnitOfWork.SaveChangesAsync() != default(int));

                        transaction.Commit();
                        return response;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<Usuario>> GetByParameters(JObject objectJSON)
        {
            try
            {
                List<Usuario> listUsuario = (await this.UnitOfWork.UsuarioRepository.GetAsync(x => x.State)).ToList() ?? new List<Usuario>();
                return listUsuario;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
