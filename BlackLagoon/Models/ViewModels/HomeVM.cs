using BlackLagoon.Domain.Entities;

namespace BlackLagoon.Web.Models.ViewModels
{
    public class HomeVM
    {
        public IEnumerable<Villa>? VillaList { get; set; }
        public DateOnly CheckInDate { get; set;  }
        public DateOnly CheckOutDate { get; set; }
        public int Nights { get; set; }
    }
}
