using System.Text;
using System.Xml;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Utilities.Middleware;

public class JsonToXmlMiddleware
{
    private readonly RequestDelegate _next;

    public JsonToXmlMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        if (context.Request.Query["format"].ToString() != "xml")
        {
            await _next(context);
            return;
        }

        var originalBodyStream = context.Response.Body;
        try
        {
            using var newResponseBody = new MemoryStream();
            context.Response.Body = newResponseBody;

            await _next(context);
            
            newResponseBody.Seek(0, SeekOrigin.Begin);
            var jsonResponse = await new StreamReader(newResponseBody).ReadToEndAsync();    
            
            var xmlResponse = ConvertJsonToXml(jsonResponse);
            if (xmlResponse != "")
            {
                context.Response.ContentType = "application/xml";
                
                var xmlBytes = Encoding.UTF8.GetBytes(xmlResponse);
                await originalBodyStream.WriteAsync(xmlBytes, 0, xmlBytes.Length);
                return;
            }
            
            var jsonBytes = Encoding.UTF8.GetBytes(jsonResponse);
            await originalBodyStream.WriteAsync(jsonBytes, 0, jsonBytes.Length);
        }
        finally
        {
            context.Response.Body = originalBodyStream;
        }
    }

    private string ConvertJsonToXml(string jsonResponse)
    {
        XmlDocument? doc = JsonConvert.DeserializeXmlNode(jsonResponse, "response");
        return doc?.OuterXml ?? "";
    }
}