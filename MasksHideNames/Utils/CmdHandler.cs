using MasksHideNames.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintagestory.API.Common;
using Vintagestory.API.Common.CommandAbbr;
using Vintagestory.API.Server;

namespace MasksHideNames.Utils
{
    public static class CmdHandler
    {
        public static MasksHideNamesConfig Config;

        public static void RegisterCommands(ICoreServerAPI sapi, MasksHideNamesConfig config)
        {
            Config = config;


            // Register commands
            // mhn is main command
            sapi.ChatCommands.GetOrCreate("mhn")
                .WithAdditionalInformation("MasksHideNames Commands")

                // toggledisguise is subcommand to toggle the entire disguise system on or off (Requires admin privilege)
                .BeginSub("toggledisguise")
                    .RequiresPrivilege(Privilege.controlserver)
                    .WithDesc("Toggle disguise system on or off")
                    .HandleWith(HandleDisguiseToggle)
                .EndSub()

                // setname is subcommand to set your own name
                .BeginSub("setname")
                    .RequiresPrivilege(Privilege.chat)
                    .WithDesc("Set your own name")
                    .HandleWith(HandleSetName)
                .EndSub()

                // setplayername is subcommand to set another player's name (Requires admin privilege)
                .BeginSub("setplayername")
                    .RequiresPrivilege(Privilege.controlserver)
                    .WithDesc("Set another player's name")
                    .HandleWith(HandleSetPlayerName)
                .EndSub()

                // blacklistmask is subcommand to toggle a masks presence on the mask blacklist (Requires admin privilege) (If no argument given uses held item)
                .BeginSub("blacklistmask")
                    .RequiresPrivilege(Privilege.controlserver)
                    .WithDesc("Add/remove a mask to the mask blacklist")
                    .HandleWith(HandleRemoveMask)
                .EndSub()
                
                // listmasks is subcommand to list all masks in the mask blacklist (Requires admin privilege)
                .BeginSub("listmasks")
                    .RequiresPrivilege(Privilege.controlserver)
                    .WithDesc("List all masks in the mask blacklist")
                    .HandleWith(HandleListMasks)
                .EndSub()

                // whitelistdisguise is subcommand to toggle a disguises presence on the disguise list (Requires admin privilege) (If no argument given uses held item)
                .BeginSub("whitelistdisguise")
                    .RequiresPrivilege(Privilege.controlserver)
                    .WithDesc("Add/remove a disguise to the disguise list")
                    .HandleWith(HandleRemoveMask)
                .EndSub()

                // listdisguises is subcommand to list all disguises in the disguise list (Requires admin privilege)
                .BeginSub("listdisguises")
                    .RequiresPrivilege(Privilege.controlserver)
                    .WithDesc("List all disguises in the disguise list")
                    .HandleWith(HandleListMasks)
                .EndSub()
                ;
        }

        private static TextCommandResult HandleListMasks(TextCommandCallingArgs args)
        {
            string msg = "MasksHideNames: List of masks in blacklist:\n";
            foreach (string mask in Config.BlacklistedMasks)
            {
                msg += mask + "\n";
            }

            return TextCommandResult.Success(msg);
        }

        private static TextCommandResult HandleRemoveMask(TextCommandCallingArgs args)
        {
            throw new NotImplementedException();
        }

        private static TextCommandResult HandleSetPlayerName(TextCommandCallingArgs args)
        {
            throw new NotImplementedException();
        }

        private static TextCommandResult HandleSetName(TextCommandCallingArgs args)
        {
            SetName(args.Caller as IPlayer, args.LastArg as string);
            return TextCommandResult.Success("MasksHideNames: Name set to " + args.LastArg);
        }

        private static TextCommandResult HandleDisguiseToggle(TextCommandCallingArgs args)
        {
            throw new NotImplementedException();
        }

        private static void SetName(IPlayer player, string name)
        {
            DisguiseHelper.UpdatePlayerName(player, name);
        }
    }
}
