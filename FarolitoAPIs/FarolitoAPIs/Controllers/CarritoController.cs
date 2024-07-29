using FarolitoAPIs.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FarolitoAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarritoController : ControllerBase
    {
        private readonly FarolitoDbContext _baseDatos;
        public CarritoController(FarolitoDbContext baseDatos)
        {
            _baseDatos = baseDatos;
        }
    }
}
