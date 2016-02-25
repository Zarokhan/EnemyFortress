namespace Utilities
{
    /// <summary>
    /// Used by client and server to send commands betweend eachother
    /// </summary>
    public enum Command
    {
        Connecting,
        Disconnecting,
        SendID,         // Server to client, send ID to client
        SendClient,    // Server to client, sends a client to another client
        RemoveClient,
        Spawn
    }

    public enum Moving
    {
        Up,
        Down,
        Left,
        Right
    }

    /// <summary>
    /// Container of strings, enabling commands to be sent.
    /// </summary>
    public static class Commands
    {
        /// <summary>
        /// Super private key.
        /// </summary>
        public const string KEY = "[RASB_SERVER::COMMAND]";     // General commands key

        /// <summary>
        /// Returns a string with key + parameters.
        /// </summary>
        /// <param name="cmd">Which command you which to send.</param>
        public static string Send(Command cmd)
        {
            return KEY + "|" + (int)cmd;
        }

        /// <summary>
        /// Returns a string with key + parameters.
        /// </summary>
        /// <param name="cmd">Which command you which to send.</param>
        /// <param name="msg">Message to send.</param>
        public static string Send(Command cmd, string msg)
        {
            return KEY + "|" + (int)cmd + "|" + msg;
        }

        /// <summary>
        /// Returns a string with key + parameters.
        /// </summary>
        /// <param name="cmd">Which command you which to send.</param>
        /// <param name="msg">Message to send.</param>
        public static string Send(Command cmd, int msg)
        {
            return KEY + "|" + (int)cmd + "|" + msg;
        }

        /// <summary>
        /// Returns a string with key + parameters.
        /// Used for group invitations.
        /// </summary>
        /// <param name="cmd">Which command you which to send.</param>
        /// <param name="msg">Message to send.</param>
        public static string Send(Command cmd, int id, string[] msg)
        {
            string str = "";
            for (int i = 0; i < msg.Length; i++)
            {
                if (str == "")
                    str = msg[i];
                else
                    str += "," + msg[i];
            }
            return KEY + "|" + (int)cmd + "|" + id + "|" + str;
        }

        /// <summary>
        /// Returns a string with key + parameters.
        /// Used for group invitations.
        /// </summary>
        /// <param name="cmd">Which command you which to send.</param>
        /// <param name="msg">Message to send.</param>
        public static string Send(Command cmd, int id, string msg)
        {
            return KEY + "|" + (int)cmd + "|" + id + "|" + msg;
        }

        /// <summary>
        /// Returns a string with key + parameters.
        /// Used for group invitations.
        /// </summary>
        /// <param name="cmd">Which command you which to send.</param>
        /// <param name="sender">Who sent the message.</param>
        /// <param name="msg">Message to send.</param>
        public static string Send(Command cmd, int id, string sender, string msg)
        {
            return KEY + "|" + (int)cmd + "|" + id + "|" + sender + "|" + msg;
        }
    }
}