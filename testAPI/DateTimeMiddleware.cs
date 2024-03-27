using System.Reflection;
using System.Text;
using Newtonsoft.Json;

public class DateTimeMiddleware
{
    private readonly RequestDelegate _next;

    public DateTimeMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        // Intercept the request
        if (context.Request.Method == "POST" || context.Request.Method == "PUT")
        {
            // Read the request body
            using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8, true, 1024, true))
            {
                var body = await reader.ReadToEndAsync();

                // Modify DateTime objects in the request body
                body = ModifyDateTime(body);

                // Rewrite the modified request body
                var byteArray = Encoding.UTF8.GetBytes(body);
                context.Request.Body = new MemoryStream(byteArray);
            }
        }

        await _next(context);
    }

    private string ModifyDateTime(string requestBody)
    {
        // Deserialize the request body to determine its structure
        var jsonObject = JsonConvert.DeserializeObject<dynamic>(requestBody);

        // Get the type of the DTO
        var dtoType = jsonObject.GetType();

        // Iterate through properties of the DTO
        foreach (PropertyInfo property in dtoType.GetProperties())
        {
            // Check if the property is of type DateTime
            if (property.PropertyType == typeof(DateTime))
            {
                // Assuming the date components are stored under "date" and "time" properties
                int day = jsonObject.date.day;
                int month = jsonObject.date.month;
                int year = jsonObject.date.year;
                int hour = jsonObject.time.hour;
                int minute = jsonObject.time.minute;
                int second = jsonObject.time.second;

                // Construct DateTime object manually
                var dateTime = new DateTime(year, month, day, hour, minute, second);

                // Set the value of the DateTime property
                property.SetValue(jsonObject, dateTime);
            }
        }

        // Serialize the modified object back to JSON for replacement in the request body
        requestBody = JsonConvert.SerializeObject(jsonObject);

        return requestBody;
    }
}
