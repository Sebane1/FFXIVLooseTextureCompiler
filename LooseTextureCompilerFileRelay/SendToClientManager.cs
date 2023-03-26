using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace LooseTextureCompilerFileRelay {
    public class SendToClientManager : StreamUtilities {
        private TcpClient client;
        private FileManager fileManager;

        public SendToClientManager(TcpClient client, FileManager fileManager) {
            this.client = client;
            this.fileManager = fileManager;
        }
        public void SendToClient() {
            using (BinaryWriter writer = new BinaryWriter(client.GetStream())) {
                using (BinaryReader reader = new BinaryReader(client.GetStream())) {
                    Guid id = Guid.NewGuid();
                    writer.Write(id.ToString());
                    Console.WriteLine("Connected to " + id.ToString());
                    while (true) {
                        try {
                            if (fileManager.Files.ContainsKey(id)) {
                                FileIdentifier identifier = fileManager.Files[id];
                                if (!identifier.UsedUp) {
                                    Console.WriteLine("Sending " + identifier.ModName + $" ({identifier.MemoryStream.Length} bytes)" + " to " + id.ToString());
                                    writer.Write((byte)1);
                                    writer.Write(identifier.ModName);
                                    writer.Write(identifier.MemoryStream.Length);
                                    identifier.MemoryStream.Position = 0;
                                    CopyStream(identifier.MemoryStream, writer.BaseStream, (int)identifier.MemoryStream.Length);
                                    writer.Flush();
                                    Console.WriteLine("Finished sending " + identifier.ModName + " to " + id.ToString());
                                    identifier.UsedUp = true;
                                }
                            }
                            writer.Write((byte)0);
                            switch (reader.Read()) {
                                case 2:
                                    client.Close();
                                    client.Dispose();
                                    break;
                                case 0:
                                    break;
                            }
                            Thread.Sleep(1000);
                        } catch {
                            client.Close();
                            client.Dispose();
                            Console.WriteLine("Lost connection to " + id.ToString());
                            break;
                        }
                    }
                }
            }
        }
    }
}
