using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ET
{
    public class Test : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            UIManager.Instance.OpenWindow(UIEnum.Panel_Main, UIPackageEnum.Main);
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}