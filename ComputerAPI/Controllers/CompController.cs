using ComputerAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ComputerAPI.Controllers
{
    [Route("comp")]
    [ApiController]
    public class CompController : ControllerBase
    {

        private readonly ComputerContext computerContext;

        public CompController(ComputerContext computerContext)
        {
            this.computerContext = computerContext;
        }
        [HttpPost]
        public async Task<ActionResult<Comp>> Post(CreateCompDto createCompDto)
        {
            var comp = new Comp
            {
                Id = Guid.NewGuid(),
                Brand = createCompDto.Brand,
                Type = createCompDto.Type,
                Display = createCompDto.Display,
                Memory = createCompDto.Memory,
                CreatedTime = DateTime.Now,
                OsId = createCompDto.OsId,
            };
            if (comp != null)
            {
                await computerContext.Comps.AddAsync(comp);
                await computerContext.SaveChangesAsync();
                return StatusCode(201, comp);
            }
            return BadRequest();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult> GetAllCompByID(Guid id)
        {
            var os = computerContext.Comps.Include(os => os.Id).Where(c => c.Id == id);

            if (os != null)
            {
                return Ok(os);
            }
            return BadRequest();
        }
        [HttpDelete("{id}")]

        public async Task<ActionResult> Delete(Guid id)
        {
            var comp = await computerContext.Comps.FirstOrDefaultAsync(eos => eos.Id == id);

            if (comp != null)
            {
                computerContext.Comps.Remove(comp);
                await computerContext.SaveChangesAsync();
                return Ok(new { message = "Sikeres törlés." });

            }
            return NotFound(new { message = "Nincs ilyen." });
        }
    }
}
