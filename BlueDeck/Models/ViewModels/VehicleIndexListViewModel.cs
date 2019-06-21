using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlueDeck.Models.ViewModels
{
    public class VehicleIndexListViewModel
    {
        public IEnumerable<Vehicle> Vehicles { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public string ModelNameSort { get; set; }
        public string ManufacturerNameSort { get; set; }
        public string ModelYearSort { get; set; }
        public string CruiserNumberSort { get; set; }
        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }

        public VehicleIndexListViewModel(List<Vehicle> _vehicles)
        {
            Vehicles = _vehicles;
        }
    }
}
