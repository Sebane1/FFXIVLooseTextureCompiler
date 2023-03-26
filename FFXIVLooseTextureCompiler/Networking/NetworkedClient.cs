using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;
using Anamnesis.Penumbra;
using System.IO;
using SixLabors.ImageSharp.PixelFormats;

namespace FFXIVLooseTextureCompiler.Networking {
    public class NetworkedClient : IDisposable {
        private bool disposedValue;
        private bool connected;
        private TcpClient listeningClient;
        private TcpClient sendingClient;
        int connectionAttempts = 0;
        private string id;

        public string Id { get => id; set => id = value; }
        public bool Connected { get => connected; set => connected = value; }

        public NetworkedClient(string ipAddress) {
            try {
                sendingClient = new TcpClient(new IPEndPoint(IPAddress.Any, 5900));
                listeningClient = new TcpClient(new IPEndPoint(IPAddress.Any, 5800));
                try {
                    sendingClient.Connect(new IPEndPoint(IPAddress.Parse(ipAddress), 5900));
                    listeningClient.Connect(new IPEndPoint(IPAddress.Parse(ipAddress), 5800));
                } catch {
                    sendingClient.Connect(new IPEndPoint(IPAddress.Parse(ipAddress), 5900));
                    listeningClient.Connect(new IPEndPoint(IPAddress.Parse(ipAddress), 5800));
                }
                connected = true;
            } catch {
                connected = false;
                MessageBox.Show("Failed to connect.");
            }
        }

        public void SendModFolder(string sendID, string modName, string penumbraFolder) {
            //  try {
            if (!string.IsNullOrEmpty(modName)) {
                try {
                    BinaryWriter writer = new BinaryWriter(sendingClient.GetStream());
                    string path = Path.Combine(penumbraFolder, modName + ".zip");
                    if (File.Exists(path)) {
                        File.Delete(path);
                    }
                    ZipFile.CreateFromDirectory(Path.Combine(penumbraFolder, modName), path);
                    writer.Write(sendID);
                    using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read)) {
                        writer.Write(modName);
                        writer.Write(fileStream.Length);
                        CopyStream(fileStream, writer.BaseStream, (int)fileStream.Length);
                        writer.Flush();
                    }
                    MessageBox.Show("Mod files sent!");
                } catch {
                    connected = false;
                    sendingClient.Close();
                    MessageBox.Show("Sending Mod failed!");
                }
            }
        }

        public void ListenForFiles(string penumbraFolder, ConnectionDisplay display) {
            using (TcpClient client = listeningClient) {
                using (BinaryReader reader = new BinaryReader(client.GetStream())) {
                    display.Id = id = reader.ReadString();
                    while (true) {
                        try {
                            string modName = reader.ReadString();
                            long length = reader.ReadInt64();
                            string path = Path.Combine(penumbraFolder, modName + ".zip");
                            using (FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write)) {
                                CopyStream(reader.BaseStream, fileStream, (int)length);
                            }
                            if (Directory.Exists(Path.Combine(penumbraFolder, modName))) {
                                Directory.Delete(Path.Combine(penumbraFolder, modName), true);
                            }
                            ZipFile.ExtractToDirectory(path, Path.Combine(penumbraFolder, modName), true);
                            PenumbraHttpApi.Reload(Path.Combine(penumbraFolder, modName), modName);
                            PenumbraHttpApi.Redraw(0);
                            if (File.Exists(path)) {
                                File.Delete(path);
                            }
                        } catch {
                            connected = false;
                            listeningClient.Close();
                            break;
                        }
                        MessageBox.Show("Mod transfer received");
                    }
                }
            }
        }
        public static void CopyStream(Stream input, Stream output, int bytes) {
            byte[] buffer = new byte[32768];
            int read;
            while (bytes > 0 &&
                   (read = input.Read(buffer, 0, Math.Min(buffer.Length, bytes))) > 0) {
                output.Write(buffer, 0, read);
                bytes -= read;
            }
        }
        protected virtual void Dispose(bool disposing) {
            if (!disposedValue) {
                if (disposing) {
                    sendingClient.Close();
                    listeningClient.Close();
                    listeningClient.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~NetworkedClient()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose() {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
