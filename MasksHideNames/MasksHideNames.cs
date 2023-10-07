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

            patchHandler = new PatchHandler("maskshidenames");

            new Gui.PlayerNameTagRenderer(api, config);


            //Register an event for when a player entity spawns
            //capi.Event.PlayerEntitySpawn += Event_PlayerSpawn;
        }

        private void Event_PlayerSpawn(IClientPlayer byPlayer)
        {
            //Set the player's real name as a saved attribute
            byPlayer.Entity.WatchedAttributes.SetString("maskshidenames:realName", byPlayer.PlayerName);

            //Do a check to see if the player is wearing a mask and handle accordingly
            MaskCheck(byPlayer);

            //Register an event for when the player's gear inventory changes that will call MaskCheck
            byPlayer.Entity.GearInventory.SlotModified += (slotint) => { MaskCheck(byPlayer); };
        }

        /// <summary>
        /// Checks if the player is wearing a mask and hides player name accordingly
        /// </summary>
        /// <param name="player"></param>
        public void MaskCheck(IClientPlayer player)
        {

            var slot = capi.World.Player.Entity.GearInventory[8];

            bool shouldHide = false;
            shouldHide = !slot.Empty;

            if (shouldHide)
            {
                foreach (string mask in config.BlacklistedMasks)
                {
                    if (slot.Itemstack.Collectible.Code.ToString() == mask)
                    {
                        shouldHide = false;
                    }
                }
            }

            SetHideName(player, shouldHide);
            
            string name = capi.World.Player.Entity.WatchedAttributes.GetBool("maskshidenames:hideName") ? "???" : capi.World.Player.Entity.WatchedAttributes.GetString("maskshidenames:realName");
            
            SetName(player, name);
        }

        /// <summary>
        /// Changes the player's hideName attribute
        /// </summary>
        /// <param name="player"></param>
        /// <param name="hideName"></param>
        public void SetHideName(IClientPlayer player, bool hideName)
        {
            capi.Logger.Debug("Setting hideName to: " + hideName);
            player.Entity.WatchedAttributes.SetBool("maskshidenames:hideName", hideName);
        }

        /// <summary>
        /// Changes the player's name (and marks the attribute path as dirty)
        /// </summary>
        /// <param name="player"></param>
        /// <param name="name"></param>
        public void SetName(IClientPlayer player, string name)
        {
            capi.Logger.Debug("Setting name to: " + name);
            player.Entity.WatchedAttributes.GetTreeAttribute("nametag")?.SetString("name", name);
            player.Entity.WatchedAttributes.MarkPathDirty("nametag");
        }

        public override void Dispose()
        {
            //capi.Event.PlayerEntitySpawn -= Event_PlayerSpawn;
        }
    }
}
