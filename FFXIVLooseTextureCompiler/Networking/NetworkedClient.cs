using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;
using Anamnesis.Penumbra;

namespace FFXIVLooseTextureCompiler.Networking {
    public class NetworkedClient : IDisposable {
        private bool disposedValue;
        private TcpListener listener;
        TcpClient client;
        int connectionAttempts = 0;
        public NetworkedClient(string ipAddress) {
            try {
                client = new TcpClient(new IPEndPoint(IPAddress.Any, 5800));
                client.Connect(new IPEndPoint(IPAddress.Parse(ipAddress), 5800));
            } catch {
                MessageBox.Show("Failed to send.");
            }
        }
        public NetworkedClient() {
        }

        //TcpListener listener = new TcpListener(new IPEndPoint(IPAddress.Any, 5800));
        public void SendModFolder(string ipAddress, string modName, string penumbraFolder) {
            //  try {
            if (!string.IsNullOrEmpty(modName)) {
                try {
                    BinaryWriter writer = new BinaryWriter(client.GetStream());
                    string path = Path.Combine(penumbraFolder, modName + ".zip");
                    if (File.Exists(path)) {
                        File.Delete(path);
                    }
                    ZipFile.CreateFromDirectory(Path.Combine(penumbraFolder, modName), path);
                    using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read)) {
                        writer.Write(modName);
                        writer.Write(fileStream.Length);
                        fileStream.CopyTo(writer.BaseStream);
                    }
                    MessageBox.Show("Mod files synced");
                } catch {
                    if (connectionAttempts < 10) {
                        if (client != null) {
                            client.Close();
                        }
                        client = new TcpClient(new IPEndPoint(IPAddress.Any, 5800));
                        try {
                            client.Connect(new IPEndPoint(IPAddress.Parse(ipAddress), 5800));
                        } catch {
                            MessageBox.Show("Failed to send.");
                        }
                        SendModFolder(ipAddress, modName, penumbraFolder);
                        connectionAttempts++;
                    } else {
                        MessageBox.Show("Failed to send.");
                    }
                }
            }
        }

        public void StartServer(string penumbraFolder) {
            listener = new TcpListener(new IPEndPoint(IPAddress.Any, 5800));
            listener.Server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, 1);
            listener.Start();
            while (true) {
                //try {
                using (TcpClient client = listener.AcceptTcpClient()) {
                    using (BinaryReader reader = new BinaryReader(client.GetStream())) {
                        while (true) {
                            string modName = reader.ReadString();
                            long length = reader.ReadInt64();
                            string path = Path.Combine(penumbraFolder, modName + ".zip");
                            if (File.Exists(path)) {
                                File.Delete(path);
                            }
                            using (FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write)) {
                                CopyStream(reader.BaseStream, fileStream, (int)length);
                            }
                            if (Directory.Exists(Path.Combine(penumbraFolder, modName))) {
                                Directory.Delete(Path.Combine(penumbraFolder, modName), true);
                            }
                            ZipFile.ExtractToDirectory(path, Path.Combine(penumbraFolder, modName), true);
                            Thread.Sleep(500);
                            PenumbraHttpApi.Reload(Path.Combine(penumbraFolder, modName), modName);
                            Thread.Sleep(500);
                            PenumbraHttpApi.Redraw(0);
                        }
                    }
                }
                //} catch {
                //    MessageBox.Show("Lost connection to client");
                //}
            }
            listener.Stop();
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
                    // TODO: dispose managed state (managed objects)
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
