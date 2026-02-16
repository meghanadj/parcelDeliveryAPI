using ParcelDelivery.Api.Models;

namespace ParcelDelivery.Api.Interfaces;

public interface IParcelDao
{
    Task<Parcel?> GetParcelByIdAsync(Guid parcelId);
    Task UpdateParcelAsync(Parcel parcel);
    Task<List<Parcel>> ListAsync();
}
