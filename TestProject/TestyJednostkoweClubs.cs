using AutoMapper;
using KarateSystem.Dto;
using KarateSystem.Repository.Interfaces;
using KarateSystem.Service.Interfaces;
using KarateSystem.ViewModel;
using KarateSystem.Views;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TestProject
{
    public class TestyJednostkoweClubs
    {
        //private Mock<IClubRepository> _mockClubRepo;
        //private ClubsDegreesMatsViewModel _viewModel;

        //private ClubDto _editingClub;
        //private ClubDto _selectedClub;

        //[SetUp]
        //public void Setup()
        //{
        //    _mockClubRepo = new Mock<IClubRepository>();
        //    _viewModel = new ClubsDegreesMatsViewModel(_mockClubRepo.Object, null, null, null, null);

        //    _editingClub = new ClubDto
        //    {
        //        ClubId = 1,
        //        ClubName = "Nowy Klub",
        //        ClubPlace = "Nowe Miasto"
        //    };

        //    _selectedClub = new ClubDto
        //    {
        //        ClubId = 1,
        //        ClubName = "Stary Klub",
        //        ClubPlace = "Stare Miasto"
        //    };

        //    _viewModel.EditingClub = _editingClub;
        //    _viewModel.SelectedClub = _selectedClub;
        //}

        //[Test]
        //public async Task ExecuteUpdateClubCommand_ValidData_UpdatesClubAndReloadsList()
        //{
        //    // Arrange
        //    var originalClub = new ClubDto { ClubId = 1, ClubName = "Stara Nazwa", ClubPlace = "Stare Miejsce" };
        //    var editedClub = new ClubDto { ClubId = 1, ClubName = "Nowa Nazwa", ClubPlace = "Nowe Miejsce" };
        //    var updatedList = new List<ClubDto> { originalClub };

        //    _viewModel.SelectedClub = originalClub; 
        //    _viewModel.EditingClub = editedClub;

        //    _mockClubRepo
        //        .Setup(r => r.UpdateClubAsync(It.IsAny<ClubDto>()))
        //        .Returns(Task.CompletedTask);

        //    _mockClubRepo
        //        .Setup(r => r.GetAllClubsAsync())
        //        .ReturnsAsync(updatedList);

        //    // Act
        //    await _viewModel.ExecuteUpdateClubCommand(null);

        //    // Assert
        //    _mockClubRepo.Verify(r => r.UpdateClubAsync(It.Is<ClubDto>(c =>
        //        c.ClubId == 1 &&
        //        c.ClubName == "Nowa Nazwa" && 
        //        c.ClubPlace == "Nowe Miejsce"
        //    )), Times.Once);

        //    Assert.That(_viewModel.Clubs.Count, Is.EqualTo(1));
        //    Assert.That(_viewModel.Clubs[0].ClubName, Is.EqualTo("Nowa Nazwa"));
        //}

    }
}
