using Microsoft.EntityFrameworkCore;
using OrgChartDemo.Models;
using OrgChartDemo.Tests.Helpers;
using System.Linq;
using TestSupport.EfHelpers;
using Xunit;

namespace OrgChartDemo.Tests
{
    /// <summary>
    /// Class containing tests for ensuring the OrgChartDemo Test environment is correctly configured for testing
    /// </summary>
    public class TestingProvisioning : UnitTestFrame
    {
        /// <summary>
        /// Determines whether this instance can create the SQlite database.
        /// </summary>
        [Fact]
        public void Can_Create_Sqlite_Db()
        {
            // Arrange            
            using (var context = new ApplicationDbContext(options))
            {
                // Create the database
                context.Database.EnsureCreated();
                

                // Act
                context.Components.Add(new Component { Name = "New Component" });
                context.SaveChanges();

                // Assert
                Assert.Equal("New Component", context.Components.First().Name);
            }
        }

        /// <summary>
        /// Determines whether this instance can reset the SQLite database.
        /// </summary>
        [Fact]
        public void Can_Reset_Db()
        {
            // Arrange
            using (var context = new ApplicationDbContext(options))
            {
                // Create, clear the Db
                context.Database.EnsureCreated();
                // Record the Count of Components upon creation
                int newComponentCount = context.Components.Count();
                // Add a component
                context.Components.Add(new Component {
                    ComponentId = 1,
                    ParentComponent = null,
                    Name = "Component1"
                });
                context.SaveChanges();

                // Record the updated count of Components
                int updatedComponentCount = context.Components.Count();

                // Act - Clear the Db using .EnsureCreated();
                context.ResetDatabase();                

                // Assert
                Assert.True(newComponentCount == 0);
                Assert.True(updatedComponentCount == 1);
                Assert.True(context.Components.Count() == 0);            
            }
        }
    }
}
