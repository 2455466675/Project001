using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace Game
{
    [Serializable]
    public class MyTest
    {
        [SerializeField]
        public int id;
        [SerializeField]
        public string uid;
        [SerializeField]
        public List<int> list = new List<int>();
        [SerializeField]
        public Dictionary<string, string> data;
        public MyTest() 
        {
            int id = 10;
            string uid = "LC";
            this.id = id;
            this.uid = uid;
            data = new Dictionary<string, string>(); 
            for (int i = 0; i < id; i++)
            {
                list.Add(i);
                data[i.ToString()] = uid + i;
            }
        }

        public override string ToString()
        {
            string print = "";

            print = print + id + ";" + uid + ";";

            for (int i = 0; i < list.Count; i++)
            {
                print = print + list[i] + ";" + data[i.ToString()] + ";"; 
            }
                
            return print;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class Test : MonoBehaviour
    {
        public Image image;
        private void Awake()
        {
            ES3.Save("test", "这是一个测试数据", "test");
            MyTest test = new MyTest();
            ES3.Save<MyTest>(nameof(MyTest), test, "test");
        }
        private void Start()
        {
            Debug.Log("path:" + Application.persistentDataPath);
            string test = ES3.Load<string>("test","test");
            Debug.Log("test:" + test);

            MyTest myTest = ES3.Load<MyTest>(nameof(MyTest), "test");
            Debug.Log("myTest:" + myTest.ToString());
            image.transform.DOLocalMove(new Vector3(500, 500, 10), 5);
        }
    }
}