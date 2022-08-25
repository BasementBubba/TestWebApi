using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Database;
using WebApplication1.Database.Models;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivityController : ControllerBase
    {
        private BaseContext context;
        public ActivityController(BaseContext _context)
        {
            context = _context; 
        }
        [Route("list")]
        [HttpGet]
        public IActionResult GetActivities()
        {
            try
            {
                return Ok(context.Activities.AsNoTracking().ToList());
            }
            catch (Exception e)
            {
                //знаю, что ошибки нужно выводить в логи, но мне не хочется их настраивать,
                //да и для тестового задания лучше буду видеть ошибки на экране
                return BadRequest(e.Message);
            }
        }

        [Route("create")]
        [HttpPost]
        public async Task<IActionResult> CreateActivity([FromBody] FullActivityModel model)
        {
            try
            {
                var act = await context.Activities.AddAsync(new Activity() { Name = model.Activity.ActivityName, CreationDate = DateTime.UtcNow });
                if (model.Files != null && model.Files.Count() != 0)
                {
                    foreach (var file in model.Files)
                    {
                        await context.ActivityFiles.AddAsync(new ActivityFile()
                        {
                            ActivityID = act.Entity,
                            Data = Convert.FromBase64String(file.Data),
                            FileExtension = file.FileExtension,
                            Name = file.FileName
                        });
                    }
                }
                context.SaveChanges();
                return Ok(act.Entity.Id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Route("update/{id:long}")]
        [HttpPost]
        public IActionResult UpdateActivity([FromBody] ActivityModel model)
        {
            try
            {
                var activity = context.Activities.Where(n => n.Id == Convert.ToInt64(Request.RouteValues["id"]))
                    .FirstOrDefault();
                if(activity != null)
                {
                    activity.Name = model.ActivityName;
                    if(activity.IsCompleted == false && model.IsCompleted == true)
                    {
                        activity.CompleteTime = DateTime.UtcNow;
                    }
                    activity.IsCompleted = model.IsCompleted;
                }
                context.SaveChanges();
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [Route("delete/{id:long}")]
        [HttpGet]
        public IActionResult DeleteActivity()
        {
            try
            {
                var activity = context.Activities.Where(n => n.Id == Convert.ToInt64(Request.RouteValues["id"]))
                    .FirstOrDefault();
                if (activity != null)
                {
                    context.Activities.Remove(activity);
                }
                else return NotFound();
                context.SaveChanges();
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [Route("{id:long}")]
        [HttpGet]
        public IActionResult GetActivity()
        {
            try
            {
                var activity = context.Activities.AsNoTracking()
                    .Where(n => n.Id == Convert.ToInt64(Request.RouteValues["id"]))
                    .FirstOrDefault();
                if (activity != null)
                {
                    return Ok(activity);
                }
                else return NotFound();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
