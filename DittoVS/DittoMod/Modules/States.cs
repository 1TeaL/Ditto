using DittoMod.SkillStates;
using System.Collections.Generic;
using System;

namespace DittoMod.Modules
{
    public static class States
    {
        internal static List<Type> entityStates = new List<Type>();

        internal static void RegisterStates()
        {
            entityStates.Add(typeof(Airforce));
            entityStates.Add(typeof(Transform));
        }
    }
}