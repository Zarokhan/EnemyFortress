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
        public string Username { get; private set; }               // Client's username on server.
        public int id { get; private set; }                                // Client's unique id.

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

            writer.Write("Connected to server");
            writer.Flush();

            while (Connected)
            {
                if (reader.BaseStream.CanRead)
                {
                    try
                    {
                        string msg = reader.ReadString();

                        if (msg.Contains(Commands.KEY))
                            HandleConnectionCommand(msg);
                        else if (msg.Contains(Commands.GAME_KEY))
                            HandleGameCommand(msg);
                        else
                        {
                            msg = Username + ": " + msg;
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
        /// Handles a command from server correctly.
        /// </summary>
        /// <param name="msg">Message from server to handle.</param>
        private void HandleConnectionCommand(string msg)
        {
            string[] splitmsg = msg.Split('|');
            int command = int.Parse(splitmsg[1]);

            switch (command)
            {
                case (int)Command.Connecting:
                    Username = splitmsg[2];
                    string broadcastmsg = Username + " connected";
                    Console.WriteLine(broadcastmsg);
                    parent.BroadcastMsg(broadcastmsg);
                    break;
            }
        }

        private void HandleGameCommand(string msg)
        {
            string[] splitmsg = msg.Split('|');
            int command = int.Parse(splitmsg[1]);

            switch (command)
            {
                case (int)GameCommand.Position:
                    string[] strpositon = splitmsg[2].Split(',');

                    string broadcastmsg = Username + " connected";

                    Console.WriteLine(broadcastmsg);
                    parent.BroadcastMsg(broadcastmsg);
                    break;
            }
        }
    }
}