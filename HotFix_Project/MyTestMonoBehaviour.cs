using System;
using System.Collections.Generic;

namespace HotFix_Project
{
    public class MyTestHotfixMonoBehaviour : HotfixMonoBehaviour
    {
        public override void Awake(MainMonoBehaviour mainMonoBehaviour)
        {
            base.Awake(mainMonoBehaviour);
            UnityEngine.Debug.Log(mainMonoBehaviour.Test);
        }

        public override void Start()
        {
            base.Start();
            UnityEngine.Debug.Log("MyTestHotfixMonoBehaviour Start");
        }

        public void Test(int a)
        {
            UnityEngine.Debug.Log("Test:" + a);
        }

        public override void Update()
        {
            base.Update();
        }
    }


}
