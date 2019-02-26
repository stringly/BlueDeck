using System.Linq;
using Moq;
using OrgChartDemo.Controllers;
using OrgChartDemo.Models;
using OrgChartDemo.Models.ViewModels;
using Xunit;
using OrgChartDemo.Persistence;
using OrgChartDemo.Tests.Helpers;
using Microsoft.EntityFrameworkCore;

namespace OrgChartDemo.Tests
{
    /// <summary>
    /// Unit tests for the PositionController
    /// </summary>
    public class PositionControllerTests : UnitTestFrame
    {
        /// <summary>
        /// Determines whether a position can be added to the Context using <see cref="T:OrgChartDemo.Controllers.PositionsController.Create(PositionWithComponentListViewModel)"/>.
        /// </summary>
        [Fact]
        public void Can_Add_Position_To_Context_Via_Controller()
        {
            // Arrange - create the testing DbContext
            using (ApplicationDbContext context = new ApplicationDbContext(options))
            {
                // Ensure the test Db is Clear
                context.Database.EnsureCreated();
                context.ResetDatabase();
                context.SeedDatabaseForTesting();                
                int expectedPositionCount = context.Positions.Count() + 1;
                // Create a mock controller with a UnitOfWork created from the testing context
                Mock<PositionsController> mockController = new Mock<PositionsController>(new UnitOfWork(context));

                // Create a new ViewModel from a new Position to mimic user data entry                
                PositionWithComponentListViewModel form = new PositionWithComponentListViewModel(new Position 
                    {
                        Name = "New Position",
                        IsManager = false,
                        IsUnique = false,
                        JobTitle = "Test",                        
                    });

                // Act - attempt to add the position using the controller Method
                mockController.Object.Create(form);

                // Assert - verify the position was added
                Assert.True(context.Positions.Count() == expectedPositionCount);
            }
        }

        /// <summary>
        /// Determines whether a position can be removed via <see cref="T:OrgChartDemo.Controllers.PositionsController.DeleteConfirmed(int)"/>.
        /// </summary>
        [Fact]
        public void Can_Remove_Position_Via_Controller()
        {
            // Arrange - create the testing DbContext
            using (ApplicationDbContext context = new ApplicationDbContext(options))
            {
                // Ensure the test Db is Clear
                context.Database.EnsureCreated();
                context.ResetDatabase();
                context.SeedDatabaseForTesting();
                int expectedPositionCount = context.Positions.Count() - 1;

                // Create a mock controller with a UnitOfWork created from the testing context
                Mock<PositionsController> mockController = new Mock<PositionsController>(new UnitOfWork(context));

                // Act - Attempt to remove the Position with PositionId = 3
                mockController.Object.DeleteConfirmed(3);

                // Assert - verify the position was removed
                Assert.True(context.Positions.Count() == expectedPositionCount);
            }
        }

        /// <summary>
        /// Tests editing a Position using <see cref="T:OrgChartDemo.Controllers.PositionsController.Edit(int, PositionWithComponentListViewModel)"/>.
        /// </summary>
        [Fact]
        public void Given_Position_Id_Can_Edit_Position_In_Context_Via_Controller()
        {
            // Arrange - create the testing DbContext
            using (ApplicationDbContext context = new ApplicationDbContext(options))
            {
                // Ensure the test Db is Clear
                context.Database.EnsureCreated();
                context.ResetDatabase();
                context.SeedDatabaseForTesting();
                Position initialPosition = context.Positions.Where(x => x.PositionId == 3).FirstOrDefault();
                var expectedName = "Expected Name";
                PositionWithComponentListViewModel form = new PositionWithComponentListViewModel(initialPosition) {
                    PositionName = expectedName
                };
                
                // Create a mock controller with a UnitOfWork created from the testing context
                Mock<PositionsController> mockController = new Mock<PositionsController>(new UnitOfWork(context));

                // Act - Attempt to remove the Position with PositionId = 3
                mockController.Object.Edit(3, form);

                // Assert - verify the position was removed
                Assert.True(context.Positions.Where(x => x.PositionId == 3).First().Name == expectedName);
            }
        }

