using System.Reflection.Metadata.Ecma335;
using AppointmentScheduler.Data;
using Microsoft.EntityFrameworkCore;

namespace AppointmentScheduler.Services
{
    public class TestService
    {
        private readonly AppDbContext dbContext;

        public TestService(AppDbContext context)
        {
            dbContext = context;
        }

        public async Task<List<TestItem>> GetTestItemsAsync()
        {
            return await dbContext.TestItems.ToListAsync();
        }

        public async Task<TestItem> GetTestItemAsync(Guid id)
        {
            return await dbContext.TestItems.FindAsync(id);
        }

        public async Task<Guid> CreateTestItemAsync(TestItem testItem)
        {
            dbContext.TestItems.Add(testItem);
            await dbContext.SaveChangesAsync();

            return testItem.Id;
        }
    }
}