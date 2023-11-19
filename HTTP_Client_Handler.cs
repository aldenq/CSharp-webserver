using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace HTTP
{
    public class Client_Handler
    {


        private string _httpRequest = string.Empty;
        public Dictionary<string, string> headers = new Dictionary<string, string>();
        public Dictionary<string, string> queryParams = new Dictionary<string, string>();
        public string method =  string.Empty;
        public string uri =  string.Empty;
        public string url =  string.Empty;
        public string httpVersion =  string.Empty;


        private HTTP_Config config;
        public delegate void RouteHandler(Client_Handler client_Handler);

        public NetworkStream stream;
        public string HttpRequest
        {
            get { return _httpRequest; }
        }
        public IReadOnlyDictionary<string, string> Headers => headers;




        private TcpClient client;
        public Client_Handler(TcpClient client, HTTP_Config config)
        {
            this.config = config;
            this.client = client;
            // this.defaultHandler = defaultHandler;

        }




        public void handle_stream()
        {
            // try
            // {
            NetworkStream stream = this.client.GetStream();
            this.stream = stream;
            using (StreamReader reader = new StreamReader(stream, Encoding.ASCII))
            {
                StringBuilder requestBuilder = new StringBuilder();
                string line;
                int contentLength = 0;

                // Read the request line
                string requestLine = reader.ReadLine();
                
                this.ProcessRequestLine(requestLine);

                
                Console.WriteLine(requestLine);
                requestBuilder.AppendLine(requestLine);

                // Read and parse headers
                while ((line = reader.ReadLine()) != null && !string.IsNullOrWhiteSpace(line))
                {
                    requestBuilder.AppendLine(line);
                    var headerParts = line.Split(new[] { ':' }, 2);
                    if (headerParts.Length == 2)
                    {
                        headers[headerParts[0].Trim()] = headerParts[1].Trim();
                    }

                    if (headerParts[0].Equals("Content-Length", StringComparison.OrdinalIgnoreCase))
                    {
                        contentLength = int.Parse(headerParts[1].Trim());
                    }
                }

                // Read the request body if Content-Length is present
                if (contentLength > 0)
                {
                    char[] buffer = new char[contentLength];
                    reader.ReadBlock(buffer, 0, contentLength);
                    requestBuilder.Append(buffer);
                }

                _httpRequest = requestBuilder.ToString();


                this.config.GetRoute(this.url)(this);



            }
            // }
            // catch (SocketException e)
            // {
            //     Console.WriteLine($"SocketException: {e}");
            // }
            // finally
            // {




            client.Close();
            // }

        }

    
    public void ProcessRequestLine(string requestLine)
    {
        string[] parts = requestLine.Split(' ');

        if (parts.Length != 3)
        {
            Console.WriteLine("Invalid HTTP request line.");
            return;
        }

        this.method = parts[0];
        this.uri = parts[1];
        this.httpVersion = parts[2];
        this.url = this.uri.Substring(0,this.uri.IndexOf("?"));
        // Console.WriteLine("Method: " + method);
        Console.WriteLine("URL: " + this.url);
        // Console.WriteLine("HTTP Version: " + httpVersion);

        // Extract and process query parameters if they exist
        this.queryParams = ExtractQueryParameters(uri);
        
    }

    private Dictionary<string, string> ExtractQueryParameters(string uri)
    {
        var queryParams = new Dictionary<string, string>();
        var uriParts = uri.Split('?');

        if (uriParts.Length > 1)
        {
            string queryString = uriParts[1];
            string[] pairs = queryString.Split('&');

            foreach (var pair in pairs)
            {
                string[] keyValue = pair.Split('=');
                if (keyValue.Length == 2)
                {
                    queryParams[keyValue[0]] = keyValue[1];
                }
            }
        }

        return queryParams;
    }

    }
}
