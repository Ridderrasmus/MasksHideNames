using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasksHideNames.Networking
{
    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class PacketDisguise
    {
        public string PlayerUID;
        public bool IsDisguised;
        public string DisguiseName;

        public PacketDisguise() 
        {
            PlayerUID = "";
            IsDisguised = false;
            DisguiseName = "";
        }

        public PacketDisguise(string playerUID, bool isDisguised, string disguiseName) 
        {
            PlayerUID = playerUID;
            IsDisguised = isDisguised;
            DisguiseName = disguiseName;
        }
    }
}
