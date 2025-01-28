using Demo.Api.Data;
using Demo.Api.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Demo.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskToDoController : ControllerBase
    {
        private readonly DemoDbContext _demoDbContext;

        public TaskToDoController(DemoDbContext demoDbContext) => _demoDbContext = demoDbContext;

        [HttpGet]
        public ActionResult<IEnumerable<TaskToDo>> Get()
        {
            return _demoDbContext.TaskToDos;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TaskToDo?>> GetById(int id)
        {
            return await _demoDbContext.TaskToDos.Where(x => x.Id == id).SingleOrDefaultAsync();
        }

        [HttpPost]
        public async Task<ActionResult> Create(TaskToDo taskToDo)
        {
            await _demoDbContext.TaskToDos.AddAsync(taskToDo);
            await _demoDbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = taskToDo.Id }, taskToDo);
        }

        [HttpPut]
        public async Task<ActionResult> Update(TaskToDo taskToDo)
        {
            _demoDbContext.TaskToDos.Update(taskToDo);
            await _demoDbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var taskToDoGetByIdResult = await GetById(id);
            if (taskToDoGetByIdResult.Value is null)
                return NotFound();
            _demoDbContext.Remove(taskToDoGetByIdResult.Value);
            await _demoDbContext.SaveChangesAsync();
            return Ok();
        }
    }
}
