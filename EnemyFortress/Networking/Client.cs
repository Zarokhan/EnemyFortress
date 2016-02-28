using EnemyFortress.Controllers;
using EnemyFortress.Forms;
using EnemyFortress.Scenes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;
using Utilities;

namespace EnemyFortress.Networking
{
    /// <summary>
    /// Client handles communication with server.
    /// </summary>
    class Client
    {
        public int ID { get; private set; }
        public string IP { get; private set; }
        public int Port { get; private set; }
        public string Alias { get; private set; }
        public int Latency { get; private set; }
        public int Ping { get; private set; }

        public GameScene GameScene { get; set; }
        private NetworkStream stream;                   // Network stream used for reading / writing data to / from server.
        private BinaryReader reader;                       // Reads data sent from server.
        private BinaryWriter writer;                         // Writes data to server.
        private TcpClient client;                               // Making connection with server.

        public volatile bool IsConnected;                  // Indication if connection with server is made.
        private object myLock;                                // Used for synchronization.
        private int nowMillis;                  // For calculating ping and latency
        private bool tryingToConnect;           // True if we are trying to connect to server

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="form">For access to important window features.</param>
        public Client(ConnectForm form)
        {
            IP = form.IP;
            Port = form.Port;
            Alias = form.Alias;
            myLock = new object();
            IsConnected = true;
            tryingToConnect = true;
        }

        /// <summary>
        /// Handles connection, constantly looking for data to read from server.
        /// </summary>
        public void HoldConnection()
        {
            try
            {
                Console.WriteLine("Connecting...");

                client = new TcpClient();
                client.Connect(IP, Port);
                stream = client.GetStream();
                reader = new BinaryReader(stream, Encoding.ASCII);
                writer = new BinaryWriter(stream, Encoding.ASCII);

                WriteMessage(Commands.Send(Command.Connecting, Alias));
                IsConnected = true;
                tryingToConnect = false;
            }
            catch (Exception ex)
            {
                IsConnected = false;
                client.Close();
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            while (IsConnected && client.Connected)
            {
                //lock (myLock) // Behöver låset vara här?
                //{
                    try
                    {
                        if (!reader.BaseStream.CanRead)
                            continue;

                        string msg = reader.ReadString();
                        if (msg.Contains(Commands.KEY))
                            HandleServerCommand(msg);
                        else
                            Console.WriteLine(msg);
                    }
                    catch
                    {
                        IsConnected = false;
                    }
                //}
            }

            Disconnect();

            Console.WriteLine("Disconnected from server");
        }

        /// <summary>
        /// Disconnect from server.
        /// </summary>
        public void Disconnect()
        {
            if (tryingToConnect)
            {
                IsConnected = false;
                client.Close();
            }
            else if (client.Connected)
            {
                IsConnected = false;
                client.Close();
                reader.Close();
                writer.Close();
            }
        }

        /// <summary>
        /// Writes a message to server.
        /// </summary>
        /// <param name="message">Message to write to server.</param>
        public void WriteMessage(string message)
        {
            if (!client.Connected || !stream.CanWrite || !stream.CanRead)
                return;

            writer.Write(message);
            writer.Flush();
        }

        /// <summary>
        /// Handles a command from server correctly.
        /// </summary>
        /// <param name="msg">Message from server to handle.</param>
        private void HandleServerCommand(string msg)
        {
            string[] splitMsg = msg.Split('|');
            int cmd = int.Parse(splitMsg[1]);
            switch (cmd)
            {
                case (int)Command.Ping: // KEY|COMMAND|ID|Ping
                    if (int.Parse(splitMsg[2]) == ID)
                        return;

                    for (int i = 0; i < GameScene.remoteTanks.Count; i++)
                    {
                        RemoteControl control = (RemoteControl)GameScene.remoteTanks[i].control;
                        if (control.ID == int.Parse(splitMsg[2]))
                            control.Ping = int.Parse(splitMsg[3]);
                    }
                    break;
                case (int)Command.Latency: // KEY|COMMAND|ID|LATENCY
                    int lat = int.Parse(splitMsg[3]);

                    // Ourself
                    if (int.Parse(splitMsg[2]) == ID)
                    {
                        Latency = lat;
                        Ping = DateTime.Now.Millisecond - nowMillis;
                        WriteMessage(Commands.Send(Command.Ping, ID + "|" + Ping)); // KEY|COMMAND|ID|Ping
                    }
                    // Other
                    else
                    {
                        for (int i = 0; i < GameScene.remoteTanks.Count; i++)
                        {
                            RemoteControl control = (RemoteControl)GameScene.remoteTanks[i].control;
                            if (control.ID == int.Parse(splitMsg[2]))
                                control.Latency = lat;
                        }
                    }
                    break;
                case (int)Command.Movement: // KEY|COMMAND|ID|X|Y|ROTATION(RADS)
                    int receivedID3 = int.Parse(splitMsg[2]);
                    if (receivedID3 == ID)
                        return;

                    for(int i = 0; i < GameScene.remoteTanks.Count; i++)
                    {
                        RemoteControl control = (RemoteControl)GameScene.remoteTanks[i].control;
                        if (control.ID == receivedID3)
                            control.UpdateMovement(float.Parse(splitMsg[3]), float.Parse(splitMsg[4]), float.Parse(splitMsg[5]));
                    }
                    break;
                case (int)Command.Spawn:                // Spawns client player
                    int x = int.Parse(splitMsg[2]);
                    int y = int.Parse(splitMsg[3]);

                    GameScene.SpawnClientTank(x, y);
                    break;
                case (int)Command.RemoveClient:     // Removes remote player from the game
                    int receivedID2 = int.Parse(splitMsg[2]);
                    if (receivedID2 == ID)
                        break;

                    // Removes remote player from game
                    for (int i = 0; i < GameScene.remoteTanks.Count; i++)
                    {
                        RemoteControl control = (RemoteControl)GameScene.remoteTanks[i].control;
                        if (receivedID2 == control.ID)
                            GameScene.remoteTanks.RemoveAt(i);
                    }

                    break;
                case (int)Command.SendClient:   // Receives client (from servers point of view)
                    int receivedID = int.Parse(splitMsg[2]);
                    if (receivedID == ID)
                        break;

                    // Check if we already received client
                    for (int i = 0; i < GameScene.remoteTanks.Count; i++)
                    {
                        RemoteControl control = (RemoteControl)GameScene.remoteTanks[i].control;
                        if (receivedID == control.ID)
                            return;
                    }
                    GameScene.SpawnRemoteTank(receivedID, splitMsg[3], int.Parse(splitMsg[4]), int.Parse(splitMsg[5]));
                    break;
                case (int)Command.SendID:
                    this.ID = int.Parse(splitMsg[2]);
                    break;
                case (int)Command.Disconnecting:
                    IsConnected = false;
                    break;
            }
        }

        /// <summary>
        /// Updates latency and ping
        /// </summary>
        public void UpdateLatency()
        {
            nowMillis = DateTime.Now.Millisecond;
            WriteMessage(Commands.Send(Command.Latency, ID + "|" + nowMillis)); // KEY|COMMAND|ID|MILLISECONDS
        }
    }
}