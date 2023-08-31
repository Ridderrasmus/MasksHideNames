using System;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Server;
using Vintagestory.Common;
using Vintagestory.GameContent;

namespace MasksHideNames
{
    public class MasksHideNamesModSystem : ModSystem
    {
        private ICoreClientAPI capi;
        private ICoreServerAPI sapi;
        // Called on server and client
        // Useful for registering block/entity classes on both sides
        public override void Start(ICoreAPI api)
        {
            api.Logger.Notification("Hello from template mod: " + api.Side);
        }

        public override void StartServerSide(ICoreServerAPI api)
        {
            this.sapi = api;
            api.Logger.Notification("Hello from template mod server side: " + Lang.Get("MasksHideNames:hello"));
        }

        public override void StartClientSide(ICoreClientAPI api)
        {
            this.capi = api;
            
            //Register an event that fires when an inventory slot is modified to check if it was a mask and then do stuff


            api.Logger.Notification("Hello from template mod client side: " + Lang.Get("MasksHideNames:hello"));
        }
    }
}
