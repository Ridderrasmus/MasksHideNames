namespace MasksHideNames.Config
{
    public class MasksHideNamesConfig
    {
        public bool DisguisesEnabled = true;
        public string[] BlacklistedMasks = new string[] { "game:clothes-face-forgotten", "othermodid:othermask" };
        
        public MasksHideNamesConfig() { }

        public MasksHideNamesConfig(MasksHideNamesConfig config)
        {
            DisguisesEnabled = config.DisguisesEnabled;
            BlacklistedMasks = config.BlacklistedMasks;
        }
    }
}