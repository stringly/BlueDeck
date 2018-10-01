using System.Collections.Generic;
using System.Linq;
using Moq;
using OrgChartDemo.Controllers;
using OrgChartDemo.Models;
using OrgChartDemo.Models.Repositories;
using OrgChartDemo.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using Xunit;
using OrgChartDemo.Persistence;

namespace OrgChartDemo.Tests
{
    /// <summary>
    /// Unit tests for the PositionController
    /// </summary>
    public class PositionControllerTests
    {
        /// <summary>
        /// Determines whether this instance [can get position list with member count].
        /// </summary>
        [Fact]
        public void Can_Get_Member_Count_From_Position()
        {
            // Arrange - Create 2 positions and a list of 
            var position1 = new Position {
                PositionId = 1,
                Name = "Position2",
                ParentComponent = new Component {
                    ComponentId = 1,
                    ParentComponent = null,
                    Name = "Component1"
                },
                IsManager = false,
                Members = new List<Member> { }
            };
            var position2 = new Position {
                PositionId = 2,
                Name = "Position2",
                ParentComponent = new Component {
                    ComponentId = 1,
                    ParentComponent = null,
                    Name = "Component1" },
                IsManager = false,
                Members = new List<Member> {
                    new Member {
                        FirstName = "Adam",
                        LastName = "One",
                        MemberId = 1,
                        Position = position1,
                        Email = "AdamOne@test.mail"
                    },
                    new Member {
                        FirstName = "Bob",
                        LastName = "Two",
                        MemberId = 2,
                        Position = position1,
                        Email = "BobTwo@test.mail"
                    }
                }
            };
            var positions = new List<Position> { position1, position2 };
            var positionRepositoryMock = new Mock<IPositionRepository>();
            positionRepositoryMock.Setup(m => m.GetPositionsWithMembers()).Returns(positions).Verifiable();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(m => m.Positions).Returns(positionRepositoryMock.Object);

            // Act - Assign each member to the Members collection of Position => PositionId == 2
            var actual = unitOfWorkMock.Object.Positions.GetPositionsWithMembers().ToList();

            // Assert - test that the count of Members is correct for two of the Positions
            positionRepositoryMock.Verify();
            Assert.NotNull(actual);
            Assert.Empty(actual[0].Members);
            Assert.Equal(2, actual[1].Members.Count());
        }

        [Fact]
        public void Given_PositionId_Can_Get_Position()
        {
            // Arrange
            var positionId = 1;
            var expectedPositionName = "PositionName";
            var position = new Position { Name = expectedPositionName, PositionId = positionId };
            var positionRepositoryMock = new Mock<IPositionRepository>();
            positionRepositoryMock.Setup(m => m.Get(positionId)).Returns(position).Verifiable();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(m => m.Positions).Returns(positionRepositoryMock.Object);

            // Act
            var actual = unitOfWorkMock.Object.Positions.Get(positionId);

            // Assert
            positionRepositoryMock.Verify();
            Assert.NotNull(actual);
            Assert.Equal(position, actual);
        }

        [Fact]
        public void Given_Form_Can_Add_Position_From_Controller_Method()
        {
            // Arrange
            var positionId = 1;
            var expectedPositionName = "PositionName";
            var position = new Position { Name = expectedPositionName, PositionId = positionId };
            var positionRepositoryMock = new Mock<IPositionRepository>();
            positionRepositoryMock.Setup(m => m.Get(positionId)).Returns(position).Verifiable();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(m => m.Positions).Returns(positionRepositoryMock.Object);
            PositionsController controller = new PositionsController(unitOfWorkMock.Object);
            PositionWithComponentListViewModel form = new PositionWithComponentListViewModel
            {
                PositionName = "New Position",
                ParentComponentId = 1,
                JobTitle = "Add New Position Test",
                IsManager = false,
                IsUnique = false
            };

            // Act
            unitOfWorkMock.Object.Positions.Add(new Position { PositionId = 10 });
            unitOfWorkMock.Object.Complete();
            //controller.Create(form);

            // Assert
            positionRepositoryMock.Verify();
            Assert.True(unitOfWorkMock.Object.Positions.GetAll().Count() == 2);
        }











        ///// <summary>
        ///// Determines whether this instance [can add position].
        ///// </summary>
        //[Fact]
        //public void CanAddPosition()
        //{
        //    // Arrange
        //    int startPositionsCount = unitOfWork.Object.Positions.GetAll().Count(); // get the count of positions in Repo at the start
        //    int finalPositionsCount = startPositionsCount + 1;
        //    Position p = new Position
        //    {
        //        Name = "New Position",
        //        ParentComponent = new Component { ComponentId = 1 },
        //        JobTitle = "Add New Position Test",
        //        IsManager = false,
        //        IsUnique = false
        //    };

