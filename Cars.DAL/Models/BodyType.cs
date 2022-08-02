using System.Collections.Generic;

namespace Cars.DAL.Models
{
    public class BodyType : IdAndNameProps
    {
        public virtual ICollection<Car> Cars { get; set; } = new List<Car>();
    }
}
