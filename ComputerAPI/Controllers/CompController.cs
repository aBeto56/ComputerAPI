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
        [HttpGet("allComputerWithOs")]
        public async Task<ActionResult<Comp>> GetAllComputerWithOs()
        {
            try
            {
                var allcomputer = await computerContext.Comps.Select(cmp => new { cmp.Brand, cmp.Type, cmp.Memory, cmp.Os.Name }).ToListAsync();
                if (allcomputer != null)
                {
                    return Ok(new { message = "Sikeres lekérdezés.", result = allcomputer });
                }
                return NotFound(new { message = "Nincs eredmény.", result = allcomputer });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = "Sikertelen lekérdezés.", result = e.Message });
            }
        }

        [HttpGet("allMicrosoftOs")]
        public async Task<ActionResult<Comp>> GetAllMicrosoftOs()
        {
            var microOs = await computerContext.Comps.Where(cmp => cmp.Os.Name.Contains("Microsoft")).Select(cmp => new { cmp.Brand, cmp.Type, cmp.Memory, cmp.Os.Name }).ToListAsync();
            return Ok(microOs);
        }

        [HttpGet("maxDisplaySize")]
        public async Task<ActionResult<Comp>> GetMaxDiplaySize()
        {
            var maxSize = await computerContext.Comps.MaxAsync(cmp => cmp.Display);
            var maxSizeComputer = await computerContext.Comps.Where(cmp => cmp.Display == maxSize).Select(cmp => new { cmp.Brand, cmp.Type, cmp.Display, cmp.Os.Name }).ToListAsync();
            return Ok(maxSizeComputer);
        }

        [HttpGet("newComputer")]
        public async Task<ActionResult<Comp>> GetNewComputer()
        {
            var newComputerDate = await computerContext.Comps.MaxAsync(cmp => cmp.CreatedTime);
            var newComputer = await computerContext.Comps.Where(cmp => cmp.CreatedTime == newComputerDate).Select(cmp => new { cmp, cmp.Os.Name }).ToListAsync();
            return Ok(newComputer);
        }
        [HttpGet("allLinuxOs")]
        public async Task<ActionResult<Comp>> GetAllLinuxOs()
        {
            var LinOs = await computerContext.Comps.Where(cmp => cmp.Os.Name.Contains("Linux")).Select(cmp => new { cmp.Brand, cmp.Type, cmp.Memory, cmp.Os.Name }).ToListAsync();
            return Ok(LinOs);
        }
    }
}
