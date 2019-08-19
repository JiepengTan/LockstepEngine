using System.Collections.Generic;
using Lockstep.Logging;

namespace Lockstep.Networking {
    public class NetworkEntity : NetworkProxy {
        protected List<NetworkProxy> Comps = new List<NetworkProxy>();

        public T AddComponent<T>() where T : NetworkProxy, new(){
            var comp = new T();
            comp.SetLogger(Debug);
            Comps.Add(comp);
            return comp;
        }

        public override void DoAwake(){
            HasInit = true;
            foreach (var comp in Comps) {
                comp.DoAwake();
            }
        }

        public override void DoStart(){
            foreach (var comp in Comps) {
                comp.DoStart();
            }
        }

        public override void DoDestroy(){
            foreach (var comp in Comps) {
                comp.DoDestroy();
            }
        }

        public override void DoUpdate(int deltaTimeMs){
            foreach (var comp in Comps) {
                comp.DoUpdate(deltaTimeMs);
            }
        }

        public override void PollEvents(){
            foreach (var comp in Comps) {
                comp.PollEvents();
            }
        }
    }
}