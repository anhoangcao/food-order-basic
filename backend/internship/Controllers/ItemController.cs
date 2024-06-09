using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using internship.Data;
using internship.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace internship.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ItemsController> _logger;

        public ItemsController(ApplicationDbContext context, ILogger<ItemsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/Items
        [HttpGet]
        public async Task<IActionResult> GetAll(int pageNumber = 1, int pageSize = 10, int? itemID = null, string itemName = null, decimal? price = null, bool? available = null)
        {
            try
            {
                _logger.LogInformation($"Fetching items - PageNumber: {pageNumber}, PageSize: {pageSize}");

                if (pageNumber <= 0)
                {
                    _logger.LogWarning("Invalid page number. Must be greater than 0.");
                    return BadRequest("Page number must be greater than 0.");
                }

                if (pageSize <= 0)
                {
                    _logger.LogWarning("Invalid page size. Must be greater than 0.");
                    return BadRequest("Page size must be greater than 0.");
                }

                var query = _context.MenuItems.AsQueryable();

                // Apply search filters
                if (itemID.HasValue)
                {
                    query = query.Where(i => i.ItemID == itemID.Value);
                }
                if (!string.IsNullOrEmpty(itemName))
                {
                    query = query.Where(i => i.ItemName.Contains(itemName));
                }
                if (price.HasValue)
                {
                    query = query.Where(i => i.Price == price.Value);
                }
                if (available.HasValue)
                {
                    query = query.Where(i => i.Available == available.Value);
                }

                var totalRecords = await query.CountAsync();
                _logger.LogInformation($"Total Records: {totalRecords}");

                var items = await query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var response = new
                {
                    TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize),
                    CurrentPage = pageNumber,
                    PageSize = pageSize,
                    Items = items
                };

                _logger.LogInformation($"TotalPages: {response.TotalPages}, Items Fetched: {items.Count}");

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching items.");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/Items/10
        [HttpGet("{id}")]
        public async Task<ActionResult<MenuItemModel>> GetMenuItem(int id)
        {
            try
            {
                var item = await _context.MenuItems.FindAsync(id);

                if (item == null)
                {
                    _logger.LogWarning($"Item with ID {id} not found.");
                    return NotFound();
                }

                return Ok(item);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while fetching the item with ID {id}.");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // POST: api/Items/UploadImage
        [HttpPost("UploadImage")]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            var fileUrl = $"{Request.Scheme}://{Request.Host}/uploads/{fileName}";
            return Ok(new { url = fileUrl });
        }

        // PUT: api/Items/10
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateItem(int id, [FromBody] MenuItemModel updatedItem)
        {
            if (id != updatedItem.ItemID)
            {
                return BadRequest();
            }

            var item = await _context.MenuItems.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            item.ItemName = updatedItem.ItemName;
            item.Description = updatedItem.Description;
            item.Price = updatedItem.Price;
            item.Available = updatedItem.Available;
            item.ImageUrl = updatedItem.ImageUrl;
            item.UpdatedAt = DateTime.UtcNow;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(item);
        }

        // POST: api/Items
        [HttpPost]
        public async Task<ActionResult<MenuItemModel>> PostItem(MenuItemModel item)
        {
            if (item == null)
            {
                return BadRequest("Item data is null.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            item.CreatedAt = DateTime.UtcNow;
            item.UpdatedAt = DateTime.UtcNow;

            _context.MenuItems.Add(item);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMenuItem), new { id = item.ItemID }, item);
        }

        // DELETE: api/Items
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            var item = await _context.MenuItems.FindAsync(id);
            if (item == null)
            {
                _logger.LogWarning($"Item with ID {id} not found.");
                return NotFound();
            }

            _context.MenuItems.Remove(item);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Item with ID {id} deleted successfully.");

            return NoContent();
        }

        private bool ItemExists(int id)
        {
            return _context.MenuItems.Any(e => e.ItemID == id);
        }
    }
}