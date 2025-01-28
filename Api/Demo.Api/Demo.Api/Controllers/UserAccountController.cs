using Demo.Api.Data;
using Demo.Api.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Demo.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAccountController : ControllerBase
    {
        private readonly DemoDbContext _demoDbContext;

        public UserAccountController(DemoDbContext demoDbContext) => _demoDbContext = demoDbContext;

        [HttpGet]
        public ActionResult<IEnumerable<UserAccount>> Get()
        {
            return _demoDbContext.UserAccounts;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserAccount?>> GetById(int id)
        {
            return await _demoDbContext.UserAccounts.Where(x => x.Id == id).SingleOrDefaultAsync();
        }

        [HttpPost]
        public async Task<ActionResult> Create(UserAccount userAccount)
        {
            await _demoDbContext.UserAccounts.AddAsync(userAccount);
            await _demoDbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = userAccount.Id }, userAccount);
        }

        [HttpPut]
        public async Task<ActionResult> Update(UserAccount userAccount)
        {
            _demoDbContext.UserAccounts.Update(userAccount);
            await _demoDbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var userAccountGetByIdResult = await GetById(id);
            if (userAccountGetByIdResult.Value is null)
                return NotFound();
            _demoDbContext.Remove(userAccountGetByIdResult.Value);
            await _demoDbContext.SaveChangesAsync();
            return Ok();
        }
    }
}
