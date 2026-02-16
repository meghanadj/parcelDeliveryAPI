using ParcelDelivery.Api.Models;

namespace ParcelDelivery.Api.Interfaces;

public interface IApprovalClassifier
{
    ApprovalStatus ClassifyApproval(decimal value);
    
}
