using Microsoft.EntityFrameworkCore;
using OrgChartDemo.Models;
using System;
using System.Collections.Generic;
using System.Text;
using TestSupport.EfHelpers;

namespace OrgChartDemo.Tests.Helpers
{
    /// <summary>
    /// A Generic class that encapsulates creating test DbContext for Unit Testing Modules
    /// </summary>
    public class UnitTestFrame
    {
        /// <summary>
        /// A <see cref="T:Microsoft.EntityFrameworkCore.DbContextOptions{ApplicationDbContext}"/>
        /// <remarks>
        /// This member should
        /// </remarks>
        /// </summary>
        public DbContextOptions<ApplicationDbContext> options;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:OrgChartDemo.Tests.TestingProvisioning"/> class.
        /// <remarks>
        /// This constructor uses the <a href="https://github.com/JonPSmith/EfCore.TestSupport/wiki">EFCore.TestSupport</a> package
        /// and creates a <see cref="Microsoft.EntityFrameworkCore.DbContextOptions"/> object that will create and 
        /// in-memory SQLite Db based on the <see cref="T:OrgChartDemo.Models.ApplicationDbContext"/> 
        /// Every Testing Clasas must have a Constructor that assigns this <see cref="Microsoft.EntityFrameworkCore.DbContextOptions"/> object. 
        /// </remarks>
        /// </summary>
        public UnitTestFrame()
        {
            options = SqliteInMemory.CreateOptions<ApplicationDbContext>();
        }
    }
}
