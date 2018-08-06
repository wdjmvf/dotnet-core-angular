using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace vega.Controllers.Resources
{
    public class VehicleResource
    {
        public int Id { get; set; }
        
        public KeyValuePaireResource Model { get; set; }
        public KeyValuePaireResource Make { get; set; }
        public bool IsRegistered { get; set; }
        public ContactResource Contact {get;set;}
        public DateTime LastUpdate { get; set; }
        public ICollection<KeyValuePaireResource> Features { get; set; }

        public VehicleResource(){
            Features = new Collection<KeyValuePaireResource>();
        }
    }
}