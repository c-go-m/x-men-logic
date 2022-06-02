using Common.Constants;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLogic.Validation
{
    public static class ValidationDNA
    {

        public static bool ValidSize(this List<string> value)
        {
            var size = value.Count();
            foreach (var item in value)
            {
                if (item.Length != size) {
                    return false;
                }
            }
            return true;
        }

        public static bool ValidMinSize(this List<string> value)
        {
            if (value == null) { return false; };
            return value.Count() >= Constants.MountSequence;
        }

        public static bool ValidData(this List<string> value)
        {
            foreach (var item in value)
            {
                if (item.Where(s => s != 'A' && s != 'T' && s != 'G' && s != 'C').Count() > 0) {
                    return false;
                }
            }
            return true;
        }


    }
}
