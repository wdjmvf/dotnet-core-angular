using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace vega.Controllers.Resources
{
    public class MakeResource : KeyValuePaireResource
    {
        public ICollection<KeyValuePaireResource> Models { get; set;}

        public MakeResource(){
            Models = new Collection<KeyValuePaireResource>();
        }
    }
}