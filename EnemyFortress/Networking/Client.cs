using EnemyFortress.Forms;
using EnemyFortress.Scenes;
using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;
using Utilities;

namespace EnemyFortress.Networking
{
    /// <summary>
    /// Simon Berggren - TGSPA14h
    /// 
    /// Client handles communication with server.
    /// </summary>
    class Client
    {
        public string IP { get; private set; }
        public int Port { get; private set; }
        public string Alias { get; private set; }

        private NetworkStream stream;                   // Network stream used for reading / writing data to / from server.
        private BinaryReader reader;                       // Reads data sent from server.
        private BinaryWriter writer;                         // Writes data to server.
        private TcpClient client;                               // Making connection with server.

        public volatile bool IsConnected;                  // Indication if connection with server is made.
        private object myLock;                                // Used for synchronization.

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
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            while (IsConnected && client.Connected)
            {
                lock (myLock)
                {
                    try
                    {
                        string msg = reader.ReadString();
                        if (msg.Contains(Commands.KEY))
                            HandleCommand(msg);
                        else if (msg.Contains(Commands.GAME_KEY))
                            HandleGameCommand(msg);
                        else
                            Console.WriteLine(msg);
                    }
                    catch
                    {
                        IsConnected = false;
                    }
                }
            }

            if (client.Connected)
                Disconnect();

            Console.WriteLine("Disconnected from server");
        }

        /// <summary>
        /// Disconnect from server.
        /// </summary>
        public void Disconnect()
        {
            if (client.Connected)
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
        private void HandleCommand(string msg)
        {
            string[] splitMsg = msg.Split('|');
            int cmd = int.Parse(splitMsg[1]);
            switch (cmd)
            {
                case (int)Command.Disconnecting:
                    IsConnected = false;
                    break;
            }
        }

        private void HandleGameCommand(string msg)
        {
            string[] splitMsg = msg.Split('|');
            int cmd = int.Parse(splitMsg[1]);
            switch (cmd)
            {
                case (int)GameCommand.Position:
                    break;
            }
        }
    }
}