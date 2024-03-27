using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

public class ModifyDateTimeAttribute : IActionFilter
{

    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (context.ActionArguments.Count > 0)
        {
            foreach (var argument in context.ActionArguments.Values)
            {
                ModifyDateTime(argument);
            }
        }

    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
    }

    private void ModifyDateTime(object obj)
    {
        // Get the type of the object
        var objectType = obj.GetType();

        // Iterate through properties of the object
        foreach (PropertyInfo property in objectType.GetProperties())
        {
            // Check if the property is of type DateTime
            if (property.PropertyType == typeof(DateTime))
            {
                // Assuming the date components are stored under "date" and "time" properties
                int day = (int)property.GetValue(obj.GetType().GetProperty("Day").GetValue(obj));
                int month = (int)property.GetValue(obj.GetType().GetProperty("Month").GetValue(obj));
                int year = (int)property.GetValue(obj.GetType().GetProperty("Year").GetValue(obj));
                int hour = (int)property.GetValue(obj.GetType().GetProperty("Hour").GetValue(obj));
                int minute = (int)property.GetValue(obj.GetType().GetProperty("Minute").GetValue(obj));
                int second = (int)property.GetValue(obj.GetType().GetProperty("Second").GetValue(obj));

                // Construct DateTime object manually
                var dateTime = new DateTime(year, month, day, hour, minute, second);

                // Set the value of the DateTime property
                property.SetValue(obj, dateTime);
            }
        }
    }
}
