using ParcelDelivery.Api.Interfaces;
using ParcelDelivery.Api.Models;

namespace ParcelDelivery.Api.Services;

public class ApprovalClassifier : IApprovalClassifier
{
    public ApprovalStatus ClassifyApproval(decimal value)
    {
        if (value > 1000) return ApprovalStatus.Pending; 
        return ApprovalStatus.Approved; 
    }
}
