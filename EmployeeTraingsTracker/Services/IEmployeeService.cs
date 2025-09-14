using EmployeeTraingsTracker.Model;
using Microsoft.EntityFrameworkCore;

namespace EmployeeTraingsTracker.Services
{
    public interface IEmployeeService
    {
        Task<List<Employee>> GetAllAsync();
        Task<Employee> GetByIdAsync(int id);
        Task AddAsync(Employee employee);
        Task UpdateAsync(Employee employee);
        Task DeleteAsync(int id);
    }
}
