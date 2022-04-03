using RoR2;
using System.Collections.Generic;
using UnityEngine;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using UnityEngine.AddressableAssets;

namespace DittoMod.Modules
{
    public static class Buffs
    {
        internal static List<BuffDef> buffDefs = new List<BuffDef>();

        internal static BuffDef ofaBuff;

        internal static BuffDef kickBuff;

        internal static BuffDef floatBuff;

        internal static BuffDef supaofaBuff;

        internal static BuffDef ofaBuff45;

        internal static BuffDef supaofaBuff45;

        internal static BuffDef fajinBuff;

        //armor buff for oklahoma
        internal static BuffDef oklahomaBuff;


        internal static BuffDef counterBuff;

        internal static void RegisterBuffs()
        {
            oklahomaBuff = Buffs.AddNewBuff("DekuArmorBuff", Assets.mainAssetBundle.LoadAsset<Sprite>("armorblue"), Color.white, false, false);
            ofaBuff = Buffs.AddNewBuff("DekuOFABuff", Assets.mainAssetBundle.LoadAsset<Sprite>("lightninggreen"), Color.white, false, false);
            supaofaBuff = Buffs.AddNewBuff("DekuInfiniteOFABuff", Assets.mainAssetBundle.LoadAsset<Sprite>("lightningwhitegreen"), Color.white, false, false);
            kickBuff = Buffs.AddNewBuff("DekuKickBuff", Assets.mainAssetBundle.LoadAsset<Sprite>("kickCount"), Color.white, true, false);
            ofaBuff45 = Buffs.AddNewBuff("DekuOFA45Buff", Assets.mainAssetBundle.LoadAsset<Sprite>("lightningblue"), Color.white, false, false);
            supaofaBuff45 = Buffs.AddNewBuff("DekuInfiniteOFA45Buff", Assets.mainAssetBundle.LoadAsset<Sprite>("lightningwhiteblue"), Color.white, false, false);
            fajinBuff = Buffs.AddNewBuff("FaJinBuff", Assets.mainAssetBundle.LoadAsset<Sprite>("armorgreen"), Color.white, true, false);
            floatBuff = Buffs.AddNewBuff("DekuFloatBuff", Assets.mainAssetBundle.LoadAsset<Sprite>("Float"), Color.white, false, false);
            counterBuff = Buffs.AddNewBuff("DekuCounterBuff", Assets.mainAssetBundle.LoadAsset<Sprite>("Counter"), Color.white, false, false);
            oklahomaBuff = Buffs.AddNewBuff("DekuArmorBuff", Assets.mainAssetBundle.LoadAsset<Sprite>("armorgreen"), Color.white, false, false);
 }

        // simple helper method
        internal static BuffDef AddNewBuff(string buffName, Sprite buffIcon, Color buffColor, bool canStack, bool isDebuff)
        {
            BuffDef buffDef = ScriptableObject.CreateInstance<BuffDef>();
            buffDef.name = buffName;
            buffDef.buffColor = buffColor;
            buffDef.canStack = canStack;
            buffDef.isDebuff = isDebuff;
            buffDef.eliteDef = null;
            buffDef.iconSprite = buffIcon;

            Buffs.buffDefs.Add(buffDef);

            return buffDef;
        }

    }
}