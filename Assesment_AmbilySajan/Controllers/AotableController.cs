using Assesment_AmbilySajan.Data;
using Assesment_AmbilySajan.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.ComponentModel;
using System.Reflection.PortableExecutable;

namespace Assesment_AmbilySajan.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AotableController : ControllerBase
    {
        private readonly TableDbCOntext tableDbContext;
        public AotableController(TableDbCOntext tableDbContext)
        {
            this.tableDbContext = tableDbContext;
        }


        // Add a new record to AOTable
        [HttpPost]
        [Route("AddRecord")]
        public async Task<IActionResult> AddNewTable([FromBody] Aotables table)
        {
            try
            {
                table.Id = Guid.NewGuid();
                await tableDbContext.AOTable.AddAsync(table);
                await tableDbContext.SaveChangesAsync();
                return Ok(table);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // Update a record in AOTables by Id
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAotable([FromRoute] Guid id, [FromBody] Aotables updatetable)
        {


            try
            {
                var existingAotable = await tableDbContext.AOTable.FindAsync(id);

                if (existingAotable == null)
                {
                    return NotFound("Record not found");
                }

                // Updating values
                existingAotable.Name = updatetable.Name ?? existingAotable.Name;
                existingAotable.Type = updatetable.Type ?? existingAotable.Type;
                existingAotable.Description = updatetable.Description ?? existingAotable.Description;
                existingAotable.Comment = updatetable.Comment ?? existingAotable.Comment;
                existingAotable.History = updatetable.History ?? existingAotable.History;
                existingAotable.Boundary = updatetable.Boundary ?? existingAotable.Boundary;
                existingAotable.Log = updatetable.Log ?? existingAotable.Log;
                existingAotable.Cache = updatetable.Cache ?? existingAotable.Cache;
                existingAotable.Notify = updatetable.Notify ?? existingAotable.Notify;
                existingAotable.Identifier = updatetable.Identifier ?? existingAotable.Identifier;

                tableDbContext.Entry(existingAotable).State = EntityState.Modified;
                await tableDbContext.SaveChangesAsync();

                return Ok(existingAotable);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        //Get All Records of Type Schedule or policy
        [HttpGet]
        [Route("GetAOTableType")]
        public async Task<ActionResult<List<Aotables>>> GetAOTableType()
        {
            try
            {
                var records = await tableDbContext.AOTable.Where(t => t.Type == "schedule" || t.Type == "policy").ToListAsync();
                return Ok(records);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }


        }
    }
}









