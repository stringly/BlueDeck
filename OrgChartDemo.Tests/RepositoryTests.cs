using System.Collections.Generic;
using Xunit;
using OrgChartDemo.Models;
using OrgChartDemo.Models.Repositories;
using Moq;
using OrgChartDemo.Models.Types;

namespace OrgChartDemo.Tests
{
    /// <summary>
    /// This class contains unit tests for the Repository Interfaces
    /// <remarks>
    /// These tests should NOT implement the test SQLite DbContext
    /// </remarks>
    /// </summary>
    public class RepositoryTests
    {
        /// <summary>
        /// Tests whether a component identifier will return the corresponding component's name.
        /// </summary>
        [Fact]
        public void Given_Component_Id_Should_Get_Component_Name()
        {
            // Arrange
            var componentId = 1;
            var expected = "Component 1";
            var component = new Component() { Name = expected, ComponentId = componentId };
            var componentRepositoryMock = new Mock<IComponentRepository>();
            componentRepositoryMock.Setup(m => m.Get(componentId)).Returns(component).Verifiable();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(m => m.Components).Returns(componentRepositoryMock.Object);

            // Act
            var actual = unitOfWorkMock.Object.Components.Get(componentId);

            // Assert
            componentRepositoryMock.Verify();
            Assert.NotNull(actual);
            Assert.Equal(expected, actual.Name);
        }

        /// <summary>
        /// Tests if the position identifier will return the corresponding get position's name.
        /// </summary>
        [Fact]
        public void Given_Position_Id_Should_Get_Position_Name()
        {
            // Arrange
            var positionId = 1;
            var expected = "Position 1";
            var position = new Position { Name = expected, PositionId = positionId };
            var positionRepositoryMock = new Mock<IPositionRepository>();
            positionRepositoryMock.Setup(x => x.Get(positionId)).Returns(position).Verifiable();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(m => m.Positions).Returns(positionRepositoryMock.Object);

            // Act
            var actual = unitOfWorkMock.Object.Positions.Get(positionId);

            // Assert
            positionRepositoryMock.Verify();
            Assert.NotNull(actual);
            Assert.Equal(expected, actual.Name);            
        }

        /// <summary>
        /// Tests if the member identifier will return the corresponding member's name.
        /// </summary>
        [Fact]
        public void Given_Member_Id_Should_Get_Member_Name()
        {
            // Arrange
            var memberId = 1;
            var expected = "Name 1";
            var member = new Member { FirstName = expected, MemberId = memberId };
            var memberRepositoryMock = new Mock<IMemberRepository>();
            memberRepositoryMock.Setup(x => x.Get(memberId)).Returns(member).Verifiable();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(m => m.Members).Returns(memberRepositoryMock.Object);

            // Act
            var actual = unitOfWorkMock.Object.Members.Get(memberId);

            // Assert
            memberRepositoryMock.Verify();
            Assert.NotNull(actual);
            Assert.Equal(expected, actual.FirstName);
        }

        /// <summary>
        /// Tests the <see cref="T:OrgChartDemo.Models.Repositories.IPositionRepository.GetPositionsWithMembers"/> 
        /// to ensure that the list of positions successfully includes it's corresponding members.
        /// </summary>
        [Fact]
        public void Position_With_Members_Should_Get_Members()
        {
            // Arrange
            var positionId = 1;
            var positionName = "Position 1";
            var position = new Position { PositionId = positionId, Name = positionName };
            var member1 = new Member { MemberId = 1, FirstName = "Member 1" };
            var member2 = new Member { MemberId = 2, FirstName = "Member 2" };
            position.Members.Add(member1);
            position.Members.Add(member2);
            var expectedMemberCount = 2;
            int actualMemberCount = 0;
            IEnumerable<Position> list = new List<Position> { position };
            var positionRepositoryMock = new Mock<IPositionRepository>();
            positionRepositoryMock.Setup(x => x.GetPositionsWithMembers()).Returns(list).Verifiable();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(x => x.Positions).Returns(positionRepositoryMock.Object);

            // Act
            IEnumerable<Position> actual = unitOfWorkMock.Object.Positions.GetPositionsWithMembers();

            // Assert
            positionRepositoryMock.Verify();
            Assert.NotNull(actual);
            foreach (Position p in actual)
            {
                foreach (Member m in p.Members)
                {
                    actualMemberCount++;
                }                
            }
            Assert.Equal(actualMemberCount, expectedMemberCount);
        }

        //[Fact]
        //public void Member_Should_Get_MemberContactNumbers()
        //{
        //    // Arrange

        //    // create mock member
        //    var memberMock = new Member()
        //    {
        //        MemberId = 1,
        //        FirstName = "Jason",
        //        LastName = "Smith",                
        //    };
        //    // create mock MemberContact
            
        //    // first, need Number Types
        //    var numberType1 = new PhoneNumberType(){
        //            PhoneNumberTypeId = 0,
        //            PhoneNumberTypeName = "Home"
        //        };
        //    var numberType2 = new PhoneNumberType(){
        //            PhoneNumberTypeId = 1,
        //            PhoneNumberTypeName = "Cell"
        //        };
        //    // now, mock Contacts

        //    var contacts = new List<MemberContactNumber>()
        //    {
        //        new MemberContactNumber()
        //        {
        //            Member = memberMock,
        //            MemberContactNumberId = 0,
        //            PhoneNumber = "(301) 648-3444",
        //            Type = numberType1
                    
        //        },
        //        new MemberContactNumber()
        //        { 
        //            Member = memberMock,
        //            MemberContactNumberId = 1,
        //            PhoneNumber = "(123) 456-7890",
        //            Type = numberType2
        //        }
                
        //    };
        //}
    }
}
