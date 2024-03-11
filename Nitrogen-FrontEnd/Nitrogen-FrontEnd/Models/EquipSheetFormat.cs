using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nitrogen_FrontEnd.Models
{
    public class EquipSheetFormat
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public int StartingDataLine { get; set; }
        public int EquipDbFieldToExcelColumnMapId { get; set; }
    }
}
