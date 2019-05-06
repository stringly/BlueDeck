using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlueDeck.Models.APIModels
{
    public class ComponentApiResult
    {
        public int? ComponentId { get; set; }
        public string Name { get; set; }
        public string Acronym { get; set; }
        public SubComponentApiResult ParentComponent { get; set; }
        public List<PositionApiResult> Positions { get; set; }

        public ComponentApiResult()
        {
        }

        public ComponentApiResult(Component _component)
        {
            ComponentId = _component?.ComponentId;
            Name = _component?.Name ?? "";
            Acronym = _component?.Acronym ?? "";
            if (_component?.ParentComponent != null)
            {
                ParentComponent = new SubComponentApiResult(_component.ParentComponent);                
            }
            if (_component?.Positions != null)
            {
                Positions = _component.Positions.ToList().ConvertAll(x => new PositionApiResult(x));
            }
            
        }
    }
}
