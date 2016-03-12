using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using EnemyFortressServer.Utilities;
using Utilities;

namespace EnemyFortressServer
{

    /// <summary>
    /// Server for game
    /// </summary>
    class Server
    {
        public const int PORT = 55250;      // Port to be used

        private TcpListener listener;       // Listens for incomming traffic
        public List<Client> clients { get; private set; }       // List of all active clients
        public List<Team> teams { get; private set; }

        private Thread connectionThread;        // Connection thread
        private Thread disconnectionThread;     // Disconnection thread

        private object mylock;              // Lock
        private volatile bool running;      // If application is running
        private int total;                  // Total of number of connections made

        /// <summary>
        /// Constructor
        /// </summary>
        public Server()
        {
            Console.WriteLine("EnemyFortress dedicated server 2016\n");
            Console.WriteLine("Authors: Robin Andersson & Simon Berggren\n");

            running = true;
            mylock = new object();
            teams = new List<Team>();
            ProcessMap();
        }

        /// <summary>
        /// Makes client join a team
        /// </summary>
        public Team TeamClient(Client client)
        {
            Team team = teams[0];   // Takes firts team

            for(int i = 0; i < teams.Count; i++)    // Goes thru all teams
            {
                if (team.members > teams[i].members)    // check if current refrence has the least amount of members
                    team = teams[i];
            }
            team.members++;
            return team;
        }

        private void ProcessMap()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "EnemyFortress Map | *.efmap";
            dialog.DefaultExt = "efmap";
            DialogResult result = dialog.ShowDialog();

            if (result == DialogResult.Cancel)
                return;
            if (result == DialogResult.OK)
            {
                StreamReader reader = new StreamReader(dialog.FileName);
                while (!reader.EndOfStream)
                {
                    string[] data = reader.ReadLine().Split('|');
                    int posx = 0, posy = 0;
                    switch (data[0])
                    {
                        case "spawn":
                            int teamid = int.Parse(data[1]);
                            string teamname = data[2];
                            posx = int.Parse(data[3]);
                            posy = int.Parse(data[4]);

                            // Check if teamname already exist
                            for (int i = 0; i < teams.Count; i++)
                            {
                                if(teamname == teams[i].teamName)
                                {
                                    teams[i].spawns.Add(new Point(posx, posy));
                                }
                            }

                            Team team = new Team();
                            team.teamID = teamid;
                            team.teamName = teamname;
                            team.spawns.Add(new Point(posx, posy));
                            teams.Add(team);
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Run method
        /// </summary>
        public void Run()
        {
            clients = new List<Client>();
            listener = new TcpListener(IPAddress.Any, PORT);

            connectionThread = new Thread(new ThreadStart(ConnectionListener));
            disconnectionThread = new Thread(new ThreadStart(DisconnectionListener));

            connectionThread.Start();
            disconnectionThread.Start();
        }

        /// <summary>
        /// Listens for incomming clients
        /// </summary>
        private void ConnectionListener()
        {
            listener.Start();
            Console.WriteLine("Listens on port: " + PORT.ToString());
            Console.WriteLine("IP: " + IPAddress.Any);

            while (running)
            {
                Client user = null;
                try
                {
                    TcpClient client = listener.AcceptTcpClient(); // Waiting for connection

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
            }
        }

        /// <summary>
        /// Listens for disconnected clients
        /// </summary>
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
                            string msg = clients[i].Alias + " disconnected";
                            BroadcastMsg(Commands.Send(Command.RemoveClient, clients[i].id));
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

        /// <summary>
        /// Sends text message to all clients
        /// </summary>
        /// <param name="msg">Text to be sent</param>
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
