using Microsoft.EntityFrameworkCore;
using ParcelDelivery.Api.Data;
using ParcelDelivery.Api.Interfaces;
using ParcelDelivery.Api.Models;

namespace ParcelDelivery.Api.DAO;

public class ParcelDao : IParcelDao
{
    private readonly AppDbContext _db;
    public ParcelDao(AppDbContext db) => _db = db;
    public async Task<Parcel?> GetParcelByIdAsync(Guid parcelId)
    {
        return await _db.Parcels
            .Include(p => p.Recipient)
            .FirstOrDefaultAsync(p => p.Id == parcelId);
    }

    public async Task UpdateParcelAsync(Parcel parcel)
    {
        _db.Parcels.Update(parcel);
        await _db.SaveChangesAsync();
    }
    public async Task<List<Parcel>> ListAsync()
    {
        return await _db.Parcels
            .Include(p => p.Recipient)
            .ToListAsync();
    }
}