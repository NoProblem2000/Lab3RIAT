using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;

namespace Lab3RIAT
{
    class CreateServer
    {
        ISerializer iSerializer;
        HttpListener httpListener;
        HttpListenerContext context;
        int port;
        string Output;

        public CreateServer(ISerializer iSerializer, int port)
        {
            this.port = port;
            this.iSerializer = iSerializer;
        }
        public void OkStatusCode()
        {
            context.Response.StatusCode = (int)HttpStatusCode.OK;
            context.Response.OutputStream.Dispose();
        }
        public void Listen()
        {
            httpListener = new HttpListener();
            httpListener.Prefixes.Add($"http://127.0.0.1:{port}/");
            httpListener.Start();
            while (httpListener.IsListening)
            {
                context = httpListener.GetContext();
                var url = context.Request.RawUrl;
                string input;
                string output;
                try
                {
                    var toInvoke = GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public).Where(x => x.ReturnType == typeof(string)).FirstOrDefault(x => url.Contains(x.Name));
                    input = new StreamReader(context.Request.InputStream).ReadToEnd();
                    output = (string)toInvoke.Invoke(this, new object[] { input });
                    new StreamWriter(context.Response.OutputStream).Write(output);
                }
                catch (Exception)
                {
                    
                }
            }
        }

        public string Ping(string input)
        {
            OkStatusCode();
            return string.Empty;
        }

        public string GetAnswer(string input)
        {
           byte[] val = iSerializer.Serialize(input);
           using (var streamReader = context.Response.OutputStream)
                streamReader.Write(val, 0, val.Length);
            return Output = val.ToString();
        }

        public string PostInputData(string inputData)
        {
            Output = PrepareResponse(inputData);
            OkStatusCode();
            return string.Empty;
        }

        public string Stop(string input)
        {
            OkStatusCode();
            httpListener.Stop();
            return null;
        }

        public string PrepareResponse(string inputLine)
        {
            var input = iSerializer.Deserialize<Input>(Encoding.UTF8.GetBytes(inputLine));
            var output = input.DoOutPut();
            var outputLine = iSerializer.Serialize(output);
            return Encoding.UTF8.GetString(outputLine);
        }
    }
}