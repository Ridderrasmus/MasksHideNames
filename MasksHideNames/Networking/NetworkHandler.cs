using MasksHideNames.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Server;

namespace MasksHideNames.Networking
{
    public static class NetworkHandler
    {
        public static MasksHideNamesConfig Config;
        public static ICoreAPI API;
        public static ICoreClientAPI CAPI;
        public static ICoreServerAPI SAPI;


        public static string ChannelName => "maskshidenames";

        internal static void RegisterCommonNetworking(ICoreAPI api, MasksHideNamesConfig config)
        {
            API = api;
            Config = config;

            // Register network channel
            API.Network.RegisterChannel(ChannelName)
                .RegisterMessageType<PacketDisguise>();
        }

        #region Server Networking

        internal static void RegisterServerNetworking(ICoreServerAPI sapi)
        {
            SAPI = sapi;

            SAPI.Network.GetChannel(ChannelName)
                .SetMessageHandler<PacketDisguise>(OnPacketDisguise_Server);
        }

        // Recieve disguise info request
        private static void OnPacketDisguise_Server(IServerPlayer fromPlayer, PacketDisguise packet)
        {
            // Open packet and get player UID
            var playerUID = packet.PlayerUID;

            // Get player name from database using UID
            Config.PlayerNames.TryGetValue(playerUID, out string playerName);

            // Check if player is disguised
            bool isDisguised = DisguiseHelper.DisguiseCheck(fromPlayer.Entity);

            // Send packet back to client
            ReplyDisguisePacket(fromPlayer, new PacketDisguise(playerUID, isDisguised, playerName));
        }

        private static void ReplyDisguisePacket(IServerPlayer toPlayer, PacketDisguise packetDisguise)
        {
            SAPI.Network.GetChannel(ChannelName)
                .SendPacket(packetDisguise, toPlayer);
        }

        #endregion

        #region Client Networking

        internal static void RegisterClientNetworking(ICoreClientAPI capi)
        {
            CAPI = capi;
            CAPI.Network.GetChannel(ChannelName).SetMessageHandler<PacketDisguise>(OnPacketDisguise_Client);
        }

        private static void OnPacketDisguise_Client(PacketDisguise packet)
        {
            var player = CAPI.World.PlayerByUid(packet.PlayerUID);

            var name = packet.DisguiseName;

            if (packet.IsDisguised)
                name = "???";

            player.Entity.WatchedAttributes.GetTreeAttribute("nametag").SetString("name", name);
            player.Entity.WatchedAttributes.MarkPathDirty("nametag");
        }

        internal static void RequestDisguiseInfo(string playerUID)
        {
            CAPI.Network.GetChannel(ChannelName)
                .SendPacket(new PacketDisguise(playerUID, false, ""));
        }

        #endregion
    }
}
