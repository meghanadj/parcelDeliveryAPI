using ParcelDelivery.Api.Models;

namespace ParcelDelivery.Api.DTO;

public record OrderDTO(
    int Id,
    int OrderNumber,
    DateTime ShippingDate,
    List<ParcelDTO> Parcels
);

