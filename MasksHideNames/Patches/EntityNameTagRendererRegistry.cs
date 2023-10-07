using HarmonyLib;
using System.Reflection;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.GameContent;
using static Vintagestory.GameContent.EntityNameTagRendererRegistry;

namespace MasksHideNames.Patches
{
    static class EntityNameTagRendererRegistryPatch
    {
        public static void Patch(Harmony harmony)
        {
            var OriginalField1 = AccessTools.Field(typeof(EntityNameTagRendererRegistry), nameof(EntityNameTagRendererRegistry.DefaultNameTagRenderer));
            var OriginalMethod1 = RuntimeReflectionExtensions.GetMethodInfo((NameTagRendererDelegate)OriginalField1.GetValue(null));
            var PrefixMethod1 = AccessTools.Method(typeof(EntityNameTagRendererRegistryPatch), nameof(DefaultNameTagRenderer));
            harmony.Patch(OriginalMethod1, prefix: new HarmonyMethod(PrefixMethod1));

            var OriginalMethod2 = AccessTools.Method(typeof(DefaultEntitlementTagRenderer), nameof(DefaultEntitlementTagRenderer.renderTag));
            var PrefixMethod2 = AccessTools.Method(typeof(EntityNameTagRendererRegistryPatch), nameof(DefaultEntitlementTagRenderer_renderTag));
            harmony.Patch(OriginalMethod2, prefix: new HarmonyMethod(PrefixMethod2));
        }

        private static void DefaultEntitlementTagRenderer_renderTag(ref LoadedTexture __result, double[] ___color, TextBackground ___background, Entity entity)
        {
            if (entity is not EntityPlayer playerEntity) return;

            var name = playerEntity.WatchedAttributes.GetString("maskshidenames:realName", "ERROR");

            if (DisguiseHelper.DisguiseCheck(playerEntity))
                name = "???";

            entity.WatchedAttributes.GetTreeAttribute("nametag").SetString("name", name);
        }

        private static void DefaultNameTagRenderer(ref LoadedTexture __result, Entity entity)
        {
            if (entity is not EntityPlayer playerEntity) return;
            
            var name = playerEntity.WatchedAttributes.GetString("maskshidenames:realName", "ERROR");
            
            if (DisguiseHelper.DisguiseCheck(playerEntity))
                name = "???";

            entity.WatchedAttributes.GetTreeAttribute("nametag").SetString("name", name);
        }

        
    }
}