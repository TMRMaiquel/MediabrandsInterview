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
    public class EmployeeService : ServiceBase, IEmployeeService
    {
        #region Constructor

        public EmployeeService(IUnitOfWorkAdmin unitOfWork)
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

                Employee employee = (await this.UnitOfWork.EmployeeRepository.GetAsync(x => x.State && x.Id == Id)).FirstOrDefault() ?? new Employee();
                List<Office> listOffice = (await this.UnitOfWork.OfficeRepository.GetAsync(x => x.State)).ToList() ?? new List<Office>();
                List<Position> listPosition = (await this.UnitOfWork.PositionRepository.GetAsync(x => x.State)).ToList() ?? new List<Position>();

                response.employee = employee;
                response.listOffice = listOffice;
                response.listPosition = listPosition;

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Employee> Insert(JObject objectJSON)
        {
            try
            {
                using (var transaction = this.UnitOfWork.BeginTransaction())
                {
                    try
                    {
                        Employee employee = objectJSON["DTOEmployee"].ToObject<Employee>();
                        Office office = objectJSON["DTODetailForm"]["DTOOffice"].ToObject<Office>();

                        employee.IdOffice = office.Id;

                        //await this.UnitOfWork.EmployeeRepository.ChangeStateTracking(employee, EntityTracking.Added);
                        //await this.UnitOfWork.EmployeeRepository.NoTrackingObject(employee, x => x.Office);
                        //await this.UnitOfWork.EmployeeRepository.NoTrackingCollection(employee, x => x.ListEmployeePosition);
                        //await this.UnitOfWork.SaveChangesAsync();

                        await this.UnitOfWork.EmployeeRepository.InsertBySP(employee);

                        employee = (await this.UnitOfWork.EmployeeRepository.GetAsync(x => x.State && x.Id == employee.Id, null, 0, true)).FirstOrDefault();

                        transaction.Commit();
                        return employee;
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

        public async Task<Employee> Update(JObject objectJSON)
        {
            try
            {
                using (var transaction = this.UnitOfWork.BeginTransaction())
                {
                    try
                    {
                        Employee employee = objectJSON["DTOEmployee"].ToObject<Employee>();
                        Office office = objectJSON["DTODetailForm"]["DTOOffice"].ToObject<Office>();

                        employee.IdOffice = office.Id;

                        await this.UnitOfWork.EmployeeRepository.ChangeStateTracking(employee, EntityTracking.Modified);
                        await this.UnitOfWork.EmployeeRepository.NoTrackingObject(employee, x => x.Office);
                        await this.UnitOfWork.EmployeeRepository.NoTrackingCollection(employee, x => x.ListEmployeePosition);

                        await this.UnitOfWork.SaveChangesAsync();

                        employee = (await this.UnitOfWork.EmployeeRepository.GetAsync(x => x.State && x.Id == employee.Id, null, 0, true)).FirstOrDefault();

                        transaction.Commit();
                        return employee;
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
                        Employee employee = objectJSON["DTOEmployee"].ToObject<Employee>();

                        employee.State = false;

                        await this.UnitOfWork.EmployeeRepository.ChangeStateTracking(employee, EntityTracking.Modified);

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

        public async Task<List<Employee>> GetByParameters(JObject objectJSON)
        {
            try
            {
                List<Employee> listEmployee = (await this.UnitOfWork.EmployeeRepository.GetAsync(x => x.State)).ToList() ?? new List<Employee>();
                return listEmployee;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
