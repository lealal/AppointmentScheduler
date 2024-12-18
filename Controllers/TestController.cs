using AppointmentScheduler.Data;
using AppointmentScheduler.Managers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentScheduler.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("api/test")]
    public class TestController : ControllerBase
    {
        private readonly TestManager testManager; 

        public TestController(TestManager manager)
        {
            testManager = manager;
        }
        
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Service is alive");
        }

        [HttpPost("testitem")]
        public async Task<IActionResult> CreateTestItem([FromBody] TestItem testItem)
        {
            if (testItem == null)
            {
                return BadRequest("TestItem is null");
            }

            var id = await testManager.CreateTestItemAsync(testItem);
            return Ok(id);
        }

        [HttpGet("testitem/{id}")]
        public async Task<IActionResult> GetTestItem(Guid id)
        {
            var testItem = await testManager.GetTestItemAsync(id);

            if (testItem == null)
            {
                return NotFound();
            }

            return Ok(testItem);
        }

        [HttpGet("testitems")]
        public async Task<IActionResult> GetTestItems()
        {
            var testItems = await testManager.GetTestItemsAsync();
            return Ok(testItems);
        }
    }
}
