using ParcelDelivery.Api.Interfaces;
using ParcelDelivery.Api.Models;
using ParcelDelivery.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace ParcelDelivery.Api.Services;

public class ParcelClassifier : IParcelClassifier
{
    private readonly AppDbContext _context;

    public ParcelClassifier(AppDbContext context)
    {
        _context = context;
    }

    // Current business rules:
    // - up to 1 kg => Mail
    // - up to 10 kg => Regular
    // - over 10 kg => Heavy
    public async Task<Department> ClassifyDepartment(double weightKg)
    {
        var depts = await _context.Departments
            .Where(d => d.Name != "Insurance")
            .ToListAsync();

        var match = depts
            .OrderBy(d => d.WeightLimit ?? double.MaxValue)
            .FirstOrDefault(d => weightKg <= (d.WeightLimit ?? double.MaxValue));

        return match ?? depts.FirstOrDefault(d => d.Name == "Heavy") ?? throw new Exception("No suitable department found");
    }
    
    public async Task<Department> GetDefaultDepartmentAsync()
    {
        return await _context.Departments.FirstOrDefaultAsync(d => d.Name == "Insurance") ?? throw new Exception("Default department not found");
    }
}
