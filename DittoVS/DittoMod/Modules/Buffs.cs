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

        //transform
        internal static BuffDef transformBuff;
        internal static BuffDef transformdeBuff;

        //items
        internal static BuffDef assaultvestBuff;
        internal static BuffDef choicescarfBuff;
        internal static BuffDef choicebandBuff;
        internal static BuffDef choicespecsBuff;
        internal static BuffDef leftoversBuff;
        internal static BuffDef lifeorbBuff;
        internal static BuffDef luckyeggBuff;
        internal static BuffDef rockyhelmetBuff;
        internal static BuffDef scopelensBuff;
        internal static BuffDef shellbellBuff;

        //abilities
        internal static BuffDef flamebodyBuff;
        internal static BuffDef hugepowerBuff;
        internal static BuffDef levitateBuff;
        internal static BuffDef magicguardBuff;
        internal static BuffDef moodyBuff;
        internal static BuffDef moxieBuff;
        internal static BuffDef multiscaleBuff;
        internal static BuffDef sniperBuff;

        internal static void RegisterBuffs()
        {
            Sprite fractureddebuff = Addressables.LoadAssetAsync<BuffDef>("RoR2/DLC1/BleedOnHitVoid/bdFracture.asset").WaitForCompletion().iconSprite;

            transformdeBuff = Buffs.AddNewBuff("transformdeBuff", fractureddebuff, Color.white, true, true);
            transformBuff = Buffs.AddNewBuff("transformBuff", Assets.mainAssetBundle.LoadAsset<Sprite>("Transform"), Color.white, true, false);
            assaultvestBuff = Buffs.AddNewBuff("assaultvestBuff", Assets.mainAssetBundle.LoadAsset<Sprite>("AssaultVest"), Color.white, false, false);
            choicescarfBuff = Buffs.AddNewBuff("choicescarfBuff", Assets.mainAssetBundle.LoadAsset<Sprite>("ChoiceScarf"), Color.white, false, false);
            choicebandBuff = Buffs.AddNewBuff("choicebandBuff", Assets.mainAssetBundle.LoadAsset<Sprite>("ChoiceBand"), Color.white, false, false);
            choicespecsBuff = Buffs.AddNewBuff("choicespecsBuff", Assets.mainAssetBundle.LoadAsset<Sprite>("ChoiceSpecs"), Color.white, false, false);
            leftoversBuff = Buffs.AddNewBuff("leftoversBuff", Assets.mainAssetBundle.LoadAsset<Sprite>("Leftovers"), Color.white, false, false);
            lifeorbBuff = Buffs.AddNewBuff("lifeorbBuff", Assets.mainAssetBundle.LoadAsset<Sprite>("LifeOrb"), Color.white, false, false);
            luckyeggBuff = Buffs.AddNewBuff("luckyeggBuff", Assets.mainAssetBundle.LoadAsset<Sprite>("LuckyEgg"), Color.white, false, false);
            rockyhelmetBuff = Buffs.AddNewBuff("rockyhelmetBuff", Assets.mainAssetBundle.LoadAsset<Sprite>("RockyHelmet"), Color.white, false, false);
            scopelensBuff = Buffs.AddNewBuff("scopelensBuff", Assets.mainAssetBundle.LoadAsset<Sprite>("ScopeLens"), Color.white, false, false);
            shellbellBuff = Buffs.AddNewBuff("shellbellBuff", Assets.mainAssetBundle.LoadAsset<Sprite>("ShellBell"), Color.white, false, false);

            flamebodyBuff = Buffs.AddNewBuff("flamebodyBuff", Assets.blazingBuffIcon, Color.red, false, false);
            hugepowerBuff = Buffs.AddNewBuff("hugepowerBuff", Assets.elephantBuffIcon, Color.blue, false, false);
            levitateBuff = Buffs.AddNewBuff("levitateBuff", Assets.levitateBuffIcon, Color.magenta, false, false);
            magicguardBuff = Buffs.AddNewBuff("magicguardBuff", Assets.brainBuffIcon, new Color(0.86f, 0.44f, 0.59f), false, false);
            moodyBuff = Buffs.AddNewBuff("moodyBuff", Assets.boostBuffIcon, new Color(0.59f, 0.31f, 0.16f), false, false);
            moxieBuff = Buffs.AddNewBuff("moxieBuff", Assets.mainAssetBundle.LoadAsset<Sprite>("ShellBell"), Color.white, false, false);
            multiscaleBuff = Buffs.AddNewBuff("multiscaleBuff", Assets.mainAssetBundle.LoadAsset<Sprite>("ShellBell"), Color.white, false, false);
            sniperBuff = Buffs.AddNewBuff("sniperBuff", Assets.mainAssetBundle.LoadAsset<Sprite>("ShellBell"), Color.white, false, false);

            //Sprite TransformBuff = Addressables.LoadAssetAsync<BuffDef>("RoR2/Base/LunarSkillReplacements/bdLunarSecondaryRoot.asset").WaitForCompletion().iconSprite;
            //transformBuff = AddNewBuff("TransformTimer", TransformBuff, Color.yellow, false, false);

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
