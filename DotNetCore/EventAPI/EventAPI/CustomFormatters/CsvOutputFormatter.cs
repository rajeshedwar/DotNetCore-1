using EventAPI.Models;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace EventAPI.CustomFormatters
{
    public class CsvOutputFormatter : TextOutputFormatter
    {
        public CsvOutputFormatter()
        {
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/csv"));
            SupportedEncodings.Add(Encoding.UTF8);
            SupportedEncodings.Add(Encoding.Unicode);
        }

        protected override bool CanWriteType(Type type)
        {
            if (typeof(EventData).IsAssignableFrom(type) || typeof(IEnumerable<EventData>).IsAssignableFrom(type))
            {
                return base.CanWriteType(type);
            }
            return false;
        }

        public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
        {
            var response = context.HttpContext.Response;
            var buffer = new StringBuilder();

            if (context.Object is EventData)
            {
                var edata = context.Object as EventData;
                buffer.AppendLine("Id,Title,StartDate,EndDate,Locationm,Speaker,Url");
                buffer.AppendLine($"{edata.Id},{edata.StartDate},{edata.EndDate},{edata.Location},{edata.Speaker},{edata.Url}");
            }
            else if (context.Object is IEnumerable<EventData>)
            {
                var edataList = context.Object as IEnumerable<EventData>;
                buffer.AppendLine("Id,Title,StartDate,EndDate,Locationm,Speaker,Url");
                foreach (var edata in edataList)
                {
                    buffer.AppendLine($"{edata.Id},{edata.StartDate},{edata.EndDate},{edata.Location},{edata.Speaker},{edata.Url}");
                }
            }
            await response.WriteAsync(buffer.ToString());
        }
    }
}
