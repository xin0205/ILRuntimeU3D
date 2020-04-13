using ILRuntime.CLR.Method;
using ILRuntime.Runtime.Enviorment;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

/// <summary>
/// ILRuntime实例方法
/// </summary>
public class InstanceMethod
{

    /// <summary>
    /// 热更新层实例
    /// </summary>
    private object m_HotfixInstance;

    /// <summary>
    /// 热更新层方法
    /// </summary>
    private IMethod m_Method;

    private MethodInfo m_MethodInfo;

    private bool m_ILRuntimeMode;

    /// <summary>
    /// 方法参数缓存
    /// </summary>
    private object[] m_Params;

    public InstanceMethod(object hotfixInstance, string typeName, string methodName, int paramCount)
    {
        InteralConstruction(hotfixInstance, typeName, methodName, paramCount, true);
        m_Method = HotfixManager.Instance.Appdomain.LoadedTypes[typeName].GetMethod(methodName, paramCount);

    }

    public InstanceMethod(object hotfixInstance, string typeName, string methodName, Type[] paramsTypes)
    {
        paramsTypes = paramsTypes == null ? new Type[] { } : paramsTypes;

        InteralConstruction(hotfixInstance, typeName, methodName, paramsTypes.Length, false);
        Type type = HotfixManager.Instance.HotfixAssembly.GetType(typeName);
        m_MethodInfo = type.GetMethod(methodName, paramsTypes);

    }

    private void InteralConstruction(object hotfixInstance, string typeName, string methodName, int paramCount, bool ilRuntimeMode) {

        this.m_ILRuntimeMode = ilRuntimeMode;
        m_HotfixInstance = hotfixInstance;

        m_Params = new object[paramCount];
    }

    public void Invoke()
    {
        InternalInvoke();

    }

    public void Invoke(object a)
    {
        m_Params[0] = a;
        InternalInvoke();
    }

    public void Invoke(object a, object b)
    {
        m_Params[0] = a;
        m_Params[1] = b;
        InternalInvoke();
    }

    public void Invoke(object a, object b, object c)
    {
        m_Params[0] = a;
        m_Params[1] = b;
        m_Params[2] = c;

        InternalInvoke();
    }

    private void InternalInvoke() {
        if (m_ILRuntimeMode)
        {
            HotfixManager.Instance.Appdomain.Invoke(m_Method, m_HotfixInstance, m_Params);
        }
        else
        {
            m_MethodInfo.Invoke(m_HotfixInstance, m_Params);
        }
    }
}

