using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nitrogen_FrontEnd.Models
{
    public class Equipment
    {
        public int Id { get; set; }
        public string ProjectNumber { get; set; }
        public string Area { get; set; }
        public string EquipmentId { get; set; }
        public string EquipmentSubId { get; set; }
        public int? ParentEquipmentId { get; set; }
        public string CustomerEquipmentId { get; set; }
        public string Description { get; set; }
        public string ControlPanel { get; set; }
        public string Notes { get; set; }
        public string ModelNumber { get; set; }
        public int VendorId { get; set; }
        public int UserDefinitionId { get; set; }
        public int ExcelRowNumber { get; set; }
    }
}
