using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using MVC;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using System.Linq;

public class DataModelTreeItem : TreeViewItem 
{
    public bool IsNull => model == null;

    private IDataModel model;

    public DataModelTreeItem(IDataModel model) : base(-1, -1, "")
    {
        this.model = model;
        children = new List<TreeViewItem>();
    }

    public DataModelTreeItem(IDataModel model, int depth):base(model.GetHashCode(), depth)
    {
        this.model = model;
        children = new List<TreeViewItem>();
    }

    public void GenerateChild()
    {
        children.Clear();
        if (model == null)
        {
            return;
        }
        if (model.IsDataBase) 
        {
            return;
        }
        
        if (model.ValueType == ValueType.Container) 
        {
            DataContainer container = model as DataContainer;
            foreach (var item in container)
            {
                DataModelTreeItem treeItem = new DataModelTreeItem(item, depth + 1);
                children.Add(treeItem);
            }
        }

        if (model.ValueType == ValueType.Collection)
        {
            DataCollection collection = model as DataCollection;
            foreach (var item in collection)
            {
                DataModelTreeItem treeItem = new DataModelTreeItem(item, depth + 1);
                children.Add(treeItem);
            }
        }
    }

    public void GenerateChild(DataContainer container)
    {
        children.Clear();
        if (container == null || container.Count == 0)
        {
            return;
        }
        foreach (var item in container)
        {
            DataModelTreeItem treeItem = new DataModelTreeItem(item, depth + 1);
            children.Add(treeItem);
        }
    }

    public void RefreshChild()
    {
        GenerateChild();
        if (hasChildren)
        {

            foreach (var item in children.Cast<DataModelTreeItem>())
            {
                item.RefreshChild();
            }
        }

        displayName = ToString();
    }

    public override string ToString()
    {
        if (IsNull)
        {
            return "Root";
        }

        if (model.IsDataBase)
        {
            return model.ToString();
        }

        if (model.ValueType == ValueType.Container)
        {
            DataContainer container = model as DataContainer;
            return $"{container.Key}[count:{container.Count}]";
        }

        if (model.ValueType == ValueType.Collection)
        {
            DataCollection collection = model as DataCollection;
            return $"{collection.Key}[count:{collection.Count}]";
        }

        return base.ToString();
    }
}

/// <summary>
/// 
/// </summary>
public class DataModelTree : TreeView
{
    private DataModelTreeItem root;

    public DataModelTree(TreeViewState state) : base(state)
    {
    }

    public DataModelTree(TreeViewState state, MultiColumnHeader multiColumnHeader) : base(state, multiColumnHeader)
    {
    }

    public override void OnGUI(Rect rect)
    {
        base.OnGUI(rect);

        //if (Event.current.type == EventType.MouseDown && Event.current.button == 0 && rect.Contains(Event.current.mousePosition))
        //{
        //    SetSelection(new int[0], TreeViewSelectionOptions.FireSelectionChanged);
        //}

        //root?.RefreshChild();
    }

    protected override TreeViewItem BuildRoot()
    {
        if (root == null || root.IsNull)
        {
            root = new DataModelTreeItem(DataContainer.Root);
        }

        return root;
    }

    public void RefreshAll()
    {
        BuildRoot();
        root.GenerateChild();
        root.RefreshChild();
    }
}

public class DataModelWindow : EditorWindow
{
    private TreeViewState treeState;
    private DataModelTree tree;

    private float currScrollViewHeight = 40;
    private Rect cursorChangeRect;

    private void Awake()
    {
        currScrollViewHeight = position.height - 145;
        cursorChangeRect = new Rect(0, currScrollViewHeight, position.width, 5);

        Debug.Log("Awake");
        //DataContainer.CreateRoot();
        //DataContainer Game = DataContainer.Root.CreateContainer("Game");
        //DataContainer Item = Game.CreateContainer("Item");
        //DataContainer Role = Game.CreateContainer("Role");

        //Item.SetBaseValue("count", 24);

        //DataCollection ItemList = Item.CreateCollection("ItemList");
        //for (int i = 0; i < 4; i++)
        //{
        //    DataContainer item = ItemList.Append();
        //    item.SetBaseValue("id", i);
        //    item.SetBaseValue("name", $"name_{i}");
        //}

        //DataCollection RoleList = Role.CreateCollection("RoleList");
        //for (int i = 0; i < 3; i++)
        //{
        //    DataContainer role = RoleList.Append();
        //    role.SetBaseValue("id", i);
        //    role.SetBaseValue("name", $"name_{i}");
        //}

        //DataContainer Game2 = DataContainer.Root.CreateContainer("Game2");
    }

    private void OnEnable()
    {
        if (tree == null)
        {
            treeState = new TreeViewState();
            tree = new DataModelTree(treeState);
            tree.Reload();
        }
        OnFocus();
    }

    private void OnFocus()
    {

    }

    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(0, 0, position.width, position.height));
        OnTreeGUI();
        GUILayout.EndArea();
    }

    void OnTreeGUI()
    {
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Refresh"))
        {
            if (Application.isPlaying)
            {                
                tree.RefreshAll();
            }
        }
        GUILayout.EndHorizontal();

        tree.Reload();
        var rc = new Rect(0, 70, position.width, position.height - 20);
        tree.OnGUI(rc);
    }

    [MenuItem("Tools/Game/DataModel", false, 1)]
    static void OpenDBManager()
    {
        DataModelWindow window = (DataModelWindow)GetWindowWithRect(typeof(DataModelWindow), new Rect(0, 0, 300, 250), false, "DataModel");
        window.maximized = true;
        window.wantsMouseEnterLeaveWindow = true;
        window.wantsMouseMove = true;
        window.maxSize = new Vector2(2000, 2000);

        window.Show();
    }
}

