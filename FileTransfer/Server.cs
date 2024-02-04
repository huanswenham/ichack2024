using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

class Server
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
                byte[] buffer = new byte[MAX_BUFFER_SIZE];
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                string receivedMessage = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Console.WriteLine(receivedMessage);
                Message message = new Message();
                message.from_json(receivedMessage);
                Console.WriteLine($"Received: {message.to_json()}");

                if (message.type == "upload") {
                    // Convert the base64 string to a byte array and write it to a file
                    byte[] data = message.data;
                    System.IO.File.WriteAllBytes(message.filename, data);
                } else if (message.type == "download") {
                    // Read the file and send it back to the client
                    byte[] data = System.IO.File.ReadAllBytes(message.filename);
                    // Open filestream for transferring file
                    using (var fileStream = new FileStream(message.filename, FileMode.Create, FileAccess.Write))
                    {
                        fileStream.Write(data, 0, data.Length);
                    }
                }
            }
        }
    }
}

public class Message {
    public string type;
    public string filename;
    public string timestamp;
    public byte[] data;

    public string to_json() {
        return "{\"type\":\"" + type + "\",\"filename\":\"" + filename + "\",\"timestamp\":\"" + timestamp + "\",\"data\":\"" + Convert.ToBase64String(data) + "\"}";
    }

    public void from_json(string json) {
        // Split on the colon
        string[] parts = json.Split(':'); 
        type = parts[1].Split('\"')[1];
        filename = parts[2].Split('\"')[1];
        timestamp = parts[3].Split('\"')[1];
        data = Convert.FromBase64String(parts[4].Split('\"')[1]);
    }
}