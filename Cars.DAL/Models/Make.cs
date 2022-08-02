using System.Collections.Generic;

namespace Cars.DAL.Models
{
    public class Make : IdAndNameProps // Toyota
    {
        public virtual ICollection<Model> Models { get; set; } = new List<Model>();
    }
}
