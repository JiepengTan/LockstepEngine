using System;

namespace Lockstep.Networking
{
    public delegate void PeerActionHandler(IPeer peer);

    public interface IServerSocket
    {
        /// <summary>
        /// Invoked, when a client connects to this socket
        /// </summary>
        event PeerActionHandler Connected;

        /// <summary>
        /// Invoked, when a client connects to this socket
        /// </summary>
        [Obsolete("Use 'Connected' event instead")]
        event PeerActionHandler OnConnected;

        /// <summary>
        /// Invoked, when client disconnects from this socket
        /// </summary>
        event PeerActionHandler Disconnected;

        /// <summary>
        /// Invoked, when client disconnects from this socket
        /// </summary>
        [Obsolete("Use 'Disconnected' event instead")]
        event PeerActionHandler OnDisconnected;

        /// <summary>
        /// Opens the socket and starts listening to a given port
        /// </summary>
        /// <param name="port"></param>
        void Listen(int port,string key = "");

        /// <summary>
        /// Stops listening
        /// </summary>
        void Stop();
    }
}