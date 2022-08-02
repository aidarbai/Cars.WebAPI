using System.Collections.Generic;

namespace Cars.DAL.Models
{
    public class Color : IdAndNameProps
    {
        // add fullname prop
        public virtual ICollection<Car> Cars { get; set; } = new List<Car>();
    }
}
