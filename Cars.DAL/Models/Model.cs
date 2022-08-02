using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Cars.DAL.Models
{
    //[ExcludeFromCodeCoverage]
    public class Model : IdAndNameProps // Corolla
    {
        public virtual Make Make { get; set; }
        public virtual ICollection<Car> Cars { get; set; } = new List<Car>();
    }
}
