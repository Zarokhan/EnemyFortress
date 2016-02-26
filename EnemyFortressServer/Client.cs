using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using Utilities;

namespace EnemyFortressServer
{
    /// <summary>
    /// Simon Berggren - TGSPA14h
    /// Robin Andersson
    /// Represents a client connected to server.
    /// </summary>
    class Client
    {
        public bool Connected { get; private set; }                // Indication if connection with client is made.
        public string Alias { get; private set; }               // Client's username on server.
        public int id { get; private set; }                                // Client's unique id.

        int x, y;
        float latency;
        int ping;

        private Server parent;
        private NetworkStream stream;                                 // Network stream used for reading / writing data to / from client.
        private BinaryReader reader;                                     // Reads data sent from client.
        private BinaryWriter writer;                                       // Writes data to client.

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="id">For keeping track of clients.</param>
        public Client(Server parent, int id)
        {
            this.parent = parent;
            this.id = id;
            Connected = true;
        }

        /// <summary>
        /// Handles connection, constantly looking for data to read from client.
        /// </summary>
        public void HoldConnection(object client)
        {
            stream = ((TcpClient)client).GetStream();
            writer = new BinaryWriter(stream, Encoding.ASCII);
            reader = new BinaryReader(stream, Encoding.ASCII);

            WriteMessage("Connected to server");

            // Send id to game client
            WriteMessage(Commands.Send(Command.SendID, id));

            // Spawns client into world
            Random rnd = new Random(DateTime.Now.Second);
            x = rnd.Next(1280);
            y = rnd.Next(720);
            WriteMessage(Commands.Send(Command.Spawn, x + "|" + y));

            // Send all other game clients to game client
            for(int i = 0; i < parent.clients.Count; i++)
            {
                // We dont need to send ourself
                if (parent.clients[i].id == id)
                    continue;

                WriteMessage(Commands.Send(Command.SendClient, parent.clients[i].id + "|" + parent.clients[i].Alias + "|" + parent.clients[i].x + "|" + parent.clients[i].y));
            }

            while (Connected)
            {
                if (reader.BaseStream.CanRead)
                {
                    try
                    {
                        string msg = reader.ReadString();

                        if (msg.Contains(Commands.KEY))
                            HandleConnectionCommand(msg);
                        else
                        {
                            msg = Alias + ": " + msg;
                            Console.WriteLine(msg);
                            parent.BroadcastMsg(msg);
                        }
                    }
                    catch (Exception)
                    {
                        Connected = false;
                    }
                }
            }
        }

        /// <summary>
        /// Writes a message to client.
        /// </summary>
        /// <param name="message">Message to send.</param>
        public void WriteMessage(string message)
        {
            if (!Connected || !stream.CanWrite || !stream.CanRead)
                return;

            writer.Write(message);
            writer.Flush();
        }

        /// <summary>
        /// Closes connection with client.
        /// </summary>
        public void Close()
        {
            stream.Close();
            reader.Close();
            writer.Close();
        }

        /// <summary>
        /// Handles messages from client
        /// </summary>
        /// <param name="msg">Message from server to handle.</param>
        private void HandleConnectionCommand(string msg)
        {
            string[] splitmsg = msg.Split('|');
            int command = int.Parse(splitmsg[1]);

            switch (command)
            {
                case (int)Command.Ping: // KEY|COMMAND|ID|Ping
                    if (int.Parse(splitmsg[2]) != id)
                        return;

                    ping = int.Parse(splitmsg[3]);
                    parent.BroadcastMsg(Commands.Send(Command.Ping, splitmsg[2] + "|" + ping));
                    break;
                case (int)Command.Latency:  // KEY|COMMAND|ID|MILLISECONDS
                    if (int.Parse(splitmsg[2]) != id)
                        return;

                    latency = DateTime.Now.Millisecond - int.Parse(splitmsg[3]);
                    parent.BroadcastMsg(Commands.Send(Command.Latency, splitmsg[2] + "|" + (int)latency));
                    break;
                case (int)Command.Movement: // KEY|COMMAND|ID|X|Y
                    int receivedid = int.Parse(splitmsg[2]);
                    x = int.Parse(splitmsg[3]);
                    y = int.Parse(splitmsg[4]);

                    parent.BroadcastMsg(Commands.Send(Command.Movement, receivedid + "|" + x + "|" + y));
                    break;
                case (int)Command.Connecting:
                    Alias = splitmsg[2];
                    string broadcastmsg = Alias + " connected";
                    Console.WriteLine(broadcastmsg);
                    parent.BroadcastMsg(broadcastmsg);

                    parent.BroadcastMsg(Commands.Send(Command.SendClient, id + "|" + Alias + "|" + x + "|" + y));
                    break;
            }
        }
    }
}