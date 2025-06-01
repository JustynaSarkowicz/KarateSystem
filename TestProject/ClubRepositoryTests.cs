using KarateSystem.Models;
using KarateSystem.Dto;
using KarateSystem.Repository;
using KarateSystem.Configurations;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using AutoMapper;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace TestProject
{
    public class ClubRepositoryTests
    {
        private ApplicationDbContext _dbContext;
        private ClubRepository _repo;
        private IMapper _mapper;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            _dbContext = new ApplicationDbContext(options);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Club, ClubDto>().ReverseMap();
            });

            _mapper = config.CreateMapper();
            _repo = new ClubRepository(_dbContext, _mapper);
        }

        [TearDown]
        public void TearDown()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }

        [Test]
        public async Task AddClubAsync_ShouldAddClub()
        {
            var clubDto = new ClubDto { ClubId = 1, ClubName = "Test Club", ClubPlace = "Test" };

            await _repo.AddClubAsync(clubDto);

            var result = await _dbContext.Clubs.FirstOrDefaultAsync(c => c.ClubId == 1);
            Assert.NotNull(result);
            Assert.That(result.ClubName, Is.EqualTo("Test Club"));
        }

        [Test]
        public async Task GetAllClubsAsync_ShouldReturnAllClubs()
        {
            _dbContext.Clubs.Add(new Club { ClubId = 1, ClubName = "Club A", ClubPlace = "Test A" });
            _dbContext.Clubs.Add(new Club { ClubId = 2, ClubName = "Club B", ClubPlace = "Test B" });
            await _dbContext.SaveChangesAsync();

            var clubs = await _repo.GetAllClubsAsync();

            Assert.That(clubs.Count, Is.EqualTo(2));
            Assert.That(clubs.Any(c => c.ClubName == "Club A"));
        }

        [Test]
        public async Task GetClubAsync_ShouldReturnCorrectClub()
        {
            _dbContext.Clubs.Add(new Club { ClubId = 5, ClubName = "Special Club", ClubPlace = "Test" });
            await _dbContext.SaveChangesAsync();

            var club = await _repo.GetClubAsync(5);

            Assert.NotNull(club);
            Assert.That(club.ClubName, Is.EqualTo("Special Club"));
        }

        [Test]
        public async Task UpdateClubAsync_ShouldUpdateClub()
        {
            _dbContext.Clubs.Add(new Club { ClubId = 1, ClubName = "Old Name", ClubPlace = "Test" });
            await _dbContext.SaveChangesAsync();

            var updatedDto = new ClubDto { ClubId = 1, ClubName = "New Name", ClubPlace = "Test" };
            await _repo.UpdateClubAsync(updatedDto);

            var updatedClub = await _dbContext.Clubs.FindAsync(1);
            Assert.That(updatedClub.ClubName, Is.EqualTo("New Name"));
        }

        [Test]
        public async Task DeleteClubAsync_ShouldDeleteClub()
        {
            _dbContext.Clubs.Add(new Club { ClubId = 1, ClubName = "To Be Deleted", ClubPlace = "Test" });
            await _dbContext.SaveChangesAsync();

            await _repo.DeleteClubAsync(1);

            var club = await _dbContext.Clubs.FindAsync(1);
            Assert.Null(club);
        }

        [Test]
        public void DeleteClubAsync_ShouldThrow_WhenClubHasCompetitors()
        {
            _dbContext.Clubs.Add(new Club { ClubId = 1, ClubName = "WithCompetitors", ClubPlace = "Test" });
            _dbContext.Competitors.Add(new Competitor { CompId = 1, CompClubId = 1, CompFirstName = "John", CompLastName = "Potter", CompGender = true, CompDateOfBirth = new DateTime(2000, 9, 23), CompDegreeId = 1, CompWeight = 60});
            _dbContext.SaveChanges();

            Assert.ThrowsAsync<System.Exception>(async () =>
            {
                await _repo.DeleteClubAsync(1);
            });
        }
    }
}
