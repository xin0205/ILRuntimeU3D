using ILRuntime.CLR.TypeSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class MainMonoBehaviour : MonoBehaviour
{
    [SerializeField]
    private string m_HotfixMonoBehaviourName;

    public object HotfixInstance;
   
    private InstanceMethod m_Start;
    
    private InstanceMethod m_Test;
    private InstanceMethod m_Update;

    public int Test;

    private void Awake()
    {
        Test = 10;

        string hotfixMonoBehaviourFullName = "HotFix_Project." + m_HotfixMonoBehaviourName;

        if (HotfixManager.Instance.ILRuntimeMode)
        {
            IType type = HotfixManager.Instance.Appdomain.LoadedTypes[hotfixMonoBehaviourFullName];
            HotfixInstance = ((ILType)type).Instantiate();

            m_Start = new InstanceMethod(HotfixInstance, hotfixMonoBehaviourFullName, "Start", 0);
            m_Update = new InstanceMethod(HotfixInstance, hotfixMonoBehaviourFullName, "Update", 0);
            m_Test = new InstanceMethod(HotfixInstance, hotfixMonoBehaviourFullName, "Test", 1);

            HotfixManager.Instance.Appdomain.Invoke(hotfixMonoBehaviourFullName, "Awake", HotfixInstance, this);
        }
        else {
            HotfixInstance = HotfixManager.Instance.HotfixAssembly.CreateInstance(hotfixMonoBehaviourFullName);

            InstanceMethod awake = new InstanceMethod(HotfixInstance, hotfixMonoBehaviourFullName, "Awake", new Type[] { typeof(MainMonoBehaviour) });
            m_Start = new InstanceMethod(HotfixInstance, hotfixMonoBehaviourFullName, "Start", null);
            m_Update = new InstanceMethod(HotfixInstance, hotfixMonoBehaviourFullName, "Update", null);
            m_Test = new InstanceMethod(HotfixInstance, hotfixMonoBehaviourFullName, "Test", new Type[] { typeof(int)});

            awake?.Invoke(this);
        }        
        m_Test.Invoke(5);

        
    }

    void Start()
    {
        m_Start?.Invoke();
    }

    void Update()
    {
        m_Update?.Invoke();
    }
}
