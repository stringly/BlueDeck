using OrgChartDemo.Models;
using Microsoft.Data.Sqlite;
using OrgChartDemo.Models.Types;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace OrgChartDemo.Tests.Helpers
{
    /// <summary>
    /// Extension methods use to provision an in-Memory test SQLite Db used in unit testing
    /// </summary>
    public static class DatabaseHelpers
    {
        /// <summary>
        /// Seeds the database for testing.
        /// </summary>
        /// <param name="context">The instance of <see cref="T:OrgChartDemo.Models.ApplicationDbContext"/> we are extending.</param>
        public static void SeedDatabaseForTesting(this ApplicationDbContext context)
        {
            
            context.Components.AddRange(new List<Component> {
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
                }
            });
            context.Positions.AddRange(new List<Position> {
                // two Positions, one of which is designated as parent, assigned to same ParentComponent (to test IsManager duplicate)
                new Position {
                    PositionId = 1,
                    Name = "Position1",
                    ParentComponent = new Component {
                        ComponentId = 1,
                        ParentComponent = null,
                        Name = "Component1"
                    },
                    IsManager = true
                },
                new Position {
                    PositionId = 2,
                    Name = "Position2",
                    ParentComponent = new Component {
                        ComponentId = 1,
                        ParentComponent = null,
                        Name = "Component1"
                    },
                    IsManager = false
                },
                // three Positions, neither of which is manager, assigned to same ParentComponent
                new Position {
                    PositionId = 3,
                    Name = "Position3",
                    ParentComponent = new Component {
                        ComponentId = 2,
                        ParentComponent = null,
                        Name = "Component2"
                    },
                    IsManager = false
                },
                new Position {
                    PositionId = 4,
                    Name = "Position4",
                    ParentComponent = new Component {
                        ComponentId = 2,
                        ParentComponent = null,
                        Name = "Component2"
                    },
                    IsManager = false
                },
                new Position {
                    PositionId = 5,
                    Name = "Position5",
                    ParentComponent = new Component {
                        ComponentId = 2,
                        ParentComponent = null,
                        Name = "Component2"
                    },
                    IsManager = false
                },
                // single Position, only child of ParentComponent, designated as IsManager
                new Position {
                    PositionId = 6,
                    Name = "Unassigned",
                    ParentComponent = new Component {
                        ComponentId = 100,
                        ParentComponent = null,
                        Name = "Unassigned Pool"
                    },
                    IsManager = false
                }
            });
            context.Members.AddRange(new List<Member> {
                // first member assigned to position 1
                new Member {
                    FirstName = "Adam",
                    LastName = "One",
                    MemberId = 1,
                    Position = new Position {
                        PositionId = 1,
                        Name = "Position1",
                        ParentComponent = new Component {
                            ComponentId = 1,
                            ParentComponent = null,
                            Name = "Component1"
                        },
                        IsManager = true
                    },
                    Email = "AdamOne@test.mail"
                },
                // two members assigned to position 2
                new Member {
                    FirstName = "Bob",
                    LastName = "Two",
                    MemberId = 2,
                    Position = new Position {
                        PositionId = 2,
                        Name = "Position2",
                        ParentComponent = new Component {
                            ComponentId = 1,
                            ParentComponent = null,
                            Name = "Component1"
                        },
                        IsManager = false
                    },
                    Email = "BobTwo@test.mail"
                },
                new Member {
                    FirstName = "Chuck",
                    LastName = "Three",
                    MemberId = 3,
                    Position = new Position {
                        PositionId = 2,
                        Name = "Position2",
                        ParentComponent = new Component {
                            ComponentId = 1,
                            ParentComponent = null,
                            Name = "Component1"
                        },
                        IsManager = false
                    },
                },
                // one member assigned to position 3
                new Member {
                    FirstName = "Dave",
                    LastName = "Four",
                    MemberId = 4,
                    Position = new Position {
                        PositionId = 3,
                        Name = "Position3",
                        ParentComponent = new Component {
                            ComponentId = 2,
                            ParentComponent = null,
                            Name = "Component2"
                        },
                        IsManager = false
                    },
                }
            });
                //context.MemberRanks.AddRange(new List<MemberRank> {
                //    new MemberRank { RankId = 0, RankFullName = "Police Officer", RankShort = "P/O", PayGrade = "L01"},
                //    new MemberRank { RankId = 1, RankFullName = "Police Officer First Class", RankShort = "POFC", PayGrade = "L02" },
                //    new MemberRank { RankId = 2, RankFullName = "Corporal", RankShort = "Cpl.", PayGrade = "L03" },
                //    new MemberRank { RankId = 3, RankFullName = "Sergeant", RankShort = "Sgt.", PayGrade = "L04" }
                //    });
            context.SaveChanges();      
        }

        /// <summary>
        /// Resets the database.
        /// </summary>
        /// <param name="context">The instance of <see cref="T:OrgChartDemo.Models.ApplicationDbContext"/> being extended.</param>
        public static void ResetDatabase(this ApplicationDbContext context)
        {
            context.Database.ExecuteSqlCommand("DELETE FROM Components");
            context.Database.ExecuteSqlCommand("DELETE FROM Positions");
            context.Database.ExecuteSqlCommand("DELETE FROM Members");
            context.SaveChanges();
        }
    }
}
