using ParcelDelivery.Api.Models;

namespace ParcelDelivery.Api.Interfaces;

public interface IParcelClassifier
{
    Task<Department> ClassifyDepartment(double weightKg);
    Task<Department> GetDefaultDepartmentAsync();
}
