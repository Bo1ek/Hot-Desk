using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using SoftwareMind.Application.Common.Models;
using SoftwareMind.Infrastructure.Data;
using SoftwareMind.Infrastructure.Repositories;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace SoftwareMind.IntegrationTests.Infrastructure.Repositories
{
    public class ReservationRepositoryTests : IAsyncLifetime
    {
        private CustomWebApplicationFactory<Program> _webApplicationFactory;
        private HttpClient _client;
        private ApplicationDbContext _context;
        private ReservationRepository _sut;

        public async Task InitializeAsync()
        {
            _webApplicationFactory = new CustomWebApplicationFactory<Program>();
            _client = _webApplicationFactory.CreateClient();

            var scopeFactory = _webApplicationFactory.Services.GetService<IServiceScopeFactory>();
            using (var scope = scopeFactory.CreateScope())
            {
                _context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                _sut = new ReservationRepository(_context, new DeskRepository(_context));
            }

            await Task.CompletedTask;
        }

        public async Task DisposeAsync()
        {
            await _webApplicationFactory.DisposeAsync();
        }

        [Fact]
        public async Task BookDeskForOneDay_WhenCalledWithProperData_ShouldAddReservationForOneDay()
        {
            // Arrange
            var deskId = 1;
            var userId = "1";
            var startDate = DateTime.Now;

            // Act
            await _sut.BookDeskForOneDay(deskId, userId, startDate, CancellationToken.None);

            // Assert
            var addedReservation = await _context.Reservations.FirstOrDefaultAsync(r => r.DeskId == deskId && r.UserId == userId && r.StartDate == startDate);
            Assert.NotNull(addedReservation);
            Assert.Equal(deskId, addedReservation.DeskId);
            Assert.Equal(userId, addedReservation.UserId);
            Assert.Equal(startDate, addedReservation.StartDate);
        }
    }
}
