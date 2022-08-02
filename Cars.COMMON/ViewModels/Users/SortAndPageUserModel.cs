using System.Text;
using static Cars.COMMON.Constants.AppConstants;

namespace Cars.COMMON.ViewModels.Users
{
    public class SortAndPageUserModel
    {
        public string SortBy { get; set; } = Attributes.LASTNAME;
        public Order Order { get; set; } = Order.ASC;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;

        public override string ToString()
        {
            var props = this.GetType().GetProperties();
            var sb = new StringBuilder();
            foreach (var p in props)
            {
                sb.AppendLine(p.Name + ": " + p.GetValue(this, null));
            }
            return sb.ToString();
        }
    }
}