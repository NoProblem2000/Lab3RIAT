using System;

namespace Lab3RIAT
{
    class Program
    {
        static void Main(string[] args)
        {
            int port = 80;
            ISerializer iSerializer = new JsonSerializer();
            if(!int.TryParse(Console.ReadLine(),out port))
                return;
            CreateServer createServer = new CreateServer(iSerializer,port);
            createServer.Listen();
        }
    }
}
