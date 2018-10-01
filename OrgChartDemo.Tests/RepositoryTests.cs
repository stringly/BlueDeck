using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using OrgChartDemo;
using OrgChartDemo.Models;
using OrgChartDemo.Models.Repositories;
using Moq;

namespace OrgChartDemo.Tests
{
    public class RepositoryTests
    {
        [Fact]
        public void Given_Component_Id_Should_Get_Component_Name()
        {
            // Arrange
            var componentId = 1;
            var expected = "Office of the Chief of Police";
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
    }
}
