using MasksHideNames.Config;
using MasksHideNames.Networking;
using MasksHideNames.Patches;
using MasksHideNames.Utils;
using System;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.CommandAbbr;
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

            new DisguiseHelper(api, config);

            NetworkHandler.RegisterCommonNetworking(api, config);
        }

        public override void StartServerSide(ICoreServerAPI api)
        {
            sapi = api;

            CmdHandler.RegisterCommands(sapi, config);

            NetworkHandler.RegisterServerNetworking(sapi);

            sapi.Event.PlayerJoin += Event_PlayerJoin;

        }

        private void Event_PlayerJoin(IServerPlayer byPlayer)
        {
            string name = DisguiseHelper.GetPlayerName(byPlayer);
        }

        public override void StartClientSide(ICoreClientAPI api)
        {
            this.capi = api;

            

            NetworkHandler.RegisterClientNetworking(capi);
            
            patchHandler = new PatchHandler("maskshidenames");
            patchHandler.Patch();



            //Register an event for when a player entity spawns
            capi.Event.PlayerEntitySpawn += Event_PlayerSpawn;
        }

        private void Event_PlayerSpawn(IClientPlayer byPlayer)
        {
            if (byPlayer.Entity == null)
                return;

            NetworkHandler.RequestDisguiseInfo(byPlayer.PlayerUID);

            //Register an event for when the player's gear inventory changes that will call MaskCheck
            byPlayer.Entity.GearInventory.SlotModified += (slotint) => 
            {
                NetworkHandler.RequestDisguiseInfo(byPlayer.PlayerUID);
            };
        }
    }
}
