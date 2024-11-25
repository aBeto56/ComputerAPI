using ComputerAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ComputerAPI.Controllers
{
    [Route("osystem")]
    [ApiController]
    public class OsystemController : ControllerBase
    {
        private readonly ComputerContext computerContext;

        public OsystemController(ComputerContext computerContext)
        {
            this.computerContext = computerContext;
        }

        [HttpPost]
        public async Task<ActionResult<Osystem>> Post(CreateOsDto createOsDto)
        {
            var os = new Osystem
            {
                Id = Guid.NewGuid(),
                Name = createOsDto.Name,
            };

            if (os != null)
            {
                await computerContext.Osystems.AddAsync(os);
                await computerContext.SaveChangesAsync();
                return StatusCode(201, os);
            }

            return BadRequest();
        }
        [HttpGet]

        public async Task<ActionResult<Osystem>> Get()
        {
            return Ok(await computerContext.Osystems.ToListAsync());
        }
        [HttpGet("{id}")]

        public async Task<ActionResult<Osystem>> GetById(Guid id)
        {
            var os = computerContext.Osystems.FirstOrDefaultAsync(o => o.Id == id);

            if (os != null)
            {
                return Ok(os);
            }
            return NotFound(new { message = "Nincs ilyen" });
        }
        [HttpPut]

        public async Task<ActionResult<Osystem>> Put(Guid id , UpdateOsDto updateOsDto)
        {
            var existingOs = await computerContext.Osystems.FirstOrDefaultAsync(eos => eos.Id == id);

            if (existingOs != null)
            {
                existingOs.Name = updateOsDto.Name;
                computerContext.Osystems.Update(existingOs);
                await computerContext.SaveChangesAsync();
                return Ok(existingOs);
            }
            return NotFound(new { message = "Nincs ilyen" });
        }
    } 
}
