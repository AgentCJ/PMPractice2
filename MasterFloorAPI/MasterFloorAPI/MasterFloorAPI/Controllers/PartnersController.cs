using MasterFloorAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MasterFloorAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PartnersController : ControllerBase
{
    private readonly MasterFloorContext _context;

    public PartnersController(MasterFloorContext context)
    {
        _context = context;
    }

    // GET: api/partners
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var partners = await _context.Partners
            .Include(p => p.Type)
            .Include(p => p.LegalAddress)
            .ToListAsync();

        var result = new List<object>();
        foreach (var p in partners)
        {
            var totalQuantity = await _context.SaleHeaders
                .Where(sh => sh.PartnerId == p.Id)
                .SelectMany(sh => sh.SaleItems)
                .SumAsync(si => si.Quantity);

            int discount = 0;
            if (totalQuantity >= 300000) discount = 15;
            else if (totalQuantity >= 50000) discount = 10;
            else if (totalQuantity >= 10000) discount = 5;

            result.Add(new
            {
                p.Id,
                p.Name,
                PartnerType = p.Type?.Name ?? "",
                DirectorFullName = $"{p.DirectorLastName} {p.DirectorFirstName} {p.DirectorMiddleName}".Trim(),
                p.Phone,
                p.Rating,
                Address = p.LegalAddress == null ? "" :
                    $"{p.LegalAddress.PostalCode}, {p.LegalAddress.City}, {p.LegalAddress.Street}, {p.LegalAddress.House}",
                TotalSalesQuantity = totalQuantity,
                DiscountPercent = discount
            });
        }
        return Ok(result);
    }

    // GET: api/partners/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var partner = await _context.Partners
            .Include(p => p.Type)
            .Include(p => p.LegalAddress)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (partner == null)
            return NotFound($"Партнёр с Id = {id} не найден.");

        return Ok(partner);
    }

    // GET: api/partners/types
    [HttpGet("types")]
    public async Task<IActionResult> GetPartnerTypes()
    {
        var types = await _context.PartnerTypes.ToListAsync();
        return Ok(types);
    }

    // POST: api/partners
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PartnerCreateUpdateDto dto)
    {
        try
        {
            // Валидация
            if (dto.Rating.HasValue && dto.Rating.Value < 0)
                return BadRequest("Рейтинг не может быть отрицательным.");

            if (dto.LegalAddress == null)
                return BadRequest("Адрес обязателен.");

            // Проверим, существует ли тип
            var typeExists = await _context.PartnerTypes.AnyAsync(t => t.Id == dto.TypeId);
            if (!typeExists)
                return BadRequest($"Тип с Id = {dto.TypeId} не существует.");

            // Генерация следующего Id для адреса (ручное присвоение)
            int nextAddressId = await _context.Addresses
                .OrderByDescending(a => a.Id)
                .Select(a => a.Id)
                .FirstOrDefaultAsync() + 1;

            var newAddress = new Address
            {
                Id = nextAddressId,
                PostalCode = dto.LegalAddress.PostalCode,
                Area = dto.LegalAddress.Area,
                City = dto.LegalAddress.City,
                Street = dto.LegalAddress.Street,
                House = dto.LegalAddress.House
            };
            _context.Addresses.Add(newAddress);
            await _context.SaveChangesAsync();

            // Генерация следующего Id для партнёра
            int nextPartnerId = await _context.Partners
                .OrderByDescending(p => p.Id)
                .Select(p => p.Id)
                .FirstOrDefaultAsync() + 1;

            // Уникальный INN
            string uniqueInn = "INN" + Guid.NewGuid().ToString("N").Substring(0, 10);

            var newPartner = new Partner
            {
                Id = nextPartnerId,
                Name = dto.Name,
                TypeId = dto.TypeId,
                Rating = dto.Rating,
                DirectorLastName = dto.DirectorLastName,
                DirectorFirstName = dto.DirectorFirstName,
                DirectorMiddleName = dto.DirectorMiddleName,
                Phone = dto.Phone,
                Email = dto.Email,
                LegalAddressId = newAddress.Id,
                Inn = uniqueInn
            };
            _context.Partners.Add(newPartner);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = newPartner.Id }, newPartner);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Внутренняя ошибка сервера: {ex.Message}");
        }
    }

    // PUT: api/partners/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] PartnerCreateUpdateDto dto)
    {
        try
        {
            var existingPartner = await _context.Partners
                .Include(p => p.LegalAddress)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (existingPartner == null)
                return NotFound($"Партнёр с Id = {id} не найден.");

            if (dto.Rating.HasValue && dto.Rating.Value < 0)
                return BadRequest("Рейтинг не может быть отрицательным.");

            // Проверим, существует ли новый тип (если он меняется)
            var typeExists = await _context.PartnerTypes.AnyAsync(t => t.Id == dto.TypeId);
            if (!typeExists)
                return BadRequest($"Тип с Id = {dto.TypeId} не существует.");

            // Обновляем поля
            existingPartner.Name = dto.Name;
            existingPartner.TypeId = dto.TypeId;
            existingPartner.Rating = dto.Rating;
            existingPartner.DirectorLastName = dto.DirectorLastName;
            existingPartner.DirectorFirstName = dto.DirectorFirstName;
            existingPartner.DirectorMiddleName = dto.DirectorMiddleName;
            existingPartner.Phone = dto.Phone;
            existingPartner.Email = dto.Email;
            // INN не трогаем

            // Обновляем адрес (если передан)
            if (dto.LegalAddress != null)
            {
                existingPartner.LegalAddress.PostalCode = dto.LegalAddress.PostalCode;
                existingPartner.LegalAddress.Area = dto.LegalAddress.Area;
                existingPartner.LegalAddress.City = dto.LegalAddress.City;
                existingPartner.LegalAddress.Street = dto.LegalAddress.Street;
                existingPartner.LegalAddress.House = dto.LegalAddress.House;
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Внутренняя ошибка сервера: {ex.Message}");
        }


    }
    // GET: api/partners/{id}/sales
    [HttpGet("{id}/sales")]
    public async Task<IActionResult> GetSalesHistory(int id)
    {
        var partner = await _context.Partners.FindAsync(id);
        if (partner == null)
            return NotFound($"Партнёр с Id = {id} не найден.");

        var sales = await _context.SaleHeaders
            .Where(sh => sh.PartnerId == id)
            .SelectMany(sh => sh.SaleItems)
            .Select(si => new SaleHistoryDto
            {
                ProductName = si.ProductArticleNavigation.Name,
                Quantity = si.Quantity,
                SaleDate = si.Sale.SaleDate
            })
            .OrderByDescending(s => s.SaleDate)
            .ToListAsync();

        return Ok(sales);
    }
}