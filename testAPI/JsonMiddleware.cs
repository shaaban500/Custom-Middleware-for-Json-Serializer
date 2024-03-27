using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

public class JsonMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    
    {
        string requestBody;
        using (var reader = new StreamReader(context.Request.Body))
        {
            requestBody = await reader.ReadToEndAsync();
            requestBody = requestBody.Replace("\\U202f", "");
            var hasPM = requestBody.Contains("pm");

            // Deserialize the JSON string to a dictionary
            if (requestBody != "")
            {
                var jsonObject = JsonConvert.DeserializeObject<Dictionary<string, object>>(requestBody);


                // Iterate over each key-value pair in the dictionary
                foreach (var kvp in jsonObject)
                {
                    jsonObject[kvp.Key] = TryParsee(hasPM, jsonObject, kvp);
                }
                // Serialize the modified dictionary back to string
                requestBody = JsonConvert.SerializeObject(jsonObject);
            }
        }

        // Convert the modified JSON string back to bytes
        var bytes = Encoding.UTF8.GetBytes(requestBody);

        // Update the request body with the modified bytes
        context.Request.Body = new MemoryStream(bytes);
        context.Request.ContentLength = bytes.Length;

        // Call the next middleware in the pipeline
        await next(context);
    }

    private static object TryParsee(bool hasPM, Dictionary<string, object>? jsonObject, KeyValuePair<string, object> kvp)
    {
        var dateeeeeeee = kvp.Value.ToString();
        var date = dateeeeeeee.Substring(0, Math.Min(19, dateeeeeeee.Length));

        if (hasPM)
        {
            try
            {
                var dateTochange = DateTime.Parse(date);
                dateTochange = dateTochange.AddHours(12);
                date = dateTochange.ToString("yyyy-MM-ddTHH:mm:ss");
            }
            catch (Exception)
            {
                return kvp.Value;
            }

        }

        return date;
    }
}
