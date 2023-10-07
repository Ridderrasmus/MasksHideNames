using MasksHideNames.Config;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace MasksHideNames.Gui
{
    public class PlayerNameTagRenderer
    {
        private static ICoreClientAPI capi;
        private static MasksHideNamesConfig config;

        public PlayerNameTagRenderer(ICoreClientAPI capi, MasksHideNamesConfig config)
        {
            PlayerNameTagRenderer.capi = capi;
            PlayerNameTagRenderer.config = config;
        }

        public static LoadedTexture GetRenderer(EntityPlayer entity, double[] color = null, TextBackground bg = null)
        {
            capi.Logger.Debug("[MasksHideNames] GetRenderer called");

            color ??= ColorUtil.WhiteArgbDouble;
            var textColor = CairoFont.WhiteMediumText().WithColor(color);
            var textBg = bg?.Clone() ?? new TextBackground()
            {
                FillColor = GuiStyle.DialogLightBgColor,
                Padding = 3,
                Radius = GuiStyle.ElementBGRadius,
                Shade = true,
                BorderColor = GuiStyle.DialogBorderColor,
                BorderWidth = 3
            };


            string playerName = entity.GetBehavior<EntityBehaviorNameTag>()?.DisplayName;

            //If the player is wearing a mask, change name tag to "???"
            if (entity.GearInventory[8].Empty)
            {
                return capi.Gui.TextTexture.GenUnscaledTextTexture(playerName, textColor, textBg);
            }

            foreach (string mask in config.BlacklistedMasks)
            {
                if (entity.GearInventory[8].Itemstack.Collectible.Code.ToString() == mask)
                    return capi.Gui.TextTexture.GenUnscaledTextTexture(playerName, textColor, textBg);
            }

            return capi.Gui.TextTexture.GenUnscaledTextTexture("???", textColor, textBg);
        }
    }
}
