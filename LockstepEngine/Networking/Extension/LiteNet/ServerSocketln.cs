using System;
using System.Collections.Generic;
using System.Linq;
using LiteNetLib;
using Lockstep.Networking;
using Lockstep.Serialization;
using Lockstep.Util;

namespace Lockstep.Networking {
    /// <summary>
    /// Server socket, which accepts websocket connections
    /// </summary>
    public class ServerSocketLn : IServerSocket {
        public event PeerActionHandler Connected;
        public event PeerActionHandler Disconnected;
        public event Action<IIncommingMessage> MessageReceived;

        public event PeerActionHandler OnConnected {
            add => Connected += value;
            remove => Connected -= value;
        }

        public event PeerActionHandler OnDisconnected {
            add => Disconnected += value;
            remove => Disconnected -= value;
        }

        private NetManager _server;
        private Dictionary<int, PeerLn> _id2Peer = new Dictionary<int, PeerLn>();
        private PeerLn[] _allPeers;

        public ServerSocketLn(){ }

        /// Opens the socket and starts listening to a given port
        public void Listen(int port, string key = ""){
            var _listener = new EventBasedNetListener();
            _server = new NetManager(_listener) {
                DisconnectTimeout = 30000,
            };
            _listener.ConnectionRequestEvent += request => { request.AcceptIfKey(key); };

            _listener.PeerConnectedEvent += pe => {
                var speer = new PeerLn(pe);
                _id2Peer[pe.Id] = speer;
                _allPeers = null;
                speer.SetConnectedState(true);
                speer.MessageReceived += OnMessage;
                Connected?.Invoke(speer);
            };

            _listener.NetworkReceiveEvent += (pe, reader, method) => {
                if (_id2Peer.TryGetValue(pe.Id, out var peer)) {
                    peer.HandleDataReceived(reader.GetRemainingBytes(), 0);
                }
            };

            _listener.PeerDisconnectedEvent += (pe, info) => {
                _allPeers = null;
                if (_id2Peer.TryGetValue(pe.Id,out var peer)) {
                    peer.SetConnectedState(false);
                    peer.MessageReceived -= OnMessage;
                    Disconnected?.Invoke(peer);
                    peer.NotifyDisconnectEvent();
                    _id2Peer.Remove(pe.Id);
                }
            };

            _server.Start(port);
        }

        public void BorderMessage(short type, ISerializablePacket data){
            if (_allPeers == null) {
                _allPeers = _id2Peer.Values.ToArray();
            }

            var peers = _allPeers;
            foreach (var peer in peers) {
                peer.SendMessage(type, data);
            }
        }

        void OnMessage(IIncommingMessage msg){
            MessageReceived?.Invoke(msg);
        }

        /// <summary>
        /// Stops listening
        /// </summary>
        public void Stop(){
            _server.Stop();
        }

        public void Update(){ }

        public void PollEvents(){
            _server?.PollEvents();
        }
    }
}