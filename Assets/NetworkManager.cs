using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class NetworkManager {
    const string serverIp = "146.169.53.165";
    const int serverPort = 8080;
    public static void UploadFile(string inputPath, string outputPath) {        using (var client = new TcpClient(serverIp, serverPort))
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
            };

            string json = message.to_json();
            Debug.Log(json);
            byte[] jsonBytes = Encoding.UTF8.GetBytes(json);
            stream.Write(jsonBytes, 0, jsonBytes.Length);

            // Send the file to the server
            stream.Write(fileBytes, 0, fileBytes.Length);

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
            };

            string json = message.to_json();
            byte[] jsonBytes = Encoding.UTF8.GetBytes(json);
            stream.Write(jsonBytes, 0, jsonBytes.Length);

            // Read the file data from the server
            using (var fileStream = new FileStream(outputPath, FileMode.Create))
            {
                // Read the file data from the client and write it to the FileStream
                byte[] buffer = new byte[1024];
                int bytesRead;
                while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    fileStream.Write(buffer, 0, bytesRead);
                }
            }

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
