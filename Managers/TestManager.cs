using AppointmentScheduler.Data;
using AppointmentScheduler.Services;

namespace AppointmentScheduler.Managers
{
    public class TestManager
    {
        private readonly TestService testService;   

        public TestManager(TestService service)
        {
            testService = service; 
        }

        public async Task<List<TestItem>> GetTestItemsAsync()
        {
            return await testService.GetTestItemsAsync();
        }

        public async Task<Guid> CreateTestItemAsync(TestItem testItem)
        {
            return await testService.CreateTestItemAsync(testItem);
        }

        public async Task<TestItem> GetTestItemAsync(Guid id)
        {
            return await testService.GetTestItemAsync(id);
        }
    }
}