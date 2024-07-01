using UnityEngine;
using System.IO;
using System;
using OfficeOpenXml;
using System.Text;
using UnityEditor;
using System.Reflection;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using Game.Cfg;
using Game;
using UnityEditor.Compilation;
using System.Threading;
using static UnityEditor.Progress;
using NUnit.Framework.Internal;
using OfficeOpenXml.Style.XmlAccess;

public class ExcelCfg
{
    public string language;
    public ExcelCfgItem[] cfgList;
}

[Serializable]
public class ExcelCfgItem
{
    public string excel;
    public string type;
    public int sheet;
}

/// <summary>
/// 
/// </summary>
public class BuildCfgTool
{
    private static string excelPath = "../Config/Excel/";
    private static string gameCfgPath = Application.dataPath + "/Editor/GameConfig.json";
    /// <summary>
    /// 生成的配置类型脚本位置
    /// </summary>
    public static string dataModelPath = Application.dataPath + "/Scripts/Config/Model/";
    /// <summary>
    /// 生成的配置数据文件位置
    /// </summary>
    public static string cfgDataPath = Application.streamingAssetsPath + "/cfgData.txt";

    [MenuItem("Tools/构建配置数据模型")]
    public static void BuildConfigModle()
    {
        string json = File.ReadAllText(gameCfgPath);
        ExcelCfg excelCfg = JsonUtility.FromJson<ExcelCfg>(json);
        CreateExcelListDataModel(excelCfg);
        CreateLanguageModel();
        AssetDatabase.Refresh();
        Debug.Log("构建配置数据模型完成");
    }

    [MenuItem("Tools/生成配置数据")]
    public static void BuildConfigObject()
    {
        string json = File.ReadAllText(gameCfgPath);
        ExcelCfg excelCfg = JsonUtility.FromJson<ExcelCfg>(json);
        ReadExcelToCreateDataObject(excelCfg);
        Debug.Log("生成配置数据完成");
    }

    /// <summary>
    /// 建立数据模型
    /// </summary>
    /// <param name="excelCfg"></param>
    private static void CreateExcelListDataModel(ExcelCfg excelCfg)
    {
        if (excelCfg == null || excelCfg.cfgList == null || excelCfg.cfgList.Length == 0)
        {
            Debug.LogError("配置文件解析异常");
            return;
        }

        if (Directory.Exists(dataModelPath))
        {
            Directory.Delete(dataModelPath, true);
            Directory.CreateDirectory(dataModelPath);
        }

        for (int i = 0; i < excelCfg.cfgList.Length; i++)
        {
            ExcelCfgItem item = excelCfg.cfgList[i];
            FileInfo fileInfo = new FileInfo(excelPath + item.excel); 
            if (!fileInfo.Exists)
            {
                Debug.Log("配置文件不存在" + item.excel);
                continue;
            }
            using (ExcelPackage excelPackage = new ExcelPackage(fileInfo))
            {
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets[item.sheet];
                CreateDataModelByExcel(item, worksheet);
            }
        }
    }
    /// <summary>
    /// 创建语言数据模型
    /// </summary>
    private static void CreateLanguageModel()
    {
        List<(string, string)> properyList = new List<(string, string)>
        {
            ("string", "id"),
            ("string", "text"),
            ("int", "color")
        };
        CreateCSFile("LanguageCfg", properyList);
    }

    /// <summary>
    /// 通过excel构建数据结构模型
    /// </summary>
    /// <param name="item"></param>
    /// <param name="worksheet"></param>
    private static void CreateDataModelByExcel(ExcelCfgItem item, ExcelWorksheet worksheet)
    {
        List<(string, string)> properyList = new List<(string, string)>();
        int column = worksheet.Dimension.End.Column;

        for (int i = 1; i <= column; i++)
        {       
            if (worksheet.Cells[2, i].Value.ToString() != "c")
            {
                continue;
            }
            string propertyType = worksheet.Cells[3, i].Value.ToString();
            string propertyName = worksheet.Cells[1, i].Value.ToString();
            properyList.Add((propertyType, propertyName));
        }

        CreateCSFile(item.type, properyList);
    }

    /// <summary>
    /// 生成cs文件
    /// </summary>
    /// <param name="className"></param>
    /// <param name="propertyList"></param>
    private static void CreateCSFile(string className, List<(string, string)> propertyList)
    {
        FileStream file = new FileStream(dataModelPath + className + ".cs", FileMode.Create);
        StreamWriter sw = new StreamWriter(file);
        StringBuilder classStr = new StringBuilder();
        classStr.Append("using System;\n");
        classStr.Append("namespace Game.Cfg \n{\n");
        classStr.Append("   [Serializable]\n");
        classStr.Append("   public class ");
        classStr.Append(className);
        classStr.Append(" : CfgDataBase");
        classStr.Append("\n   {\n");

        for (int i = 0; i < propertyList.Count; i++)
        {
            string propertyType = propertyList[i].Item1;
            string propertyName = propertyList[i].Item2;
            if (string.Equals(propertyName, "id"))
            {
                continue;
            }
            classStr.Append("       public ");
            classStr.Append("readonly ");
            classStr.Append(propertyType);
            classStr.Append(" ");
            classStr.Append(propertyName);
            classStr.Append(";\n");
        }

        classStr.Append("   }\n");
        classStr.Append("}");
        sw.Write(classStr);
        sw.Close();
        file.Close();
    }

