using System;
using System.Collections.Generic;
using System.Linq;
using LiteNetLib;
using Lockstep.Logging;
using Lockstep.Networking;
using Lockstep.Serialization;

namespace Lockstep.Networking {
    public class NetServer<TMsgType> : IPollEvents,IDoDestroy
        where TMsgType : struct {
        private readonly ServerSocketLn _server;

        private string _clientKey;

        //所有的消息处理函数
        protected IncommingMessageHandler[] _allDealFuncs;
        private int maxMsgIdx;
        public event PeerActionHandler OnDisconnected;


        public NetServer(string clientKey, int maxMsgIdx, string[] msgFlags, object msgHandlerObj){
            this._clientKey = clientKey;
            this.maxMsgIdx = maxMsgIdx;
            _allDealFuncs = new IncommingMessageHandler[maxMsgIdx];
            foreach (var msgFlag in msgFlags) {
                NetworkUtil.RegisterEvent<TMsgType, IncommingMessageHandler>("" + msgFlag, "".Length,
                    RegisterMsgHandler,
                    msgHandlerObj);
            }

            _server = new ServerSocketLn();
            _server.MessageReceived += OnMessage;
            _server.OnDisconnected += (pe) => {
                OnDisconnected?.Invoke(pe);
            };
        }

        public void RegisterMsgHandler(TMsgType msgType, IncommingMessageHandler handler){
            var idx = (short) (object) msgType;
            _allDealFuncs[idx] = handler;
        }

        public void BorderMessage(TMsgType type, BaseFormater msg){
            _server.BorderMessage((short) (object) type, msg);
        }

        public void Listen(int port){
            _server.Listen(port, _clientKey);
        }

        public void DoDestroy(){
            _server.Stop();
        }

        private void OnMessage(IIncommingMessage msg){
            var msgType = msg.OpCode;
            if (msgType >= maxMsgIdx) {
                Debug.LogError("msgType out of range " + msgType);
                return;
            }

            try {
                var _func = _allDealFuncs[msgType];
                if (_func != null) {
                    _func(msg);
                }
                else {
                    Debug.LogError("ErrorMsg type :no msg handler " + (TMsgType) (object) msgType);
                }
            }
            catch (Exception e) {
                Debug.Log($"netID{msg.Peer.Id} msgType {(TMsgType) (object) msgType} deal msg Error:{e.ToString()}");
            }
        }

        public void PollEvents(){
            _server.PollEvents();
        }
    }
}