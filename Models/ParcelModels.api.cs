namespace ParcelDelivery.Api.Models;

public class Department 
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public double? WeightLimit { get; set; }
}

public enum ApprovalStatus { Rejected = 0, Approved = 1, Pending = 2 }



public record Address
{
    public string Street { get; init; } = default!;
    public string HouseNo { get; init; } = default!;
    public string City { get; init; } = default!;
    public int Pincode { get; init; }

}

public class Recipient
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    // store the address as JSON in the DB for now; we may split Address into its own table later
    public string? AddressJson { get; set; }
    public string Phone { get; set; } = default!;

    // navigation
    public ICollection<Parcel>? Parcels { get; set; }
}

public class Parcel
{
    public Guid Id { get; set; }
    public double Weight { get; set; }
    public decimal Value { get; set; }

    public ApprovalStatus Approved { get; set; } = ApprovalStatus.Pending;


    // foreign keys
    public Guid? RecipientId { get; set; }
    public Recipient? Recipient { get; set; }

    public int DepartmentId { get; set; } = 4; 
    public Department? Department { get; set; }

    // relation to Order
    public int? OrderId { get; set; }
    public Order? Order { get; set; }
}

public record Order
{
    public int Id { get; init; }
    public int OrderNumber {get; init;}
    public DateTime ShippingDate { get; init; }
    public List<Parcel> Parcels { get; init; } = new();
}