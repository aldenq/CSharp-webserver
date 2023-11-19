using System;
using System.Collections.Generic;
using System.Text;
namespace HTTP
{
    public class HttpResponse
    {
        private string StatusLine { get; set; }
        private Dictionary<string, string> Headers { get; set; }
        private byte[] Body { get; set; }

        public HttpResponse(string statusCode = "200 OK", string httpVersion = "HTTP/1.1")
        {
            StatusLine = $"{httpVersion} {statusCode}";
            Headers = new Dictionary<string, string>();
            Body = Array.Empty<byte>();
        }

        public void SetHeader(string key, string value)
        {
            Headers[key] = value;
        }

        public void SetBody(string body, string contentType = "text/plain")
        {
            Body = Encoding.UTF8.GetBytes(body);
            SetHeader("Content-Type", contentType);
            SetHeader("Content-Length", Body.Length.ToString());
        }

        public void SetBody(byte[] body, string contentType = "application/octet-stream")
        {
            Body = body;
            SetHeader("Content-Type", contentType);
            SetHeader("Content-Length", Body.Length.ToString());
        }

        public byte[] ToBytes()
        {
            var responseBuilder = new StringBuilder();
            responseBuilder.AppendLine(StatusLine);

            foreach (var header in Headers)
            {
                responseBuilder.AppendLine($"{header.Key}: {header.Value}");
            }

            responseBuilder.AppendLine(); // End of headers

            return Encoding.UTF8.GetBytes(responseBuilder.ToString()).Concat(Body).ToArray();
        }
    }
}