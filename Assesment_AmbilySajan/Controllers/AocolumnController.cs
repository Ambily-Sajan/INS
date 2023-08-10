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
        private readonly TableDbCOntext tableDbContext;
        public AocolumnController(TableDbCOntext tableDbContext)
        {
            this.tableDbContext = tableDbContext;
        }

        //Add a column for the AOTable record 
        [HttpPost]
        [Route("AddColumn")]
        public async Task<IActionResult> AddColumn([FromBody] Aocolumn aocolumn)
        {

            try
            {
                var aotable = tableDbContext.AOTable.Find(aocolumn.TableId);

                if (aotable == null)
                {
                    return NotFound("AOTable record not found");
                }
                aocolumn.Id = Guid.NewGuid();
                tableDbContext.AOColumn.Add(aocolumn);
                await tableDbContext.SaveChangesAsync();

                return Ok("Inserted Column!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        //Edit a Record of AOColumn
        [HttpPut("id")]

        public async Task<IActionResult> EditColumn([FromRoute] Guid id, [FromBody] Aocolumn aocolumn)
        {
            try
            {
                var column = await tableDbContext.AOColumn.FindAsync(id);
                if (column == null)
                {
                    return NotFound("No record found");
                }
                column.Id = id;
                column.TableId = aocolumn.TableId ?? column.TableId;
                column.Name = aocolumn.Name ?? column.Name;
                column.Type = aocolumn.Type ?? column.Type;
                column.Description = aocolumn.Description ?? column.Description;
                column.DataType = aocolumn.DataType ?? column.DataType;
                column.DataSize = aocolumn.DataSize ?? column.DataSize;
                column.DataScale = aocolumn.DataScale ?? column.DataScale;
                column.Comment = aocolumn.Comment ?? column.Comment;
                column.Encrypted = aocolumn.Encrypted ?? column.Encrypted;
                column.Distortion = aocolumn.Distortion ?? column.Distortion;

                await tableDbContext.SaveChangesAsync();
                return Ok(column);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        //Delete the Record From AOColumn
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteColumn([FromRoute] Guid id)
        {
            try
            {
                var columndelete = await tableDbContext.AOColumn.FindAsync(id);
                if (columndelete == null)
                {
                    return NotFound("Column Not Found");
                }
                tableDbContext.AOColumn.Remove(columndelete);
                await tableDbContext.SaveChangesAsync();
                return Ok("Deleted Successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        //Get All Records By DataType
        [HttpGet]
        public async Task<ActionResult<List<Aocolumn>>> GetAOColumnByDataType([FromQuery] List<string?> datatypes)
        {
            try
            {

                var records = await tableDbContext.AOColumn.Where(c => datatypes.Contains(c.DataType)).ToListAsync();

                return Ok(records);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        //Get All Records of AOColumn Based on Table Name
        [HttpGet("{name}")]
        public async Task<ActionResult<List<Aocolumn>>> GetColumnTable(string name)
        {
            try
            {
                var tables = await tableDbContext.AOTable.FirstOrDefaultAsync(n => n.Name == name);
                if (tables == null)
                {
                    return NotFound("Table Not found");
                }
                var column = tableDbContext.AOColumn.Where(c => c.TableId == tables.Id).ToList();
                if (column == null)
                {
                    return NotFound("No Records");
                }
                return Ok(column);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }



        }



    }
}
