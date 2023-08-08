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
        public async Task<IActionResult> AddNewTable([FromBody]Aotables table)
        {
            try
            {
                table.Id = Guid.NewGuid();
                await tableDbContext.AOTable.AddAsync(table);
                await tableDbContext.SaveChangesAsync();
                return Ok(table);
            }
            catch {
                return BadRequest("Error Found,Exception Caught While Adding");
            }
        }

        // Update a record in AOTables by Id
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAotable(Guid id, [FromBody] Aotables table)
        {
            

            try
            {
                var existingAotable = await tableDbContext.AOTable.FindAsync(id);

                if (existingAotable == null)
                {
                    return NotFound("Record not found");
                }

                // Updating values
                existingAotable.Name = table.Name;
                existingAotable.Type = table.Type;
                existingAotable.Description = table.Description;
                existingAotable.Comment = table.Comment;
                existingAotable.History = table.History;
                existingAotable.Boundary = table.Boundary;
                existingAotable.Log = table.Log;
                existingAotable.Cache = table.Cache;
                existingAotable.Notify = table.Notify;
                existingAotable.Identifier = table.Identifier;

                tableDbContext.Entry(existingAotable).State = EntityState.Modified;
                await tableDbContext.SaveChangesAsync();

                return Ok(existingAotable);
            }
            catch
            {
                return BadRequest("Exception caught While updating");
            }
        }

        //Get All Records of Type Schedule or policy
        [HttpGet]
        [Route("GetSchedulePolicy")]
        public async Task<ActionResult<List<Aotables>>> GetSchedulePolicy()
        {
            try
            {
                var rcds = await tableDbContext.AOTable.Where(t => t.Type == "schedule" || t.Type == "policy").ToListAsync();
                return Ok(rcds);
            }
            catch
            {
                return BadRequest("Exception Caught on Retrieval");
            }
            
            
        }
    }
}







    

