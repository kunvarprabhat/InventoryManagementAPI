using InventoryManagementAPI.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementAPI.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public InventoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetInventories()
        {
            var inventories = await _context.Inventories.ToListAsync();
            return Ok(inventories);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddInventory([FromBody] Inventory inventory)
        {
            _context.Inventories.Add(inventory);
            await _context.SaveChangesAsync();
            return Ok(inventory);
        }

        [Authorize(Roles = "Admin, Supervisor")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateInventory(int id, [FromBody] Inventory inventory)
        {
            var existingInventory = await _context.Inventories.FindAsync(id);
            if (existingInventory == null) return NotFound();

            existingInventory.ItemName = inventory.ItemName;
            existingInventory.ItemCount = inventory.ItemCount;
            existingInventory.StockInAt = inventory.StockInAt;
            await _context.SaveChangesAsync();
            return Ok(existingInventory);
        }

        [Authorize(Policy = "SupervisorPolicy")]
        [HttpPatch("{id}/verify")]
        public async Task<IActionResult> VerifyInventory(int id, [FromBody] string verificationState)
        {
            var inventory = await _context.Inventories.FindAsync(id);
            if (inventory == null) return NotFound();

            inventory.VerificationState = verificationState;
            inventory.VerifiedBy = User.Identity.Name;
            await _context.SaveChangesAsync();
            return Ok(inventory);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInventory(int id)
        {
            var inventory = await _context.Inventories.FindAsync(id);
            if (inventory == null) return NotFound();

            _context.Inventories.Remove(inventory);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }

}
