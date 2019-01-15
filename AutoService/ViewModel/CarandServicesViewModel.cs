using AutoService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoService.ViewModel
{
    public class CarandServicesViewModel
    {
        public Car CarObj { get; set; }
        public Service NewServiceObj { get; set; }
        public IEnumerable<Service> PastServiceObj { get; set; }
        public List<ServiceType> ServiceTypesObj { get; set; }
    }
}
