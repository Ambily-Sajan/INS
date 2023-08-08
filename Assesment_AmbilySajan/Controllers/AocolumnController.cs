using Assesment_AmbilySajan.Data;
using Assesment_AmbilySajan.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Assesment_AmbilySajan.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AocolumnController : ControllerBase
    {
        private readonly TableDbCOntext columnDbContext;
        public AocolumnController(TableDbCOntext columnDbContext)
        {
            this.columnDbContext = columnDbContext;
        }

        //Add a column for the AOTable record 
        [HttpPost]
        [Route("AddColumn")]
        public async Task<IActionResult> AddColumn(Aocolumn aocolumn)
        {
            
            try
            {
                var aotable = columnDbContext.AOTable.Find(aocolumn.TableId);

                if (aotable == null)
                {
                    return NotFound("AOTable record not found");
                }
                aocolumn.Id = Guid.NewGuid();
                columnDbContext.AOColumn.Add(aocolumn);
                await columnDbContext.SaveChangesAsync();

                return Ok("Inserted Column!");
            }
            catch
            {

                return BadRequest("Exception Caught on Adding Column");
            }

        }

        //Edit a Record of AOColumn
       [HttpPut("id")]
        
        public async Task<IActionResult> EditColumn(Guid id, [FromBody] Aocolumn updatecolumn)
        {
            try
            {
                var exist = await columnDbContext.AOColumn.FindAsync(id);
                if (exist == null)
                {
                    return NotFound("No record found");
                }
                exist.Id = id;
                exist.TableId = updatecolumn.TableId ?? exist.TableId;
                exist.Name = updatecolumn.Name ?? exist.Name;
                exist.Type = updatecolumn.Type ?? exist.Type;
                exist.Description = updatecolumn.Description ?? exist.Description;
                exist.DataType = updatecolumn.DataType ?? exist.DataType;
                exist.DataSize = updatecolumn.DataSize ?? exist.DataSize;
                exist.DataScale = updatecolumn.DataScale ?? exist.DataScale;
                exist.Comment = updatecolumn.Comment ?? exist.Comment;
                exist.Encrypted = updatecolumn.Encrypted ?? exist.Encrypted;
                exist.Distortion = updatecolumn.Distortion ?? exist.Distortion;

                await columnDbContext.SaveChangesAsync();
                return Ok(exist);
            }
            catch
            {
                return BadRequest("Exception Caught while updating column records");
            }
        }

        //Delete the Record From AOColumn
        [HttpDelete("id")]
        public async Task<IActionResult> DeleteColumn(Guid id)
        {
            try
            {
                var coldel = await columnDbContext.AOColumn.FindAsync(id);
                if (coldel == null)
                {
                    return NotFound("Column Not Found");
                }
                columnDbContext.AOColumn.Remove(coldel);
                await columnDbContext.SaveChangesAsync();
                return Ok("Deleted Successfully");
            }
            catch
            {
                return BadRequest("Exception Caught While Deleting Column");
            }
        }

        //Get records of DataType 
        [HttpGet]
        public async Task<ActionResult<Aocolumn>> GetDataType([FromQuery] List<string?> datatypes)
        {
            try
            {
                var tb = await columnDbContext.AOColumn.Where(c => datatypes.Contains(c.DataType)).ToListAsync();
                return Ok(tb);
            }
            catch
            {
                return BadRequest("Exception Caught During Retrieval of DataType");
            }
        }

        //Get All Records of AOColumn Based on Table Name
        [HttpGet("{name}")]
        public async Task<ActionResult<List<Aocolumn>>> GetColumnTable(string name)
        {
            try
            {
                var tables = await columnDbContext.AOTable.FirstOrDefaultAsync(n => n.Name == name);
                if (tables == null)
                {
                    return NotFound("Table Not found");
                }
                var column = columnDbContext.AOColumn.Where(c => c.TableId == tables.Id).ToList();
                if (column == null)
                {
                    return NotFound("No rcds");
                }
                return Ok(column);
            }
            catch
            {
                return BadRequest("Exception Caught on Retrieving Column based on Table Name");
            }



        }

        

    }
}
