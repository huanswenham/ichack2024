using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class NetworkManager {
    const string serverIp = "146.169.53.165";
    const int serverPort = 8080;
    const int MAX_SIZE = 2 << 20; // 2KB
    public static void UploadFile(string inputPath, string outputPath) {
        using (var client = new TcpClient(serverIp, serverPort))
        using (var stream = client.GetStream())
        {
            // Read the file to send (customize the path)
            string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            byte[] fileBytes = File.ReadAllBytes(inputPath);

            // Convert to base64 and send to server (maybe no need to convert to base64, just send the bytes directly?)
            Message message = new Message {
                type = "upload",
                filename = outputPath,
                timestamp = timestamp,
                data = fileBytes
            };

            string json = message.to_json();
            Debug.Log(json);
            byte[] jsonBytes = Encoding.UTF8.GetBytes(json);
            stream.Write(jsonBytes, 0, jsonBytes.Length);

            // Set in PlayerPrefs the timestamp of sending the file
            PlayerPrefs.SetString("timestamp", timestamp);

            Console.WriteLine("File sent successfully!");
        }
    }

    public static void DownloadFile(string inputPath, string outputPath) {
        using (var client = new TcpClient(serverIp, serverPort))
        using (var stream = client.GetStream())
        {
            // Send the timestamp of the file to download
            string timestamp = PlayerPrefs.GetString("timestamp");
            Message message = new Message {
                type = "download",
                filename = inputPath,
                timestamp = timestamp,
                data = new byte[0]
            };

            string json = message.to_json();
            byte[] jsonBytes = Encoding.UTF8.GetBytes(json);
            stream.Write(jsonBytes, 0, jsonBytes.Length);

            // Receive the file from the server
            byte[] buffer = new byte[MAX_SIZE];
            int bytesRead = stream.Read(buffer, 0, MAX_SIZE);

            // Save the file in the path
            byte[] data = new byte[bytesRead];
            Array.Copy(buffer, data, bytesRead);
            System.IO.File.WriteAllBytes(outputPath, data);

            Console.WriteLine("File received successfully!");
        }
    }
}

// type: 'upload',
// filename: 'example.txt',
// timestamp: 123456789,
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
        type = parts[1].Split('"')[1];
        filename = parts[3].Split('"')[1];
        timestamp = parts[5].Split('"')[1];
        data = Convert.FromBase64String(parts[7].Split('"')[1]);
    }
}
