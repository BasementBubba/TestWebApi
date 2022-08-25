using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Database;
using WebApplication1.Database.Models;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private BaseContext context;
        public FilesController(BaseContext _context)
        {
            context = _context;
        }
        [Route("list")]
        [HttpGet]
        public IActionResult GetFileList()
        {
            try
            {
                return Ok(context.ActivityFiles.Select(n => n.Name + n.FileExtension).ToList());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [Route("{id:long}")]
        [HttpGet]
        public IActionResult GetFile()
        {
            try
            {
                var id = Convert.ToInt64(Request.RouteValues["id"]);
                var file = context.ActivityFiles.Where(n => n.Id == id).FirstOrDefault();
                if (file != null)
                {
                    return Ok(file);
                }
                else return BadRequest("There is no file with such an Id");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [Route("add/{id:long}")]
        [HttpPost]
        public async Task<IActionResult> AddFiles([FromBody] IEnumerable<ActivityFileModel> files) 
        {
            try
            {
                long id = Convert.ToInt64(Request.RouteValues["id"]);
                var activity = context.Activities.Where(n => n.Id == id).FirstOrDefault();
                if (activity != null) {
                    foreach (var file in files)
                    {
                     await context.ActivityFiles.AddAsync(new ActivityFile()
                        {
                            ActivityID = activity,
                            Data = Convert.FromBase64String(file.Data),
                            Name = file.FileName,
                            FileExtension = file.FileExtension
                        });
                    }
                    await context.SaveChangesAsync();
                    return Ok();
                }
                else return NotFound();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [Route("delete/{id:long}")]
        [Route("delete/all/{activity_id:long}")]
        [HttpGet]
        public async Task<IActionResult> DeleteFiles()
        {
            try
            {
                long id;
                bool delete_all = false;
                if (Request.RouteValues["id"] != null)
                {
                    id = Convert.ToInt64(Request.RouteValues["id"]);
                }
                else if (Request.RouteValues["activity_id"] != null)
                {
                    id = Convert.ToInt64(Request.RouteValues["activity_id"]);
                    delete_all = true;
                }
                else return BadRequest();
                if(delete_all)
                {
                    var files = context.ActivityFiles.Where(n => n.ActivityID.Id == id).ToList();
                    if (files != null && files.Count > 0)
                    {
                        context.ActivityFiles.RemoveRange(files);
                    }
                    else return BadRequest("There is no such an activity or no files");
                }
                else
                {
                    var file = context.ActivityFiles.FirstOrDefault(n => n.Id == id);
                    if (file != null)
                    {
                        context.ActivityFiles.Remove(file);
                    }
                    else return BadRequest("There is no activity with such an ID");
                }
                await context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [Route("update/{id:long}")]
        [HttpPost]
        public async Task<IActionResult> UpdateFile([FromBody] ActivityFileModel model)
        {
            try
            {
                long id = Convert.ToInt64(Request.RouteValues["id"]);
                var file = context.ActivityFiles.Where(n => n.Id == id).FirstOrDefault();
                if(file != null)
                {
                    file.Name = model.FileName;
                    file.FileExtension = model.FileExtension;
                    file.Data = Convert.FromBase64String(model.Data);
                }
                await context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
