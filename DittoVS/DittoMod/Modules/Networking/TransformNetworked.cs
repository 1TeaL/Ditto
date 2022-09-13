using System;
using System.Collections.Generic;
using System.Text;
using RoR2;
using R2API.Networking.Interfaces;
using UnityEngine;
using UnityEngine.Networking;
using DittoMod.SkillStates;

namespace DittoMod.Modules.Networking
{
    public class TransformNetworked : INetMessage
    {
        NetworkInstanceId IDNet;

        public TransformNetworked()
        {

        }

        public TransformNetworked(NetworkInstanceId IDNet)
        {
            this.IDNet = IDNet;
        }

        public void Serialize(NetworkWriter writer)
        {
            writer.Write(IDNet);
        }
        public void Deserialize(NetworkReader reader)
        {
            IDNet = reader.ReadNetworkId();
        }

        public void OnReceived()
        {

            GameObject masterobject = Util.FindNetworkObject(IDNet);
            CharacterMaster charMaster = masterobject.GetComponent<CharacterMaster>();
            charMaster.TransformBody("DittoBody");
        }

    }
}
