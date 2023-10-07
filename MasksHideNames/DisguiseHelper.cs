using MasksHideNames.Config;
using System.Linq;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace MasksHideNames
{
    public class DisguiseHelper
    {
        private static ICoreClientAPI capi;
        private static MasksHideNamesConfig config;

        public DisguiseHelper(ICoreClientAPI capi, MasksHideNamesConfig config)
        {
            DisguiseHelper.capi = capi;
            DisguiseHelper.config = config;
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
                    capi.Logger.Debug("Mask found!");
                    return true;
                }
                else
                {
                    if (config.Disguises.Contains(slot.Itemstack.Item.Code.ToString()))
                    {
                        capi.Logger.Debug("Disguise found!");
                        return true;
                    }
                }
            }

            capi.Logger.Debug("No disguise found!");
            return false;
        }
    }
}
