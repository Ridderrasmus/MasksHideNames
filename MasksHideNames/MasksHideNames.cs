using MasksHideNames.Config;
using MasksHideNames.Patches;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Server;

namespace MasksHideNames
{
    public class MasksHideNames : ModSystem
    {
        private MasksHideNamesConfig config;

        private PatchHandler patchHandler;

        private ICoreClientAPI capi;
        private ICoreServerAPI sapi;
        
        public override void StartPre(ICoreAPI api)
        {
            ModConfig.ReadConfig(api);
            config = ModConfig.Config;
        }

        public override void StartClientSide(ICoreClientAPI api)
        {
            this.capi = api;

            new DisguiseHelper(api, config);
            
            patchHandler = new PatchHandler("maskshidenames");
            patchHandler.Patch();



            //Register an event for when a player entity spawns
            capi.Event.PlayerEntitySpawn += Event_PlayerSpawn;
        }

        private void Event_PlayerSpawn(IClientPlayer byPlayer)
        {
            byPlayer.Entity.WatchedAttributes.SetString("maskshidenames:realName", byPlayer.PlayerName);

            //Do a check to see if the player is wearing a mask and handle accordingly
            if(DisguiseHelper.DisguiseCheck(byPlayer.Entity))
                byPlayer.Entity.WatchedAttributes.MarkPathDirty("nametag");

            //Register an event for when the player's gear inventory changes that will call MaskCheck
            byPlayer.Entity.GearInventory.SlotModified += (slotint) => 
            {
                if (DisguiseHelper.DisguiseCheck(byPlayer.Entity))
                    byPlayer.Entity.WatchedAttributes.MarkPathDirty("nametag");
            };
        }

        public override void Dispose()
        {
            capi.Event.PlayerEntitySpawn -= Event_PlayerSpawn;
        }
    }
}
