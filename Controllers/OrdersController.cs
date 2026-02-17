using Microsoft.AspNetCore.Mvc;
using ParcelDelivery.Api.DTO;
using System.Text.Json;
using ParcelDelivery.Api.Models;
using ParcelDelivery.Api.Interfaces;
namespace ParcelDelivery.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrderDao _orderDao;
    private readonly IParcelClassifier _classifier;
    private readonly IApprovalClassifier _approvalClassifier;
    public OrdersController(IOrderDao orderDao, IParcelClassifier classifier, IApprovalClassifier approvalClassifier)
    {
        _orderDao = orderDao;
        _classifier = classifier;
        _approvalClassifier = approvalClassifier;
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] OrderDTO request)
    {
        var parcelsList = new List<Parcel>();
        var insuranceDept = await _classifier.GetDefaultDepartmentAsync();

        foreach (var p in request.Parcels)
        {
            var approvalStatus = _approvalClassifier.ClassifyApproval(p.Value);
            Department dept;

            if (approvalStatus == ApprovalStatus.Approved)
            {
                dept = await _classifier.ClassifyDepartment(p.Weight);
            }
            else
            {
                dept = insuranceDept;
            }

            var parcel = new Parcel
            {
                Id = Guid.NewGuid(),
                Weight = p.Weight,
                Value = p.Value,
                Approved = approvalStatus, // set the approval status
                Recipient = new Recipient
                {
                    Id = Guid.NewGuid(),
                    Name = p.RecipientName,
                    AddressJson = JsonSerializer.Serialize(p.RecipientAddress),
                    Phone = string.Empty
                },
                Department = dept
            };
            parcelsList.Add(parcel);
        }

        var order = new Order
        {
            Id = request.Id,
            OrderNumber = request.OrderNumber,
            ShippingDate = request.ShippingDate,
            Parcels = parcelsList
        };

        await _orderDao.CreateAsync(order);

        return CreatedAtAction(nameof(GetOrderById), new { id = order.Id }, new {
            Message = "Order created",
            OrderId = order.Id,
            ParcelsCount = order.Parcels.Count,
            ReceivedAt = DateTime.Now
        });
    }

    [HttpGet]
    public async Task<IActionResult> GetAllOrders()
    {
        var orders = await _orderDao.ListAsync();
        return Ok(orders);
    }
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetOrderById(int id)
    {
        var order = await _orderDao.GetByIdAsync(id);
        if (order == null)
        {
            return NotFound(new { Message = "Order not found", OrderId = id });
        }
        return Ok(order);
    }
    
}