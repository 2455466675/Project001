using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework.Core
{
    public class SampleData : ISavableData
    {
        public static SampleData CreateSampleData(int index)
        {
            SampleData sampleData = new SampleData();
            sampleData.intValue = UnityEngine.Random.Range(1, 100);
            sampleData.boolValue = UnityEngine.Random.Range(1, 3) == 1;
            sampleData.floatValue = UnityEngine.Random.Range(1.1f, 1.9f);
            sampleData.DoubleValue = UnityEngine.Random.Range(1.1f, 1.9f);
            sampleData.StringValue = "StringValue_" + index.ToString();

            sampleData.subData = new SampleData();

            return sampleData;
        }

        public int intValue;
        public bool boolValue;
        public float floatValue;
        private double doubleValue;
        private string stringValue;
        private SampleData subData;

        public double DoubleValue
        {
            get { return doubleValue; }
            set { doubleValue = value; }
        }
        public string StringValue
        {
            get { return stringValue; }
            set { stringValue = value; }
        }

        void ISavableData.OnWrite(IWriter writer)
        {
            writer.Write("int_value", intValue);
            writer.Write("bool_value", boolValue);
            writer.Write("float_value", floatValue);
            writer.Write("double_value", doubleValue);
            writer.Write("string_value", stringValue);
            writer.Write("sub_data", subData);
        }

        void ISavableData.OnRead(IReader reader)
        {
            intValue = reader.ReadInt("int_value");
            boolValue = reader.ReadBool("bool_value");
            floatValue = reader.ReadFloat("float_value");
            doubleValue = reader.ReadDouble("double_value");
            stringValue = reader.ReadString("string_value");
            subData = reader.ReadData<SampleData>("sub_data");
        }
    }

    [Gameplay]
    public class SampleModule : IGameplay, ISavableGameModule
    {
        private int intValue;
        private bool boolValue;
        private float floatValue;
        private double doubleValue;
        private string stringValue;
        private SampleData sampleData;
        private List<SampleData> listValue;
        private Dictionary<int, SampleData> dicIntKeyValue;
        private Dictionary<string, SampleData> dicStrKeyValue;

        private List<int> listIntValue;

        public void OnInit()
        {
            MDebug.Log("SampleModule OnInit !");

            
        }

        public void OnExit()
        {
            
        }

        public void OnSaveGame(IWriter writer)
        {
            intValue = 132;
            boolValue = true;
            floatValue = 4.25f;
            doubleValue = 547.65878d;
            stringValue = "SampleModule stringValue ¹þ¹þ¹þ";

            sampleData = SampleData.CreateSampleData(-1);

            listValue = new List<SampleData>();
            dicIntKeyValue = new Dictionary<int, SampleData>();
            dicStrKeyValue = new Dictionary<string, SampleData>();

            listIntValue = new List<int>();

            for (int i = 0; i < 100; i+=3)
            {
                listValue.Add(SampleData.CreateSampleData(i));
                dicIntKeyValue.Add(i + 1, SampleData.CreateSampleData(i + 1));
                dicStrKeyValue.Add("key_" + (i + 2), SampleData.CreateSampleData(i + 2));
                listIntValue.Add(i);
            }

            writer.Write("intValue", intValue);
            writer.Write("boolValue", boolValue);
            writer.Write("floatValue", floatValue);
            writer.Write("doubleValue", doubleValue);
            writer.Write("stringValue", stringValue);
            writer.Write("sampleData", sampleData);
            writer.Write("listValue", listValue);
            writer.Write("dicIntKeyValue", dicIntKeyValue);
            writer.Write("dicStrKeyValue", dicStrKeyValue);
        }

        public void OnLoadGame(IReader reader)
        {
            intValue = reader.ReadInt("intValue");
            boolValue = reader.ReadBool("boolValue");
            floatValue = reader.ReadFloat("floatValue");
            doubleValue = reader.ReadDouble("doubleValue");
            stringValue = reader.ReadString("stringValue");
            sampleData = reader.ReadData<SampleData>("sampleData");
            listValue = reader.ReadList<SampleData>("listValue");
            dicIntKeyValue = reader.ReadDicWithIntKey<SampleData>("dicIntKeyValue");
            dicStrKeyValue = reader.ReadDicWithStringKey<SampleData>("dicStrKeyValue");
        }
    }
}

