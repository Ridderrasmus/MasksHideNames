using System.Collections.Generic;
using System.ComponentModel;

namespace MasksHideNames.Config
{
    public class MasksHideNamesConfig
    {
        public bool DisguisesEnabled = true;
        public string[] BlacklistedMasks = new string[] { "game:clothes-face-forgotten", "othermodid:othermask" };
        public string[] Disguises = new string[] { "game:some-sort-of-armor" };
        public Dictionary<string, string> PlayerNames = new Dictionary<string, string>();
        
        public MasksHideNamesConfig() { }

        public MasksHideNamesConfig(MasksHideNamesConfig config)
        {
            DisguisesEnabled = config.DisguisesEnabled;
            BlacklistedMasks = config.BlacklistedMasks;
            Disguises = config.Disguises;
            PlayerNames = config.PlayerNames;
        }

    }
}