using System;
using System.Runtime.InteropServices;

namespace Nitrogen_FrontEnd.Utilities
{
    public static class InteropRelease
    {

        public static void ReleaseObject(object obj)
        {
            try
            {
                Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                Console.WriteLine("Exception Occurred while releasing object " + ex.ToString());
            }
            finally
            {
                GC.Collect();
            }
        }
    }
}
