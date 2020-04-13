using UnityEngine;
using System.Collections;
using System.IO;
using ILRuntime.Runtime.Enviorment;
using System.Reflection;
using System;

public class InitILRuntime : MonoBehaviour
{
    [SerializeField]
    private bool m_ILRuntimeMode;

    [SerializeField]
    private GameObject m_TestPrefab;

    System.IO.MemoryStream fs;
    System.IO.MemoryStream p;

    void Start()
    {
        HotfixManager.Instance.ILRuntimeMode = m_ILRuntimeMode;

        if (HotfixManager.Instance.ILRuntimeMode)
        {
            StartCoroutine(LoadDllAndPdb(LoadILRuntime));
        }
        else {
            StartCoroutine(LoadDllAndPdb(LoadHotFixAssembly));
        }


        

    }

    IEnumerator LoadDllAndPdb(Action<byte[], byte[]> loadCallback) {
#if UNITY_ANDROID
        WWW www = new WWW(Application.streamingAssetsPath + "/HotFix_Project.dll");
#else
        WWW www = new WWW("file:///" + Application.streamingAssetsPath + "/HotFix_Project.dll");
#endif
        while (!www.isDone)
            yield return null;
        if (!string.IsNullOrEmpty(www.error))
            UnityEngine.Debug.LogError(www.error);
        byte[] dll = www.bytes;
        www.Dispose();

#if UNITY_ANDROID
        www = new WWW(Application.streamingAssetsPath + "/HotFix_Project.pdb");
#else
        www = new WWW("file:///" + Application.streamingAssetsPath + "/HotFix_Project.pdb");
#endif
        while (!www.isDone)
            yield return null;
        if (!string.IsNullOrEmpty(www.error))
            UnityEngine.Debug.LogError(www.error);
        byte[] pdb = www.bytes;

        InitializeILRuntime();

        loadCallback(dll, pdb);
        
    }

    private void LoadILRuntime(byte[] dll, byte[] pdb) {
        fs = new MemoryStream(dll);
        p = new MemoryStream(pdb);
        HotfixManager.Instance.Appdomain = new ILRuntime.Runtime.Enviorment.AppDomain();
        HotfixManager.Instance.Appdomain.LoadAssembly(fs, p, new ILRuntime.Mono.Cecil.Pdb.PdbReaderProvider());

        GameObject.Instantiate(m_TestPrefab, this.transform);
    }

    private void LoadHotFixAssembly(byte[] dll, byte[] pdb)
    {
        HotfixManager.Instance.HotfixAssembly = Assembly.Load(dll, pdb);

        GameObject.Instantiate(m_TestPrefab, this.transform);
    }

    void InitializeILRuntime()
    {
        //这里做一些ILRuntime的注册，HelloWorld示例暂时没有需要注册的
    }

    private void OnDestroy()
    {
        if (fs != null)
            fs.Close();
        if (p != null)
            p.Close();
        fs = null;
        p = null;
    }

}
