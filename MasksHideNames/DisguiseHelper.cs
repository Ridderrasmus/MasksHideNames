using MasksHideNames.Config;
using MasksHideNames.Networking;
using System.Linq;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace MasksHideNames
{
    public class DisguiseHelper
    {
        private static ICoreAPI api;
        private static MasksHideNamesConfig config;

        public DisguiseHelper(ICoreAPI api, MasksHideNamesConfig config)
        {
            DisguiseHelper.api = api;
            DisguiseHelper.config = config;
        }

        public static void UpdatePlayerName(IPlayer player, string newPlayerName)
        {
            if (player == null)
                return;

            if (!config.PlayerNames.ContainsKey(player.PlayerUID))
                config.PlayerNames.Add(player.PlayerUID, newPlayerName);
            else
                config.PlayerNames[player.PlayerUID] = newPlayerName;

            player.Entity.WatchedAttributes.MarkPathDirty("nametag");
            ModConfig.Save(api);
        }

        public static string GetPlayerName(IPlayer player)
        {
            if (player == null)
                return null;

            if (!config.PlayerNames.ContainsKey(player.PlayerUID))
            {
                config.PlayerNames.Add(player.PlayerUID, player.PlayerName);
                ModConfig.Save(api);
            }
            
            return config.PlayerNames[player.PlayerUID];
        }

        public static bool DisguiseCheck(EntityPlayer player)
        {
            //return false;

            if (player == null)
                return false;

            foreach (ItemSlot slot in player.GearInventory)
            {
                if (slot.Empty)
                    continue;

                if (slot == player.GearInventory[8] && !config.BlacklistedMasks.Contains(slot.Itemstack.Item.Code.ToString()))
                {
                    api.Logger.Debug("Mask found!");
                    return true;
                }
                
                if (config.Disguises.Contains(slot.Itemstack.Item.Code.ToString()))
                {
                    api.Logger.Debug("Disguise found!");
                    return true;
                }
                
            }

            api.Logger.Debug("No disguise found!");
            return false;
        }
    }
}
