using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace LooseTextureCompilerFileRelay {
    public class ReceiveFromClientManager : StreamUtilities {
        private TcpClient client;
        private FileManager fileManager;

        public ReceiveFromClientManager(TcpClient client, FileManager fileManager) {
            this.client = client;
            this.fileManager = fileManager;
        }

        public void ReceiveFromClient() {
            using (BinaryReader reader = new BinaryReader(client.GetStream())) {
                while (true) {
                    try {
                        Guid id = Guid.Parse(reader.ReadString());
                        string modName = reader.ReadString();
                        long length = reader.ReadInt64();
                        MemoryStream memoryStream = new MemoryStream();
                        CopyStream(reader.BaseStream, memoryStream, (int)length);
                        if (!fileManager.Files.ContainsKey(id)) {
                            fileManager.Files.Add(id, new FileIdentifier(id, modName, memoryStream));
                        } else {
                            if (fileManager.Files[id].UsedUp) {
                                fileManager.Files[id] = new FileIdentifier(id, modName, memoryStream);
                            }
                        }
                        Console.WriteLine("Receiving file for " + fileManager.Files[id].ModName + " to send to " + fileManager.Files[id].Identifier);
                    } catch {
                        Console.WriteLine("Dropped receiver!");
                        client.Client.Shutdown(SocketShutdown.Both);
                        client.Client.Disconnect(true);
                        client.Close();
                        client.Dispose();
                        break;
                    }
                }
            }
        }
    }
}
