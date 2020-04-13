using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class HotfixManager 
{
    private static HotfixManager instance;

    public static HotfixManager Instance
    {

        get {
            if (instance == null)
            {
                instance = new HotfixManager();
            }
            return instance;
        }
        
    }

    public bool ILRuntimeMode;

    public ILRuntime.Runtime.Enviorment.AppDomain Appdomain;

    public Assembly HotfixAssembly;

}
