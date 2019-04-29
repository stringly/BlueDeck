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
        public ComponentApiResult ParentComponent { get; set; }

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
                ParentComponent = new ComponentApiResult()
                {
                    ComponentId = _component.ParentComponentId,
                    Name = _component.ParentComponent.Name,
                    Acronym = _component.Acronym                    
                };
            }
            
        }
    }
}