        /// <summary>
        /// Determines whether this instance can detect duplicate position name when adding a new Position
        /// via <see cref="T:OrgChartDemo.Controllers.PositionsController.Create(PositionWithComponentListViewModel)"/>.
        /// </summary>
        [Fact]
        public void Can_Detect_Duplicate_Position_Name_For_New_Position()
        {
            // Arrange - create the testing DbContext
            using (ApplicationDbContext context = new ApplicationDbContext(options))
            {
                // Ensure the test Db is Clear
                context.Database.EnsureCreated();
                context.ResetDatabase();
                context.SeedDatabaseForTesting();
                int expectedPositionCount = context.Positions.Count();
                // Create a mock controller with a UnitOfWork created from the testing context
                Mock<PositionsController> mockController = new Mock<PositionsController>(new UnitOfWork(context));

                // Create a new ViewModel from a new Position to mimic user data entry. The position name                 
                PositionWithComponentListViewModel form = new PositionWithComponentListViewModel(new Position
                {
                    Name = "Position1",
                    IsManager = false,
                    IsUnique = false,
                    JobTitle = "Test",
                });

                // Act - attempt to add the position using the controller Method
                mockController.Object.Create(form);

                // Assert - verify the position was added
                Assert.True(mockController.Object.ViewBag.Message == "A Position with the name Position1 already exists. Use a different Name.\n");
                Assert.True(context.Positions.Count() == expectedPositionCount);
            }
        }

        /// <summary>
        /// Determines whether this instance can detect duplicate position name when adding a new Position
        /// via <see cref="T:OrgChartDemo.Controllers.PositionsController.Edit(int, PositionWithComponentListViewModel)"/>.
        /// </summary>
        [Fact]
        public void Can_Detect_Duplicate_Position_Name_For_Existing_Position()
        {
            // Arrange - create the testing DbContext
            using (ApplicationDbContext context = new ApplicationDbContext(options))
            {
                // Ensure the test Db is Clear
                context.Database.EnsureCreated();
                context.ResetDatabase();
                context.SeedDatabaseForTesting();
                int expectedPositionCount = context.Positions.Count();
                // Create a mock controller with a UnitOfWork created from the testing context
                Mock<PositionsController> mockController = new Mock<PositionsController>(new UnitOfWork(context));

                // Create a new ViewModel from the Position with id = 2 to mimic user data entry. The position name                 
                PositionWithComponentListViewModel form = new PositionWithComponentListViewModel(context.Positions.Where(x => x.PositionId == 2).FirstOrDefault());

                // Change the Position name to that of another position
                form.PositionName = "Position1";

                // Act - attempt to add the position using the controller Method
                mockController.Object.Edit(2, form);

                // Assert - verify the position was added
                Assert.True(mockController.Object.ViewBag.Message == "A Position with the name Position1 already exists. Use a different Name.\n");
                Assert.True(context.Positions.Count() == expectedPositionCount);
            }
        }

        /// <summary>
        /// Determines whether this instance can detect existing manager in parent component via 
        /// <see cref="T:OrgChartDemo.Controllers.PositionsController.Create(PositionWithComponentListViewModel)"/>.
        /// </summary>
        [Fact]
        public void Can_Detect_Existing_Manager_In_Parent_Component_Via_Controller_Create()
        {
            // Arrange - create the testing DbContext
            using (ApplicationDbContext context = new ApplicationDbContext(options))
            {
                // Ensure the test Db is Clear
                context.Database.EnsureCreated();
                context.ResetDatabase();
                context.SeedDatabaseForTesting();
                int expectedPositionCount = context.Positions.Count();
                // Create a mock controller with a UnitOfWork created from the testing context
                Mock<PositionsController> mockController = new Mock<PositionsController>(new UnitOfWork(context));

                // Create a new ViewModel from a new Position to mimic user data entry. The position name                 
                PositionWithComponentListViewModel form = new PositionWithComponentListViewModel(new Position
                {
                    Name = "Duplicate Manager",
                    IsManager = true,
                    IsUnique = false,
                    JobTitle = "Test",
                });

                // set the form's ParentComponentId to a component that is already assigned a Manager Position
                form.ParentComponentId = 1;
                // Act - attempt to add the position using the controller Method
                mockController.Object.Create(form);

                // Assert - verify the position was added
                Assert.True(mockController.Object.ViewBag.Message == "Component1 already has a Position designated as Manager. Only one Manager Position is permitted.\n");
                Assert.True(context.Positions.Count() == expectedPositionCount);
            }
        }

