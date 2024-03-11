using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nitrogen_FrontEnd.Models
{
    public class Project
    {
        public string ProjectNumber { get; set; }
        public string Description { get; set; }
        public int EquipSheetFormatId { get; set; }
        public int IoSheetFormatId { get; set; }
    }
}