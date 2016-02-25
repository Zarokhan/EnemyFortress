﻿using EnemyFortress.Controllers;
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

        private GameScene gameScene;
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
            IsConnected = true;
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
                IsConnected = false;
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
        private void HandleServerCommand(string msg)
        {
            string[] splitMsg = msg.Split('|');
            int cmd = int.Parse(splitMsg[1]);
            switch (cmd)
            {
                case (int)Command.Spawn:                // Spawns client player
                    int x = int.Parse(splitMsg[2]);
                    int y = int.Parse(splitMsg[3]);

                    gameScene.SpawnClientTank(x, y);
                    break;
                case (int)Command.RemoveClient:     // Removes remote player from the game
                    int receivedID2 = int.Parse(splitMsg[2]);
                    if (receivedID2 == ID)
                        break;

                    // Removes remote player from game
                    for (int i = 0; i < gameScene.otherTanks.Count; i++)
                    {
                        RemoteControl control = (RemoteControl)gameScene.otherTanks[i].control;
                        if (receivedID2 == control.ID)
                            gameScene.otherTanks.RemoveAt(i);
                    }

                    break;
                case (int)Command.SendClient:   // Receives client (from servers point of view)
                    int receivedID = int.Parse(splitMsg[2]);
                    if (receivedID == ID)
                        break;

                    // Check if we already received client
                    for (int i = 0; i < gameScene.otherTanks.Count; i++)
                    {
                        RemoteControl control = (RemoteControl)gameScene.otherTanks[i].control;
                        if (receivedID == control.ID)
                            return;
                    }
                    gameScene.SpawnRemoteTank(receivedID, splitMsg[3], int.Parse(splitMsg[4]), int.Parse(splitMsg[5]));
                    break;
                case (int)Command.SendID:
                    this.ID = int.Parse(splitMsg[2]);
                    break;
                case (int)Command.Disconnecting:
                    IsConnected = false;
                    break;
            }
        }

        public void SetGameScene(GameScene gameScene = null)
        {
            this.gameScene = gameScene;
        }
    }
}