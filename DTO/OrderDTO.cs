using ParcelDelivery.Api.Models;

namespace ParcelDelivery.Api.DTO;

public record OrderDTO(
    int Id,
    DateTime ShippingDate,
    List<ParcelDTO> Parcels
);

