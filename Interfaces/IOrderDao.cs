using ParcelDelivery.Api.Models;

namespace ParcelDelivery.Api.Interfaces;

public interface IOrderDao
{
    Task<Order> CreateAsync(Order order);
    Task<List<Order>> ListAsync();
    Task<Order?> GetByIdAsync(Guid id);
}