        /// <summary>
        /// Determines whether this instance can detect existing manager in parent component via 
        /// <see cref="T:OrgChartDemo.Controllers.PositionsController.Edit(int, PositionWithComponentListViewModel)"/>.
        /// </summary>
        [Fact]
        public void Can_Detect_Existing_Manager_In_Parent_Component_Via_Controller_Edit()
        {
            // Arrange - create the testing DbContext
            using (ApplicationDbContext context = new ApplicationDbContext(options))
            {
                // Ensure the test Db is Clear
                context.Database.EnsureCreated();
                context.ResetDatabase();
                context.SeedDatabaseForTesting();
                int expectedPositionCount = context.Positions.Count();
                // Create a mock controller with a UnitOfWork created from the testing context
                Mock<PositionsController> mockController = new Mock<PositionsController>(new UnitOfWork(context));

                // Create a new ViewModel from a Position assigned to the same ParentComponent as another Manager Position to mimic user data entry. 
                PositionWithComponentListViewModel form = new PositionWithComponentListViewModel(context.Positions.Where(x => x.PositionId == 2).FirstOrDefault());

                // Change the form's position to Manager
                form.IsManager = true;

                // Act - attempt to add the position using the controller Method
                mockController.Object.Edit(2, form);

                // Assert - verify the position was added
                Assert.True(mockController.Object.ViewBag.Message == "Component1 already has a Position designated as Manager. You can not elevate this Position.\n");
                Assert.True(context.Positions.Count() == expectedPositionCount);
            }
        }

        /// <summary>
        /// Determines whether this instance can prevent changing existing position to unique when multiple members are assigned via 
        /// <see cref="T:OrgChartDemo.Controllers.PositionsController.Edit(int, PositionWithComponentListViewModel)"/>.
        /// </summary>
        [Fact]
        public void Can_Prevent_Existing_Position_To_Unique_When_Multiple_Members_Via_Controller_Edit()
        {
            // Arrange - create the testing DbContext
            using (ApplicationDbContext context = new ApplicationDbContext(options))
            {
                // Ensure the test Db is Clear
                context.Database.EnsureCreated();
                context.ResetDatabase();
                context.SeedDatabaseForTesting();
                int expectedPositionCount = context.Positions.Count();
                // Create a mock controller with a UnitOfWork created from the testing context
                Mock<PositionsController> mockController = new Mock<PositionsController>(new UnitOfWork(context));

                // Create a new ViewModel from a Position assigned to the same ParentComponent as another Manager Position to mimic user data entry. 
                PositionWithComponentListViewModel form = new PositionWithComponentListViewModel(context.Positions.Where(x => x.PositionId == 2).FirstOrDefault());

                // Change the form's position to Manager
                form.IsUnique = true;

                // Act - attempt to add the position using the controller Method
                mockController.Object.Edit(2, form);

                // Assert - verify the position was added
                Assert.True(mockController.Object.ViewBag.Message == "Position2 has 2 current Members. You can't set this Position to Unique with multiple members.\n");
                Assert.True(context.Positions.Count() == expectedPositionCount);
            }
        }

        /// <summary>
        /// Determines whether this instance can move members of deleted position to the Unassigned pool 
        /// via <see cref="T:OrgChartDemo.Controllers.PositionsController.DeleteConfirmed(int)"/>.
        /// </summary>
        [Fact]
        public void Can_Unassign_Members_Of_Deleted_Position_Via_Delete()
        {
            // Arrange - create the testing DbContext
            using (ApplicationDbContext context = new ApplicationDbContext(options))
            {
                // Ensure the test Db is Clear
                context.Database.EnsureCreated();
                context.ResetDatabase();
                context.SeedDatabaseForTesting();
                int expectedPositionCount = context.Positions.Count() - 1;
                // Create a mock controller with a UnitOfWork created from the testing context
                Mock<PositionsController> mockController = new Mock<PositionsController>(new UnitOfWork(context));

                // Act - attempt to delete the position using the controller Method, which should reassign members id = 2 and 3 to "Unassigned."
                mockController.Object.DeleteConfirmed(2);

                // Assert - verify the position was deleted and the members were reassigned
                Assert.True(context.Positions.Count() == expectedPositionCount);
                
                Position resultPosition = context.Positions.Where(x => x.PositionId == 6).Include(x => x.Members).First();
                Assert.True(resultPosition.Members.Count() == 2);
                //Assert.True(context.Members.Include(p => p.Position).First(x => x.MemberId == 3).Position.Name == "Unassigned");
            }
        }
    }
}
