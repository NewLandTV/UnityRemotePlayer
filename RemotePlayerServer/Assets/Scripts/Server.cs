using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class Server : MonoBehaviour
{
    [SerializeField]
    private Player player;

    private Socket listenSocket;
    private Socket clientSocket;
    private Thread receiveThread;

    private Queue<string> receiveBuffer = new Queue<string>();

    private void Awake()
    {
        listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        listenSocket.Bind(new IPEndPoint(IPAddress.Any, 2894));
        listenSocket.Listen(1);

        receiveThread = new Thread(() =>
        {
            clientSocket = listenSocket.Accept();

            while (true)
            {
                byte[] buffer = new byte[1024];

                clientSocket.Receive(buffer, SocketFlags.None);

                string message = Encoding.ASCII.GetString(buffer);

                if (message == null || message.Equals(string.Empty))
                {
                    break;
                }

                receiveBuffer.Enqueue(message);
            }
        });

        receiveThread.Start();
    }

    private IEnumerator Start()
    {
        while (true)
        {
            if (receiveBuffer.Count > 0)
            {
                DataReceive(receiveBuffer.Dequeue());
            }

            yield return null;
        }
    }

    public void OnApplicationQuit()
    {
        if (listenSocket != null)
        {
            listenSocket.Close();
            listenSocket.Dispose();
        }

        if (clientSocket != null)
        {
            clientSocket.Close();
            clientSocket.Dispose();
        }

        if (receiveThread != null)
        {
            receiveThread.Abort();

            receiveThread = null;
        }
    }

    private void DataReceive(string message)
    {
        if (message.Contains("UP"))
        {
            player.MoveByDirection(Vector3.up);
        }
        if (message.Contains("LEFT"))
        {
            player.MoveByDirection(Vector3.left);
        }
        if (message.Contains("DOWN"))
        {
            player.MoveByDirection(Vector3.down);
        }
        if (message.Contains("RIGHT"))
        {
            player.MoveByDirection(Vector3.right);
        }
    }
}
