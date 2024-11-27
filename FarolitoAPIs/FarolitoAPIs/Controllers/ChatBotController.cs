using System.Security.Claims;
using FarolitoAPIs.Data;
using FarolitoAPIs.DTOs;
using FarolitoAPIs.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace FarolitoAPIs.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ChatBotController : ControllerBase
{
    private readonly FarolitoDbContext _context;
    private readonly UserManager<Usuario> _userManager;

    public ChatBotController(FarolitoDbContext context, UserManager<Usuario> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    // Obtener lámparas disponibles y sus precios
    [HttpGet("lamps")]
    public async Task<IActionResult> GetAvailableLamps()
    {
        var lamps = await _context.Receta
            .Select(r => new AvailableLampsDto
            {
                Id = r.Id,
                Name = r.Nombrelampara,
                AvailableQuantity = r.Inventariolamparas.Sum(il => il.Cantidad ?? 0),
                AveragePrice = r.Inventariolamparas.Average(il => il.Precio ?? 0)
            })
            .OrderBy(l => l.Name)
            .ToListAsync();

        if (lamps.Count == 0)
        {
            return NotFound(new AuthResponseDTO
            {
                IsSuccess = false,
                Message = "There are no lamps in the inventory"
            });
        }

        return Ok(lamps);
    }
    
    // Consultar estado de un pedido
    [Authorize]
    [HttpGet("order/{id}")]
    public async Task<IActionResult> GetOrderState(int id)
    {
        if (id <= 0)
        {
            return BadRequest(new AuthResponseDTO
            {
                IsSuccess = false,
                Message = "Invalid order ID."
            });
        }

        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(currentUserId))
        {
            return Unauthorized(new AuthResponseDTO
            {
                IsSuccess = false,
                Message = "User is not authenticated."
            });
        }

        try
        {
            var order = await _context.Pedidos
                .Include(p => p.Ventum)
                .FirstOrDefaultAsync(p => p.VentumId == id && p.Ventum.UsuarioId == currentUserId);

            if (order == null)
            {
                return NotFound(new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "Order not found with specified sell id."
                });
            }

            return Ok(new OrderStatusDto
            {
                OrderId = order.Id,
                State = order.Estatus,
                OrderDate = order.FechaPedido,
                ShippingDate = order.FechaEnvio,
                DeliveryDate = order.FechaEntrega
            });
        }
        catch (Exception)
        {
            return StatusCode(500, new AuthResponseDTO
            {
                IsSuccess = false,
                Message = "Something went wrong getting the order status. Please try again later."
            });
        }
    }

    // Componentes requeridos para una lámpara específica
    [HttpGet("lamp-components/{id}")]
    public async Task<IActionResult> GetLampComponents(int id)
    {
        if (id <= 0)
        {
            return BadRequest(new AuthResponseDTO
            {
                IsSuccess = false,
                Message = "Invalid lamp ID."
            });
        }

        var recipe = await _context.Receta
            .Include(r => r.Componentesreceta)
            .ThenInclude(cr => cr.Componentes)
            .ThenInclude(c => c.Productoproveedors).ThenInclude(pp => pp.Proveedor)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (recipe == null)
        {
            return NotFound(new AuthResponseDTO
            {
                IsSuccess = false,
                Message = "No such recipe."
            });
        }

        var components = recipe.Componentesreceta.Select(cr =>
            new RequiredComponentsDto
            {
                Id = cr.Componentes.Id,
                Name = cr.Componentes.Nombre,
                RequiredAmount = cr.Cantidad,
                Supplier = string.Join(", ",
                    cr.Componentes.Productoproveedors.Select(pp => pp.Proveedor.NombreAtiende ?? "Unknown"))
            }
        );

        return Ok(components);
    }

    // Disponibilidad de un componente
    [HttpGet("component-availability/{id}")]
    public async Task<IActionResult> GetComponentAvailability(int id)
    {
        if (id <= 0)
        {
            return BadRequest(new AuthResponseDTO
            {
                IsSuccess = false,
                Message = "Invalid component ID."
            });
        }

        var component = await _context.Componentes
            .Include(c => c.Inventariocomponentes)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (component == null)
        {
            return NotFound(new AuthResponseDTO
            {
                IsSuccess = false,
                Message = "Component not found"
            });
        }

        var availability = new ComponentAvailabilityDto
        {
            Id = component.Id,
            Name = component.Nombre,
            AvailableQuantity = component.Inventariocomponentes?.Sum(ic => ic.Cantidad ?? 0) ?? 0
        };

        return Ok(availability);
    }

    // Información detallada de un cliente
    [Authorize]
    [HttpGet("customer-info")]
    public async Task<IActionResult> GetCustomerInfo()
    {
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(currentUserId))
        {
            return Unauthorized(new AuthResponseDTO
            {
                IsSuccess = false,
                Message = "User is not authenticated."
            });
        }
        
        var customer = await _context.Users.FirstOrDefaultAsync(c => c.Id == currentUserId);
        
        if (customer == null)
        {
            return NotFound(new AuthResponseDTO
            {
                IsSuccess = false,
                Message = "Authenticated user not found in the system."
            });
        }

        var customerInfo = new UserDetailDTO
        {
            Id = customer.Id,
            FullName = customer.FullName,
            Email = customer.Email,
            PhoneNumber = customer.PhoneNumber,
            TwoFactorEnabled = customer.TwoFactorEnabled,
            PhoneNumberConfirmed = customer.PhoneNumberConfirmed,
            AccessFailedCount = customer.AccessFailedCount,
            Direccion = customer.Direccion,
        };

        return Ok(customerInfo);
    }

    // Información más general
    [HttpGet("info")]
    public IActionResult GetGeneralInfo()
    {
        var info = new GeneralInfoDto
        {
            OpeningHours = "Lunes a Viernes, 9:00 AM - 6:00 PM",
            Phone = "+52 477 260 6019",
            EmailAddress = "contacto_farolito@gmail.com"
        };

        return Ok(info);
    }
}