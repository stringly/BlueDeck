using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrgChartDemo.Models {
    public class FakeComponentRepository : IComponentRepository {
        public IEnumerable<OrgChartComponentWithMember> OrgChartComponents => new List<OrgChartComponentWithMember> {
            new OrgChartComponentWithMember {
                id = 1,
                parentid = null,
                componentName = "District I Commander's Office",
                memberId = 1,
                memberName = "Maj. Rosa Guixens #2013",
                contactNumber = "(301) 699-2630",
                email = "rmguixens@co.pg.md.us"
            },
            new OrgChartComponentWithMember {
                id = 2,
                parentid = 1,
                componentName = "District I",
                memberId = 2,
                memberName = "Capt. Kathleen Biddison #2185",
                contactNumber = "(301) 699-2630",
                email = "kbiddison@co.pg.md.us"
            },
            new OrgChartComponentWithMember {
                id = 3,
                parentid = 2,
                componentName = "District I, Shift I",
                memberId = 3,
                memberName = "1st Lt. John Decker #603",
                contactNumber = "(301) 699-2630",
                email = "jdecker@co.pg.md.us"
            },
            new OrgChartComponentWithMember {
                id = 4,
                parentid = 2,
                componentName = "District I, Shift II",
                memberId = 4,
                memberName = "A/Lt. Joseph Willis #3148",
                contactNumber = "(301) 699-2630",
                email = "jwillis@co.pg.md.us"
            },
            new OrgChartComponentWithMember {
                id = 5,
                parentid = 2,
                componentName = "District I, Shift III",
                memberId = 5,
                memberName = "Lt. Jason Smith #3134",
                contactNumber = "(301) 699-2630",
                email = "jcsmith1@co.pg.md.us"
            },
            new OrgChartComponentWithMember {
                id = 6,
                parentid = 2,
                componentName = "District I, Shift IV",
                memberId = 6,
                memberName = "Lt. Richard Hartman #2851",
                contactNumber = "(301) 699-2630",
                email = "rhartman@co.pg.md.us"
            },
            new OrgChartComponentWithMember {
                id = 7,
                parentid = 2,
                componentName = "District I, Shift V",
                memberId = 7,
                memberName = "Lt. Joel Lewis #2484",
                contactNumber = "(301) 699-2630",
                email = "jlewis@co.pg.md.us"
            },
            new OrgChartComponentWithMember {
                id = 8,
                parentid = 1,
                componentName = "District I COPS/Enforcement",
                memberId = 8,
                memberName = "Lt. Jeffery Walters #2543",
                contactNumber = "(301) 699-2630",
                email = "walters@co.pg.md.us"
            }
        };
    }
}
