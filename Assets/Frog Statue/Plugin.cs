/*
// ---------------------------------------
// An example of a starting point for a Script that is added to the mod


using System;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using Bindito.Core;
using TimberApi.ConsoleSystem;
using TimberApi.DependencyContainerSystem;
using TimberApi.ModSystem;

using System.Collections.Generic;
using System.Linq;
using System.Text;

using HarmonyLib;
using UnityEngine;


namespace Staircase
{
    
    public class Plugin : IModEntrypoint
    {
        public const string PluginGuid = "knattetobbert.staircase";
        public const string PluginName = "Staircase";
        public const string PluginVersion = "1.3.8";

    

        public void Entry(IMod mod, IConsoleWriter consoleWriter)
        {
    
            Debug.Log($"Loaded {PluginName} Script");
            new Harmony(PluginGuid).PatchAll();
            
        }
    }

    [HarmonyPatch(typeof(Debug), "LogWarning", typeof(object))]
    public class LogWarningPatch
    {
        static bool Prefix(object message, bool __runOriginal)
        {
            if (__runOriginal)
            {
                string mess = message as string;
                if (mess != null && mess.Contains("path marker mesh at"))
                {
                    return false;
                }
            }
            return __runOriginal;
        }
    }
}*/