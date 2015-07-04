using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Mob.Core.Extensions
{
    public static class UtilityExtensions
    {
        public static bool IsAjaxRequest(this HttpRequest request, bool pjaxOnly = false)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }
            if (pjaxOnly)
            {
                return ((request.Headers["X-PJAX"] != null));
            }

            return (request["X-Requested-With"] == "XMLHttpRequest") || ((request.Headers["X-Requested-With"] == "XMLHttpRequest"));
        }

        public static bool IsNumeric(this string str)
        {
            float output;
            return float.TryParse(str, out output);
        }

        public static bool IsInteger(this string str)
        {
            int output;
            return int.TryParse(str, out output);
        }

       

       
    }
}
