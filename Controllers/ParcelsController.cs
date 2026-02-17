using Microsoft.AspNetCore.Mvc;
using ParcelDelivery.Api.DTO;
using System.Text.Json;
using ParcelDelivery.Api.Models;
using ParcelDelivery.Api.Interfaces;
namespace ParcelDelivery.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ParcelsController : ControllerBase
{
        private readonly IParcelDao _parcelDao;
        private readonly IParcelClassifier _classifier;
    public ParcelsController(IParcelDao parcelDao, IParcelClassifier classifier)
    {
        _parcelDao = parcelDao;
        _classifier = classifier;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllParcels()
    {
        var parcels = await _parcelDao.ListAsync();
        return Ok(parcels);
    }
    [HttpPatch("{parcelId:guid}/approval")]
    public async Task<IActionResult> UpdateParcelApproval(Guid parcelId, [FromBody] ApprovalDTO dto)
    {
        var parcel = await _parcelDao.GetParcelByIdAsync(parcelId);
        if (parcel == null)
            return NotFound(new { Message = "Parcel not found", ParcelId = parcelId });
        if (parcel.Value < 1000)
            {
                return BadRequest(new
                {
                    Message = "Approval cannot be updated because parcel value is less than 1000.",
                    ParcelId = parcel.Id,
                    CurrentValue = parcel.Value
                });
            }
        parcel.Approved = dto.NewStatus;

        if (parcel.Approved == ApprovalStatus.Approved)
        {
            parcel.Department = await _classifier.ClassifyDepartment(parcel.Weight);
        }

        await _parcelDao.UpdateParcelAsync(parcel);

        return Ok(new
        {
            Message = "Parcel updated",
            ParcelId = parcel.Id,
            ApprovalStatus = parcel.Approved,
            Department = parcel.Department
        });
    }
}