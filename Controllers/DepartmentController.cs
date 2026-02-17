using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParcelDelivery.Api.Data;
using ParcelDelivery.Api.Models;
using ParcelDelivery.Api.DTO;

namespace ParcelDelivery.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DepartmentController : ControllerBase
{
    private readonly AppDbContext _context;

    public DepartmentController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetDepartments()
    {
        return Ok(await _context.Departments.ToListAsync());
    }

    [HttpPost]
    public async Task<IActionResult> CreateDepartment([FromBody] DepartmentDTO dto)
    {
        var dept = new Department
        {
            Name = dto.Name,
            WeightLimit = dto.WeightLimit
        };
        _context.Departments.Add(dept);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetDepartments), new { id = dept.Id }, dept);
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateDepartment(int id, [FromBody] DepartmentUpdateDTO dto)
    {
        var dept = await _context.Departments.FindAsync(id);
        if (dept == null) return NotFound();
        
        dept.Name = dto.Name;
        dept.WeightLimit = dto.WeightLimit;
        
        await _context.SaveChangesAsync();
        return Ok(dept);
    }
}