    /// <summary>
    /// 将excel数据读取为数据对象
    /// </summary>
    /// <param name="excelCfg"></param>
    private static void ReadExcelToCreateDataObject(ExcelCfg excelCfg)
    {
        if (excelCfg == null || excelCfg.cfgList == null || excelCfg.cfgList.Length == 0)
        {
            Debug.LogError("配置文件解析异常");
            return;
        }
        Dictionary<Type, List<CfgDataBase>> cfgMap = new Dictionary<Type, List<CfgDataBase>>();
        ReadFileDataByList(cfgMap, excelCfg);
        ReadFileDataByFolder(cfgMap, "LanguageCfg", excelCfg.language);

        CfgData cfgDataObj = new CfgData(cfgMap);
        FileStream stream = new FileStream(cfgDataPath, FileMode.Create);
        BinaryFormatter formatter = new BinaryFormatter();
        formatter.Serialize(stream, cfgDataObj);
        stream.Close();
    }

    /// <summary>
    /// 读取excel list
    /// </summary>
    /// <param name="cfgMap"></param>
    /// <param name="excelCfg"></param>
    private static void ReadFileDataByList(Dictionary<Type, List<CfgDataBase>> cfgMap, ExcelCfg excelCfg)
    {
        for (int i = 0; i < excelCfg.cfgList.Length; i++)
        {
            ExcelCfgItem item = excelCfg.cfgList[i];
            ReadData(cfgMap, item.type, item.excel, item.sheet);
        }
    }

    /// <summary>
    /// 读取整个文件夹
    /// </summary>
    /// <param name="cfgMap"></param>
    /// <param name="type"></param>
    /// <param name="folderName"></param>
    private static void ReadFileDataByFolder(Dictionary<Type, List<CfgDataBase>> cfgMap, string type, string folderName)
    {
        string folderPath = excelPath + folderName;
        string[] files = Directory.GetFiles(folderPath);

        foreach (string file in files)
        {
            string fileName = Path.GetFileName(file);
            if (fileName.StartsWith("~$"))
            {
                continue;
            }
            ReadData(cfgMap, type, folderName + fileName, 1);
        }
    }

    private static void ReadData(Dictionary<Type, List<CfgDataBase>> cfgMap, string type, string excelName, int sheet)
    {
        FileInfo fileInfo = new FileInfo(excelPath + excelName);
        if (!fileInfo.Exists)
        {
            Debug.Log("配置文件不存在" + excelName);
            return;
        }
        using (ExcelPackage excelPackage = new ExcelPackage(fileInfo))
        {
            ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets[sheet];
            Type t = Type.GetType("Game.Cfg." + type + ",Assembly-CSharp");
            if (t == null)
            {
                Debug.Log("类型为空" + type);
            }
            List<CfgDataBase> cfgList = CreateDataObject(t, worksheet);
            if (cfgMap.ContainsKey(t))
            {
                cfgMap[t].AddRange(cfgList);
            }
            else
            {
                cfgMap.Add(t, cfgList);
            }
        }
    }

    /// <summary>
    /// 创建数据对象
    /// </summary>
    /// <param name="type"></param>
    /// <param name="worksheet"></param>
    /// <returns></returns>
    private static List<CfgDataBase> CreateDataObject(Type type, ExcelWorksheet worksheet)
    {
        FieldInfo[] fileInfo = type.GetFields(BindingFlags.Public |BindingFlags.Instance);
        FieldInfo last = fileInfo[fileInfo.Length - 1];
        FieldInfo[] temp = new FieldInfo[fileInfo.Length];
        temp[0] = last;
        Array.Copy(fileInfo, 0, temp, 1, fileInfo.Length - 1);
        fileInfo = temp;

        List<CfgDataBase> data = new List<CfgDataBase>();   

        int row = worksheet.Dimension.End.Row;
    
        for (int i = 5; i <= row; i++)
        {
            object ob = Activator.CreateInstance(type);
            for (int j = 0; j < fileInfo.Length; j++)
            {
                string value = worksheet.Cells[i, j + 1].Value != null ? worksheet.Cells[i, j + 1].Value.ToString() : "0";
                FieldInfo field = fileInfo[j];
                if (field.FieldType == typeof(string))
                {
                    field.SetValue(ob, value);
                }
                else if(field.FieldType == typeof(int))
                {
                    if(int.TryParse(value, out int intValue))
                    {
                        field.SetValue(ob, intValue);
                    }
                    else
                    {
                        Debug.LogError("int转换失败" + value);
                    }
                }
                else if (field.FieldType == typeof(bool))
                {
                    field.SetValue(ob, value == "1");
                }
                else
                {
                    field.SetValue(ob, value);
                }
                
            }
            data.Add(ob as CfgDataBase);
        }

        return data;
    }
}




