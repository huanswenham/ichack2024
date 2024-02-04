using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.IO;

class ARMultiplayerServer
{
    static void Main()
    {
        const int port = 8080;
        const int MAX_BUFFER_SIZE = 2048;
        var server = new TcpListener(IPAddress.Any, port);

        server.Start();

        Console.WriteLine($"WebSocket server started on port {port}. Waiting for connections...");

        while (true)
        {
            using (var client = server.AcceptTcpClient())
            using (var stream = client.GetStream())
            {
                Console.WriteLine("Client connected.");

                // Read data from the client (assuming it's a WebSocket frame)
                byte[] jsonbuffer = new byte[MAX_BUFFER_SIZE];
                int bytesRead = stream.Read(jsonbuffer, 0, jsonbuffer.Length);
                string receivedMessage = Encoding.UTF8.GetString(jsonbuffer, 0, bytesRead);
                Console.WriteLine(receivedMessage);
                Message message = new Message();
                message.from_json(receivedMessage);
                Console.WriteLine($"Received: {message.to_json()}");

                if (message.type == "upload") {
                    using (var fileStream = new FileStream(message.filename, FileMode.Create))
                    {
                        // Read the file data from the client and write it to the FileStream
                        byte[] buffer = new byte[1024];
                        while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            fileStream.Write(buffer, 0, bytesRead);
                        }
                    }
                } else if (message.type == "download") {
                    // Check if file present
                    if (!File.Exists(message.filename)) {
                        Console.WriteLine("File not found");
                        continue;
                    }

                    // Read the file data
                    byte[] fileData = File.ReadAllBytes(message.filename);

                    // Send the file data to the client
                    stream.Write(fileData, 0, fileData.Length);
                }
            }
        }
    }
}

public class Message {
    public string type;
    public string filename;
    public string timestamp;

    public string to_json() {
        return "{\"type\":\"" + type + "\",\"filename\":\"" + filename + "\",\"timestamp\":\"" + timestamp + "\"}";
    }

    public void from_json(string json) {
        // Split on the colon
        string[] parts = json.Split(':');
        type = parts[1].Split('"')[1];
        filename = parts[2].Split('"')[1];
        timestamp = parts[3].Split('"')[1];
    }
}