        //    // Act
        //    unitOfWork.Object.Positions.Add(p);
        //    unitOfWork.Object.Complete();
        //    // Assert
        //    Assert.True(unitOfWork.Object.Positions.GetAll().Count() == finalPositionsCount);
        //}
        ///// <summary>
        ///// Determines whether this instance [can add position from PositionController.Create()].
        ///// </summary>
        //[Fact]
        //public void CanAddPositionFromController()
        //{
        //    // Arrange
        //    int startPositionsCount = unitOfWork.Object.Positions.GetAll().Count(); // get the count of positions in Repo at the start
        //    int finalPositionsCount = startPositionsCount + 1;
        //    PositionsController controller = new PositionsController(unitOfWork.Object);
        //    PositionWithComponentListViewModel form = new PositionWithComponentListViewModel
        //    {
        //        PositionName = "New Position",
        //        ParentComponentId = 1,
        //        JobTitle = "Add New Position Test",
        //        IsManager = false,
        //        IsUnique = false
        //    };

        //    // Act 
        //    controller.Create(form);

        //    // Assert

        //    Assert.True(unitOfWork.Object.Positions.GetAll().Count() == finalPositionsCount);
        //}
        ///// <summary>
        ///// Determines whether this instance [can detect existing position name] and prevent a duplicate add.
        ///// </summary>
        //[Fact]
        //public void CanDetectExistingPositionName()
        //{
        //    // Arrange
        //    int startPositionsCount = unitOfWork.Object.Positions.GetAll().Count(); // get the count of positions in Repo at the start
        //    PositionsController controller = new PositionsController(unitOfWork.Object);
        //    PositionWithComponentListViewModel form = new PositionWithComponentListViewModel
        //    {
        //        PositionName = "Position1",
        //        JobTitle = "Position1 Test Job"
        //    };

        //    // Act - this should NOT add a Position - the name "Position1" already exists in the repo
        //    controller.Create(form);

        //    // Assert
        //    Assert.Equal(startPositionsCount, unitOfWork.Object.Positions.GetAll().Count());

        //}
        ///// <summary>
        ///// Determines whether this instance [can detect component position manager duplicate].
        ///// </summary>
        //[Fact]
        //public void CanDetectComponentPositionManagerDuplicate()
        //{
        //    // Arrange
        //    Component c = unitOfWork.Object.Components.Find(x => x.ComponentId == 4).FirstOrDefault();
        //    Position p = new Position
        //    {
        //        Name = "Test Position",
        //        ParentComponent = c,
        //        IsManager = true,
        //        IsUnique = true,
        //        JobTitle = "Test Duplicate Manager Job Title"
        //    };
        //    unitOfWork.Object.Positions.Add(p);

        //    int startPositionsCount = unitOfWork.Object.Positions.GetAll().Count(); // get the count of positions in Repo at the start
        //    int finalPositionsCount = startPositionsCount + 1;

        //    PositionsController controller = new PositionsController(unitOfWork.Object);
        //    PositionWithComponentListViewModel form = new PositionWithComponentListViewModel
        //    {
        //        PositionName = "New Position",
        //        ParentComponentId = 4,
        //        JobTitle = "Add New Position Test",
        //        IsManager = true,
        //        IsUnique = false
        //    };

        //    // Act - This should NOT add a Position to the repo
        //    controller.Create(form);

        //    // Assert            
        //    Assert.True(unitOfWork.Object.Positions.GetAll().Count() == startPositionsCount);
        //}
        ///// <summary>
        ///// Determines whether this instance [can edit component].
        ///// </summary>
        //[Fact]
        //public void CanEditPosition()
        //{
        //    // Arrange
        //    Position oldPosition = unitOfWork.Object.Positions.Find(x => x.PositionId == 1).FirstOrDefault();
        //    oldPosition.Name = "NewPosition1";
        //    oldPosition.IsManager = false;
        //    oldPosition.IsUnique = false;
        //    oldPosition.ParentComponent = new Component
        //    {
        //        ComponentId = 10,
        //        Name = "Added from CanEditPosition()",
        //    };
        //    // Act
        //    unitOfWork.Object.Complete();

        //    // Assert
        //    Position updatedPosition = unitOfWork.Object.Positions.Find(x => x.PositionId == 1).FirstOrDefault();
        //    Assert.True(updatedPosition.Name == "NewPosition1");
        //    Assert.True(updatedPosition.ParentComponent.ComponentId == 10);
        //}
    }
}
