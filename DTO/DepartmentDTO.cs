
namespace ParcelDelivery.Api.DTO;    
public record DepartmentDTO(string Name, double? WeightLimit);
public record DepartmentUpdateDTO(string Name, double? WeightLimit);
