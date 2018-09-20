using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Moq;
using OrgChartDemo.Controllers;
using OrgChartDemo.Models;
using OrgChartDemo.Models.Types;
using OrgChartDemo.Models.ViewModels;
using Xunit;

namespace OrgChartDemo.Tests
{
    /// <summary>
    /// Unit tests for the PositionController
    /// </summary>
    public class PositionControllerTests
    {
        private Mock<FakeRepository> mockRepo;
        /// <summary>
        /// Initializes a new instance of the <see cref="PositionControllerTests"/> class.
        /// <remarks>
        /// This will constructor initializes a <see cref="FakeRepository"/>
        /// </remarks>
        /// </summary>
        public PositionControllerTests()
        {            
            Mock<FakeRepository> mock = new Mock<FakeRepository>();

            // add Components
            mock.Setup(m => m.Components).Returns(new List<Component>
            {
                new Component { 
                    ComponentId = 1, 
                    ParentComponent = null, 
                    Name = "Component1" 
                },
                new Component { 
                    ComponentId = 2, 
                    ParentComponent = null, 
                    Name = "Component2"
                },
                new Component { 
                    ComponentId = 3, 
                    ParentComponent = null, 
                    Name = "Component3" 
                },
                new Component { 
                    ComponentId = 4, 
                    ParentComponent = null, 
                    Name = "Component4" 
                },
            });
            // add Positions
            mock.Setup(m => m.Positions).Returns( new List<Position>
            {
                // two Positions, one of which is designated as parent, assigned to same ParentComponent (to test IsManager duplicate)
                new Position { 
                    PositionId = 1, 
                    Name = "Position1", 
                    ParentComponent = mock.Object.Components.Where(x => x.ComponentId == 1).FirstOrDefault(), 
                    IsManager = true 
                },
                new Position { 
                    PositionId = 2, 
                    Name = "Position2", 
                    ParentComponent = mock.Object.Components.Where(x => x.ComponentId == 1).FirstOrDefault(), 
                    IsManager = false 
                },
                // three Positions, neither of which is manager, assigned to same ParentComponent
                new Position { 
                    PositionId = 3, 
                    Name = "Position3", 
                    ParentComponent = mock.Object.Components.Where(x => x.ComponentId == 2).FirstOrDefault(), 
                    IsManager = false 
                },
                new Position { 
                    PositionId = 4, 
                    Name = "Position4", 
                    ParentComponent = mock.Object.Components.Where(x => x.ComponentId == 2).FirstOrDefault(), 
                    IsManager = false 
                },
                new Position { 
                    PositionId = 5, 
                    Name = "Position5", 
                    ParentComponent = mock.Object.Components.Where(x => x.ComponentId == 2).FirstOrDefault(), 
                    IsManager = false 
                },
                // single Position, only child of ParentComponent, designated as IsManager
                new Position { 
                    PositionId = 6, 
                    Name = "Position6", 
                    ParentComponent = mock.Object.Components.Where(x => x.ComponentId == 3).FirstOrDefault(), 
                    IsManager = false 
                },
            });
            // add Members
            mock.Setup(m => m.Members).Returns(new List<Member>
            {
                // first member assigned to position 1
                new Member { 
                    FirstName = "Adam", 
                    LastName = "One", 
                    MemberId = 1, 
                    Position = mock.Object.Positions.Where(x => x.PositionId == 1).FirstOrDefault(),
                    Email = "AdamOne@test.mail" 
                },
                // two members assigned to position 2
                new Member { 
                    FirstName = "Bob", 
                    LastName = "Two", 
                    MemberId = 2, 
                    Position = mock.Object.Positions.Where(x => x.PositionId == 2).FirstOrDefault(),
                    Email = "BobTwo@test.mail"
                },
                new Member { 
                    FirstName = "Chuck", 
                    LastName = "Three", 
                    MemberId = 3, 
                    Position = mock.Object.Positions.Where(x => x.PositionId == 2).FirstOrDefault(), 
                },
                // one member assigned to position 3
                new Member { 
                    FirstName = "Dave", 
                    LastName = "Four", 
                    MemberId = 4, 
                    Position = mock.Object.Positions.Where(x => x.PositionId == 3).FirstOrDefault(), 
                }
            });
           mockRepo = mock;
        }
        /// <summary>
        /// Determines whether this instance [can get position list with member count].
        /// </summary>
        [Fact]        
        public void CanGetPositionListWithMemberCount()
        {
            // Arrange - Get a list of Members assigned to Position #1 (should be 2)
            var membersOfPositionOne = mockRepo.Object.Members.Where(m => m.Position.PositionId == 2).ToList();
            // Arrange - Assign each member to the Members collection of Position => PositionId == 2
            foreach (Member m in membersOfPositionOne){
                mockRepo.Object.Positions.Where(x => x.PositionId == 2).FirstOrDefault().Members.Add(m);
            }
            // Act - Call the GetPositionListWithMemberCount method of the mockRepo Object to get a List of PositionWithMemberCountItem(s)
            IEnumerable<PositionWithMemberCountItem> results = mockRepo.Object.GetPositionListWithMemberCount();

            // Assert - test that the count of Members is correct for two of the Positions
            Assert.True(results.Where(x => x.PositionId == 2).FirstOrDefault().MembersCount == 2);
            Assert.True(results.Where(x => x.PositionId == 1).FirstOrDefault().MembersCount == 0);
        }
        /// <summary>
        /// Determines whether this instance [can add position].
        /// </summary>
        [Fact]
        public void CanAddPosition()
        {
            // Arrange
            int startPositionsCount = mockRepo.Object.Positions.Count(); // get the count of positions in Repo at the start
            int finalPositionsCount = startPositionsCount + 1;
            Position p = new Position {
                Name = "New Position",
                ParentComponent = new Component { ComponentId = 1 },
                JobTitle = "Add New Position Test",
                IsManager = false,
                IsUnique = false                
            };

            // Act
            mockRepo.Object.AddPosition(p);
            
            // Assert
            Assert.True(mockRepo.Object.Positions.Count == finalPositionsCount);
        }
        /// <summary>
        /// Determines whether this instance [can add position from PositionController.Create()].
        /// </summary>
        [Fact]
        public void CanAddPositionFromController()
        {
            // Arrange
            int startPositionsCount = mockRepo.Object.Positions.Count(); // get the count of positions in Repo at the start
            int finalPositionsCount = startPositionsCount + 1;
            PositionsController controller = new PositionsController(mockRepo.Object);
            PositionWithComponentListViewModel form = new PositionWithComponentListViewModel {
                PositionName = "New Position",
                ParentComponentId = 1,
                JobTitle = "Add New Position Test",
                IsManager = false,
                IsUnique = false                
            };

            // Act 
            controller.Create(form);

            // Assert
            
            Assert.True(mockRepo.Object.Positions.Count == finalPositionsCount );
        }
        /// <summary>
        /// Determines whether this instance [can detect existing position name] and prevent a duplicate add.
        /// </summary>
        [Fact]
        public void CanDetectExistingPositionName()
        {            
            // Arrange
            int startPositionsCount = mockRepo.Object.Positions.Count(); // get the count of positions in Repo at the start
            PositionsController controller = new PositionsController(mockRepo.Object);
            PositionWithComponentListViewModel form = new PositionWithComponentListViewModel {
                PositionName = "Position1",
                JobTitle = "Position1 Test Job"
            };

            // Act - this should NOT add a Position - the name "Position1" already exists in the repo
            controller.Create(form);

            // Assert
            Assert.Equal(startPositionsCount, mockRepo.Object.Positions.Count());
                    
        }
        /// <summary>
        /// Determines whether this instance [can detect component position manager duplicate].
        /// </summary>
        [Fact]
        public void CanDetectComponentPositionManagerDuplicate()
        {
            // Arrange
            Component c = mockRepo.Object.Components.Find(x => x.ComponentId == 4);
            Position p = new Position {
                    Name = "Test Position",
                    ParentComponent = c,
                    IsManager = true,
                    IsUnique = true,
                    JobTitle = "Test Duplicate Manager Job Title"                    
                    };
            mockRepo.Object.AddPosition(p);

            int startPositionsCount = mockRepo.Object.Positions.Count(); // get the count of positions in Repo at the start
            int finalPositionsCount = startPositionsCount + 1;
            
            PositionsController controller = new PositionsController(mockRepo.Object);
            PositionWithComponentListViewModel form = new PositionWithComponentListViewModel {
                PositionName = "New Position",
                ParentComponentId = 4,
                JobTitle = "Add New Position Test",
                IsManager = true,
                IsUnique = false                
            };

            // Act - This should NOT add a Position to the repo
            controller.Create(form);

            // Assert            
            Assert.True(mockRepo.Object.Positions.Count == startPositionsCount );
        }
        /// <summary>
        /// Determines whether this instance [can edit component].
        /// </summary>
        [Fact]
        public void CanEditPosition()
        {
            // Arrange
            Position oldPosition = mockRepo.Object.Positions.Find(x => x.PositionId == 1);
            Position newPosition = new Position
            {
                PositionId = 1,
                Name = "NewPosition1",
                IsManager = false,
                IsUnique = false,
                ParentComponent = new Component
                {
                    ComponentId = 10,
                    Name = "Added from CanEditPosition()",
                },                
            };
            // Act
            mockRepo.Object.EditPosition(newPosition);

            // Assert
            Position updatedPosition = mockRepo.Object.Positions.Find(x => x.PositionId == 1);
            Assert.True(updatedPosition.Name == "NewPosition1");
            Assert.True(updatedPosition.ParentComponent.ComponentId == 10);
        }
    }
}
