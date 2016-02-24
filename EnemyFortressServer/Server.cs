using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace EnemyFortressServer
{
    class Server
    {
        public const int PORT = 55250;

        private TcpListener listener;
        private List<Client> clients;

        private Thread connectionThread;
        private Thread disconnectionThread;

        private object mylock;
        private volatile bool running;
        private int total;

        public Server()
        {
            running = true;
            mylock = new object();
        }

        public void Run()
        {
            clients = new List<Client>();
            listener = new TcpListener(IPAddress.Any, PORT);

            connectionThread = new Thread(new ThreadStart(ConnectionListener));
            disconnectionThread = new Thread(new ThreadStart(DisconnectionListener));

            connectionThread.Start();
            disconnectionThread.Start();
        }

        private void ConnectionListener()
        {
            listener.Start();
            Console.WriteLine("Listens on: " + PORT.ToString());
            Console.WriteLine(IPAddress.Any);

            while (running)
            {
                Console.WriteLine("Waiting for connection...");
                Client user = null;
                try
                {
                    TcpClient client = listener.AcceptTcpClient();

                    user = new Client(this, total);
                    total++;

                    Task task = new Task(new Action<object>(user.HoldConnection), client);
                    task.Start();
                }
                catch (Exception)
                {
                    return;
                }

                lock (mylock)
                    clients.Add(user);

                Console.WriteLine("User Connected");
            }
        }

        private void DisconnectionListener()
        {
            while (running)
            {
                lock (mylock)
                {
                    for (int i = 0; i < clients.Count; i++)
                    {
                        if (!clients[i].Connected)
                        {
                            string msg = clients[i].Username + " disconnected";

                            clients[i].Close();
                            clients.RemoveAt(i);

                            Console.WriteLine(msg);
                            BroadcastMsg(msg);
                            break;
                        }
                    }
                }
            }
        }

        public void BroadcastMsg(string msg)
        {
            lock (mylock)
            {
                for (int i = 0; i < clients.Count; i++)
                {
                    if (clients[i].Connected)
                        clients[i].WriteMessage(msg);
                }
            }
        }
    }
}
