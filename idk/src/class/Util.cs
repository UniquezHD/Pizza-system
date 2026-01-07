using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace idk.src.Class
{
    internal class Util
    {
        public bool IsJsonValid(JsonElement json)
        {
            if (json.GetArrayLength() < 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
