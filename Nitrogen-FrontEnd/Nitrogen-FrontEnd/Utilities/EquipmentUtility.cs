using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nitrogen_FrontEnd.Utilities
{
    public static class EquipmentUtility
    {
        public static Dictionary<string, string> ExtractEquipmentIdAndSubId(string equipmentListNum)
        {
            string[] ids = equipmentListNum.Split('.');

            Dictionary<string, string> idDict = new Dictionary<string, string>()
            {
                { "id" , ids[0] },
            };

            if (ids.Length > 1 && ids[1] != "0" && ids[1] != "00")
            {
                idDict.Add("subId", ids[1]);
            }

            return idDict;
        }
    }
}

