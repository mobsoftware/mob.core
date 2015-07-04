using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Mob.Core.Helpers
{
    public static class EnumHelpers
    {
        public static Dictionary<int, string> ToDictionary<T>()
        {
            if (!typeof(T).IsEnum)
                throw new ArgumentException("Type must be an enum");
            return Enum.GetValues(typeof(T))
                .Cast<T>()
                .ToDictionary(t =>
                   (int)Convert.ChangeType(t, t.GetType()),
                   t => t.ToString()
                );
        }

        public static List<SelectListItem> ToSelectListItems<T>(int? SelectedValue)
        {
            var selectListItem = new List<SelectListItem>();
            if (!typeof(T).IsEnum)
                throw new ArgumentException("Type must be an enum");

            var dictionary = ToDictionary<T>();
            foreach (var kv in dictionary)
            {
                var listItem = new SelectListItem()
                {
                    Text = kv.Value.ToString(),
                    Value = kv.Key.ToString(),
                    Selected = SelectedValue != null && SelectedValue.Value == kv.Key
                };
                selectListItem.Add(listItem);
            }

            return selectListItem;

        } 
    }
}
