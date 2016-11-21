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
            int port;
            string Output;

        public CreateServer(ISerializer iSerializer, int port)
        {
            this.port = port;
            this.iSerializer = iSerializer;
        }

        public void Listen()
        {
            httpListener = new HttpListener();
            httpListener.Prefixes.Add(string.Format("http://127.0.0.1:{0}/", port));
            httpListener.Start();
            while (httpListener.IsListening)
            {
                var context = httpListener.GetContext();
                var url = context.Request.RawUrl;
                string input;
                string output;
                var toInvoke = GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public).Where(x => x.ReturnType == typeof(string)).FirstOrDefault(x => url.Contains(x.Name));
                input = new StreamReader(context.Request.InputStream).ReadToEnd();
                output = (string)toInvoke.Invoke(this, new object[]{input});
                new StreamWriter(context.Response.OutputStream).Write(output);
                if (output == null)
                {
                    httpListener.Stop();
                }
            }
        }

        public string Ping()
        {
            return string.Empty;
        }

        public string GetAnswer(string input)
        {
            return Output;
        }

        public string PostInputData(string inputData)
        {
            Output = PrepareResponse(inputData);
            return string.Empty;
        }

        public string Stop(string input)
        {
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