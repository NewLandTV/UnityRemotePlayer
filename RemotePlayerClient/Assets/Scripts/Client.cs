using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class Client : MonoBehaviour
{
    private Socket socket;

    private void Awake()
    {
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        socket.Connect("192.168.0.5", 2894);
    }

    private void OnApplicationQuit()
    {
        if (socket != null)
        {
            socket.Close();

            socket = null;
        }
    }

    public void Send(string message)
    {
        socket.Send(Encoding.ASCII.GetBytes(message), SocketFlags.None);
    }
}
