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
    public class LeftoversNetworked : INetMessage
    {
        NetworkInstanceId IDNet;

        public LeftoversNetworked()
        {

        }

        public LeftoversNetworked(NetworkInstanceId IDNet)
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
            if (NetworkServer.active)
            {
                GameObject masterobject = Util.FindNetworkObject(IDNet);
                CharacterMaster charMaster = masterobject.GetComponent<CharacterMaster>();
                CharacterBody charBody = charMaster.GetComponent<CharacterBody>();

                charBody.healthComponent.Heal(Modules.StaticValues.leftoversregen * charBody.healthComponent.fullHealth, new ProcChainMask(), true);

            }
        }

    }
}
