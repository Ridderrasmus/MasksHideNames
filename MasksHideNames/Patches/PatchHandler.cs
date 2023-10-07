using HarmonyLib;
using System;
using Vintagestory.GameContent;

namespace MasksHideNames.Patches
{
    class PatchHandler : IDisposable
    {
        private Harmony harmony;

        public PatchHandler(string harmonyId)
        {
            harmony = new Harmony(harmonyId);
        }

        public void Patch()
        {
            // Create harmony patches here
            EntityNameTagRendererRegistryPatch.Patch(harmony);
        }

        public void Unpatch()
        {
            harmony.UnpatchAll(harmony.Id);
        }

        public void Dispose()
        {
            Unpatch();
        }
    }
}
