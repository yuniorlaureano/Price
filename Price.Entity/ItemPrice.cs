using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Price.Entity
{
    public class ItemPrice
    {
        public string CanvCode { get; set; }
        public short CanvEdition { get; set; }
        public string BookCode { get; set; }
        public short BookEdition { get; set; }
        public string SectionCode { get; set; }
        public string PlacementCode { get; set; }
        public string ItemCode { get; set; }
        public string SpotColor { get; set; }
        public string DiscountCode { get; set; }
        public Double OldPrice { get; set; }
        public Double Price { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string UserCode { get; set; }
        public string MainItemCode { get; set; }
        public string ExtraGrpCodeMain { get; set; }
    }
}
