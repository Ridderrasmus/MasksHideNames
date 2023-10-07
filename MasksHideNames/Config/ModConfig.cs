using System;
using Vintagestory.API.Common;

namespace MasksHideNames.Config
{
    static class ModConfig
    {
        private const string ConfigFileName = "masks-hide-names.json";
        public static MasksHideNamesConfig Config { get; private set; }
        public static event Action ConfigUpdated;

        public static void ReadConfig(ICoreAPI api)
        {
            try
            {
                Config = LoadConfig(api);

                if (Config == null)
                {
                    GenerateConfig(api);
                    Config = LoadConfig(api);
                }
                else
                {
                    GenerateConfig(api, Config);
                }
            }
            catch
            {
                GenerateConfig(api);
                Config = LoadConfig(api);
            }
        }
        public static void Save(ICoreAPI api)
        {
            GenerateConfig(api, Config);
            ConfigUpdated?.Invoke();
        }

        private static MasksHideNamesConfig LoadConfig(ICoreAPI api) => api.LoadModConfig<MasksHideNamesConfig>(ConfigFileName);
        private static void GenerateConfig(ICoreAPI api) => api.StoreModConfig(new MasksHideNamesConfig(), ConfigFileName);
        private static void GenerateConfig(ICoreAPI api, MasksHideNamesConfig config) => api.StoreModConfig(new MasksHideNamesConfig(config), ConfigFileName);

        public static void Dispose()
        {
            Config = null;
        }
    }
}
