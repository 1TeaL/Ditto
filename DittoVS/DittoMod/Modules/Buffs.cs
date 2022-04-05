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

        //internal static BuffDef transformBuff;
        internal static BuffDef choicescarfBuff;
        internal static BuffDef choicebandBuff;
        internal static BuffDef choicespecsBuff;
        internal static BuffDef leftoversBuff;
        internal static BuffDef rockyhelmetBuff;
        internal static BuffDef scopelensBuff;
        internal static BuffDef shellbellBuff;

        internal static void RegisterBuffs()
        {
            choicescarfBuff = Buffs.AddNewBuff("choicescarfBuff", Assets.mainAssetBundle.LoadAsset<Sprite>("ChoiceScarf"), Color.white, false, false);
            choicebandBuff = Buffs.AddNewBuff("choicebandBuff", Assets.mainAssetBundle.LoadAsset<Sprite>("ChoiceBand"), Color.white, false, false);
            choicespecsBuff = Buffs.AddNewBuff("choicespecsBuff", Assets.mainAssetBundle.LoadAsset<Sprite>("ChoiceSpecs"), Color.white, false, false);
            leftoversBuff = Buffs.AddNewBuff("leftoversBuff", Assets.mainAssetBundle.LoadAsset<Sprite>("Leftovers"), Color.white, false, false);
            rockyhelmetBuff = Buffs.AddNewBuff("rockyhelmetBuff", Assets.mainAssetBundle.LoadAsset<Sprite>("RockyHelmet"), Color.white, false, false);
            scopelensBuff = Buffs.AddNewBuff("scopelensBuff", Assets.mainAssetBundle.LoadAsset<Sprite>("ScopeLens"), Color.white, false, false);
            shellbellBuff = Buffs.AddNewBuff("shellbellBuff", Assets.mainAssetBundle.LoadAsset<Sprite>("ShellBell"), Color.white, false, false);

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