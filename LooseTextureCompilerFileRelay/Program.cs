using System.IO.Compression;
using System.Net;
using System.Net.Sockets;
using System.Reflection.PortableExecutable;

namespace LooseTextureCompilerFileRelay {
    internal class Program {
        static TcpListener fileReceiverListener = null;
        static TcpListener fileSenderListener = null;
        static void Main(string[] args) {
            fileReceiverListener = new TcpListener(5900);
            fileSenderListener = new TcpListener(5800);
            fileReceiverListener.Start();
            fileSenderListener.Start();
            FileManager fileManager = new FileManager();
            Console.WriteLine("Starting server");
            Thread thread = new Thread(new ThreadStart(delegate {
                while (true) {
                    TcpClient client = fileReceiverListener.AcceptTcpClient();
                    ReceiveFromClientManager receiveFromClientManager = new ReceiveFromClientManager(client, fileManager);
                    Thread thread = new Thread(new ThreadStart(receiveFromClientManager.ReceiveFromClient));
                    thread.Start();
                }
            }));
            thread.Start();

            thread = new Thread(new ThreadStart(delegate {
                while (true) {
                    TcpClient client = fileSenderListener.AcceptTcpClient();
                    SendToClientManager sendToClientManager = new SendToClientManager(client, fileManager);
                    Thread thread = new Thread(new ThreadStart(sendToClientManager.SendToClient));
                    thread.Start();
                }
            }));
            thread.Start();

            while (true) {
                Thread.Sleep(5000);
            }
        }
    }
}