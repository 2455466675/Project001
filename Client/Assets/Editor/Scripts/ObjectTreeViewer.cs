using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

public class ObjectTreeViewer : EditorWindow
{
    private class NodeData
    {
        public int Id;
        public bool IsClass;
        public int HashCode;
        public string Name;
        public string Value;
        public string Type;
        public NodeData Parent;
        public object OriginalObject;
        public FieldInfo SourceField;
        public PropertyInfo SourceProperty;
        // 如果此节点是容器（List/Dict）中的元素，记录其在容器中的 key（list 为 int 索引，dict 为原始 key 对象）
        public object ContainerKey;
        public bool IsContainerEntry;

        public List<NodeData> Children;

        public NodeData(NodeData parent, int treeId, bool isClass, int hashCode, string name, string value, string typeName)
        {
            Children = new();
            Parent = parent;
            Id = treeId;
            IsClass = isClass;
            HashCode = hashCode;
            Name = name;
            Value = value;
            Type = typeName;
        }

        public void AddChild(NodeData node)
        {
            Children.Add(node);
        }

        public bool IsCycle(int hashCode)
        {
            if (Parent == null)
                return false;
            if (Parent.HashCode == hashCode)
                return true;
            return Parent.IsCycle(hashCode);
        }
    }

    private class MyTreeViewItem : UnityEditor.IMGUI.Controls.TreeViewItem
    {
        public NodeData NodeData { get; set; }
        public bool IsEditing { get; set; }
        public string EditValue { get; set; }

        public MyTreeViewItem(int id, int depth, string displayName, NodeData nodeData) : base(id, depth, displayName)
        {
            NodeData = nodeData;
            IsEditing = false;
        }
    }

    private class ObjectTreeView : UnityEditor.IMGUI.Controls.TreeView
    {
        private ObjectTreeViewer window;
        private MyTreeViewItem editingItem;
        private bool isEditing = false;

        public ObjectTreeView(TreeViewState state, ObjectTreeViewer owner) : base(state)
        {
            window = owner;
            showAlternatingRowBackgrounds = true;
            showBorder = true;

            Reload();
        }

        protected override UnityEditor.IMGUI.Controls.TreeViewItem BuildRoot()
        {
            var root = new UnityEditor.IMGUI.Controls.TreeViewItem { id = -1, depth = -1, displayName = "Root" };

            if (window.targetObject != null && window.rootNodeData != null)
            {
                var item = BuildTreeItem(window.rootNodeData, 0);
                if (item != null)
                    root.AddChild(item);
            }

            SetupDepthsFromParentsAndChildren(root);
            return root;
        }

        private MyTreeViewItem BuildTreeItem(NodeData nodeData, int depth)
        {
            var item = new MyTreeViewItem(nodeData.Id, depth, GetDisplayName(nodeData), nodeData);

            if (nodeData.Children != null)
            {
                foreach (var child in nodeData.Children)
                {
                    var childItem = BuildTreeItem(child, depth + 1);
                    if (childItem != null)
                        item.AddChild(childItem);
                }
            }

            return item;
        }

        private string GetDisplayName(NodeData nodeData)
        {
            if (nodeData == null) return "null";

            if (nodeData.IsClass || nodeData.Value == "null")
            {
                return $"{nodeData.Name}: {nodeData.Value} ({nodeData.Type})";
            }
            else
            {
                return $"{nodeData.Name}: {nodeData.Value}";
            }            
        }

        protected override void RowGUI(RowGUIArgs args)
        {
            var item = args.item as MyTreeViewItem;
            if (item == null || item.NodeData == null)
            {
                base.RowGUI(args);
                return;
            }

            var rect = args.rowRect;
            var labelRect = rect;
            labelRect.x += GetContentIndent(args.item);
            labelRect.width -= GetContentIndent(args.item);

            var style = new GUIStyle(EditorStyles.label);

            if (item.NodeData.Value == "null")
                style.normal.textColor = Color.gray;
            else if (item.NodeData.Type == "int" || item.NodeData.Type == "float" || item.NodeData.Type == "double" ||
                     item.NodeData.Type == "long" || item.NodeData.Type == "short" || item.NodeData.Type == "byte")
                style.normal.textColor = new Color(0.6f, 0.8f, 1f);
            else if (item.NodeData.Type == "string")
                style.normal.textColor = new Color(1f, 0.8f, 0.6f);
            else if (item.NodeData.Type == "bool")
                style.normal.textColor = new Color(0.8f, 0.6f, 1f);
            else if (item.NodeData.IsClass)
                style.normal.textColor = new Color(0.8f, 1f, 0.8f);

            GUI.Label(labelRect, GetDisplayName(item.NodeData), style);
        }

        protected override void KeyEvent()
        {
            if (Event.current.type == EventType.KeyDown)
            {
                if (Event.current.keyCode == KeyCode.F2 && HasSelection())
                {
                    var selectedIds = GetSelection();
                    if (selectedIds != null && selectedIds.Count > 0)
                    {
                        EditItem(selectedIds[0]);
                    }
                }
            }

            base.KeyEvent();
        }

        protected override void DoubleClickedItem(int id)
        {            
            EditItem(id);
        }

        protected override bool CanRename(UnityEditor.IMGUI.Controls.TreeViewItem item)
        {
            var _item = item as MyTreeViewItem;
            if (_item == null)
            {
                return false;
            }
            if (_item.NodeData == null)
            {
                return false;
            }
            if (_item.NodeData.IsClass)
            {
                return false;
            }
            if (_item.NodeData.Children?.Count > 0)
            {
                return false;
            }
            if (_item.NodeData.Value == "null" || _item.NodeData.Value == "循环引用")
            {
                return false;
            }
            if (_item.NodeData.Type == "bool")
            {
                return false;
            }
            // 结构体字段不可编辑（因为修改 struct 字段需要写回整个 struct，实现复杂）
            if (IsStructField(_item.NodeData))
            {
                return false;
            }
            return true;
        }

        private void EditItem(int id)
        {
            var selectedItem = FindItem(id, rootItem) as MyTreeViewItem;
            if (selectedItem != null)
            {
                if (selectedItem.NodeData != null && selectedItem.NodeData.Type == "bool" && CanToggleBool(selectedItem))
                {
                    ToggleBoolValue(selectedItem);
                    Event.current.Use();
                    return;
                }

                if (CanRename(selectedItem))
                {
                    StartEditing(selectedItem);
                    Event.current.Use();
                    return;
                }
            }
        }

        private bool CanToggleBool(MyTreeViewItem item)
        {
            if (item?.NodeData == null) return false;
            if (item.NodeData.Type != "bool") return false;
            if (item.NodeData.IsClass) return false;
            if (item.NodeData.Children?.Count > 0) return false;
            if (item.NodeData.Value == "null" || item.NodeData.Value == "循环引用") return false;
            if (IsStructField(item.NodeData)) return false;
            return true;
        }

        // 检查节点是否属于某个结构体（沿父链向上找）
        private bool IsStructField(NodeData nodeData)
        {
            if (nodeData == null) return false;
            var current = nodeData.Parent;
            while (current != null)
            {
                var obj = current.OriginalObject;
                if (obj != null && obj.GetType().IsValueType)
                {
                    return true;
                }
                current = current.Parent;
            }
            return false;
        }

        protected override void RenameEnded(RenameEndedArgs args)
        {
            base.RenameEnded(args);
            EndEditing(args);
        }

        private void StartEditing(MyTreeViewItem item)
        {
            editingItem = item;
            editingItem.IsEditing = true;
            editingItem.EditValue = editingItem.NodeData.Value;
            editingItem.displayName = editingItem.NodeData.Value;
            isEditing = true;
            BeginRename(item);
        }

        private void EndEditing(RenameEndedArgs args)
        {
            if (editingItem == null || !isEditing)
                return;

            if (args.acceptedRename && args.newName != null && args.newName != args.originalName)
            {
                editingItem.EditValue = args.newName;
                ApplyValueChange(editingItem);
            }

            editingItem.IsEditing = false;
            editingItem.EditValue = null;
            editingItem = null;
            isEditing = false;

            window.RefreshTree();
        }

        private void ToggleBoolValue(MyTreeViewItem item)
        {
            var nodeData = item.NodeData;
            var currentValue = nodeData.Value.ToLower();
            bool newValue;

            if (currentValue == "true")
            {
                newValue = false;
            }
            else if (currentValue == "false")
            {
                newValue = true;
            }
            else
            {
                Debug.LogWarning($"无法识别的 bool 值: {nodeData.Value}");
                return;
            }
            item.EditValue = newValue.ToString();
            ApplyValueChange(item);
            window.RefreshTree();
        }

        private void ApplyValueChange(MyTreeViewItem item)
        {
            try
            {
                var nodeData = item.NodeData;

                var targetObj = GetTargetObject(nodeData);
                if (targetObj == null)
                {
                    Debug.LogWarning($"无法获取父对象，无法修改值");
                    return;
                }

                object convertedValue = ConvertValue(item.EditValue, nodeData.Type);
                if (convertedValue == null && nodeData.Type != "null" && nodeData.Type != "string")
                {
                    Debug.LogWarning($"值转换失败: {item.EditValue} -> {nodeData.Type}");
                    return;
                }

                // 通过反射字段/属性写回
                if (nodeData.SourceField != null || (nodeData.SourceProperty != null && nodeData.SourceProperty.CanWrite))
                {
                    if (nodeData.SourceField != null)
                    {
                        nodeData.SourceField.SetValue(targetObj, convertedValue);
                        Debug.Log($"成功修改字段 {nodeData.Name} 的值: {nodeData.Value} -> {convertedValue}");
                    }
                    else
                    {
                        nodeData.SourceProperty.SetValue(targetObj, convertedValue);
                        Debug.Log($"成功修改属性 {nodeData.Name} 的值: {nodeData.Value} -> {convertedValue}");
                    }
                    return;
                }

                // 节点是容器（List/Dict）的简单值类型元素，通过容器索引器写回
                if (nodeData.IsContainerEntry)
                {
                    if (targetObj is IList list && nodeData.ContainerKey is int index)
                    {
                        list[index] = convertedValue;
                        Debug.Log($"成功修改列表 [{index}] 的值: {nodeData.Value} -> {convertedValue}");
                        return;
                    }
                    if (targetObj is IDictionary dict && nodeData.ContainerKey != null)
                    {
                        dict[nodeData.ContainerKey] = convertedValue;
                        Debug.Log($"成功修改字典 [{nodeData.ContainerKey}] 的值: {nodeData.Value} -> {convertedValue}");
                        return;
                    }
                    Debug.LogWarning($"无法修改容器元素 {nodeData.Name}");
                    return;
                }

                Debug.LogWarning($"无法修改 {nodeData.Name}，无法找到可写的字段、属性或容器");
            }
            catch (Exception ex)
            {
                Debug.LogError($"修改值时出错: {ex.Message}");
            }
        }

        private object GetTargetObject(NodeData nodeData)
        {
            if (nodeData.Parent == null)
                return null;

            // 父节点的 OriginalObject 才是真正承载这个字段/属性的对象
            return nodeData.Parent.OriginalObject;
        }

        private object ConvertValue(string valueStr, string targetType)
        {
            if (string.IsNullOrEmpty(valueStr))
            {
                if (targetType == "string")
                    return "";
                if (targetType == "bool")
                    return false;
                return null;
            }

            try
            {
                switch (targetType)
                {
                    case "int": return int.Parse(valueStr);
                    case "float": return float.Parse(valueStr);
                    case "double": return double.Parse(valueStr);
                    case "long": return long.Parse(valueStr);
                    case "short": return short.Parse(valueStr);
                    case "byte": return byte.Parse(valueStr);
                    case "char": return valueStr.Length > 0 ? valueStr[0] : '\0';
                    case "bool": return bool.Parse(valueStr);
                    case "uint": return uint.Parse(valueStr);
                    case "ulong": return ulong.Parse(valueStr);
                    case "ushort": return ushort.Parse(valueStr);
                    case "sbyte": return sbyte.Parse(valueStr);
                    case "string": return valueStr;
                    default: return null;
                }
            }
            catch
            {
                return null;
            }
        }   
    }

    private object targetObject;
    private ObjectTreeView treeView;
    private NodeData rootNodeData;
    private string staticPath = "Game.gameplayManager";
    private int nextId = 1;
    private TreeViewState treeViewState;

    [MenuItem("Tools/MyTools/对象树查看器")]
    public static void ShowWindow()
    {
        GetWindow<ObjectTreeViewer>("对象树查看器");
    }

    private void OnEnable()
    {
        if (treeViewState == null)
            treeViewState = new TreeViewState();
    }

    private void OnGUI()
    {
        DrawToolbar();
        DrawTreeView();
    }

    private void DrawToolbar()
    {
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);

        EditorGUILayout.LabelField("成员路径 (格式: 类型名.成员):", EditorStyles.boldLabel);

        staticPath = EditorGUILayout.TextField(staticPath);

        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("获取对象", GUILayout.Height(25)))
        {
            GetObjectByPath(staticPath);
        }

        if (GUILayout.Button("刷新当前对象", GUILayout.Height(25)))
        {
            RefreshTree();
        }

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.HelpBox(
            "示例:\n" +
            "• Game.gameplayManager\n\n" +            
            "按 F2 编辑\n",
            MessageType.Info);

        EditorGUILayout.EndVertical();

        EditorGUILayout.Space(5);
    }

    private void DrawTreeView()
    {
        if (treeView == null)
        {
            EditorGUILayout.HelpBox("请先获取对象", MessageType.Info);
            return;
        }

        var rect = GUILayoutUtility.GetRect(0, 100000, 0, 100000);
        treeView.OnGUI(rect);
    }

    private void GetObjectByPath(string path)
    {
        if (string.IsNullOrEmpty(path))
        {
            Debug.LogWarning("请输入路径");
            return;
        }

        try
        {
            targetObject = ResolvePath(path);
            if (targetObject != null)
            {
                Debug.Log($"成功获取对象: {targetObject.GetType().Name}");
                RefreshTree();
            }
            else
            {
                Debug.LogError($"无法解析路径: {path}");
                treeViewState = new TreeViewState();
                treeView = null;
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"解析路径时出错: {ex.Message}\n{ex.StackTrace}");
            treeView = null;
        }
    }

    private object ResolvePath(string path)
    {
        var members = path.Split('.');
        //if (members.Length < 2)
        //{
        //    return null;
        //}

        var typeName = members[0];
        var type = FindType(typeName);
        if (type == null)
        {
            Debug.LogError($"找不到类型: {typeName}");
            return null;
        }

        Type currentType = type;
        object current = null;

        for (int i = 1; i < members.Length; i++)
        {
            var member = members[i];

            var staticField = GetStaticField(currentType, member);
            if (staticField != null)
            {
                current = staticField.GetValue(current);
                currentType = current?.GetType();
                continue;
            }

            var staticProp = GetStaticProperty(currentType, member);
            if (staticProp != null)
            {
                current = staticProp.GetValue(current);
                currentType = current?.GetType();
                continue;
            }

            var instField = GetInstField(currentType, member);
            if (instField != null)
            {
                current = instField.GetValue(current);
                currentType = current?.GetType();
                continue;
            }

            var instProp = GetInstProperty(currentType, member);
            if (instProp != null && instProp.CanRead)
            {
                current = instProp.GetValue(current);
                currentType = current?.GetType();
                continue;
            }

            Debug.LogError($"找不到成员: {member}");
            return null;
        }

        return current;
    }

    private Type FindType(string typeName)
    {
        var type = Type.GetType(typeName);
        if (type != null) return type;

        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            type = assembly.GetType(typeName);
            if (type != null) return type;

            type = assembly.GetTypes().FirstOrDefault(t => t != null &&
                   (t.Namespace == null || !t.Namespace.StartsWith("Demo")) &&
                   t.Name == typeName);
            if (type != null) return type;
        }
        return null;
    }

    private FieldInfo GetStaticField(Type type, string name)
    {
        while (type != null)
        {
            var field = type.GetField(name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            if (field != null) return field;
            type = type.BaseType;
        }
        return null;
    }

    private PropertyInfo GetStaticProperty(Type type, string name)
    {
        while (type != null)
        {
            var prop = type.GetProperty(name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.FlattenHierarchy);
            if (prop != null) return prop;
            type = type.BaseType;
        }
        return null;
    }

    private FieldInfo GetInstField(Type type, string name)
    {
        if (type == null) return null;
        return type.GetField(name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
    }

    private PropertyInfo GetInstProperty(Type type, string name)
    {
        if (type == null) return null;
        return type.GetProperty(name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
    }

    private void RefreshTree()
    {
        if (targetObject == null)
        {
            treeView = null;
            return;
        }

        nextId = 1;
        var name = targetObject.GetType().Name;
        var root = new NodeData(null, NextTreeId(), true, targetObject.GetHashCode(), name, "", GetCSharpTypeName(targetObject.GetType()));
        root.OriginalObject = targetObject;
        BuildTree(root, targetObject, name, GetCSharpTypeName(targetObject.GetType()), true);

        rootNodeData = root;

        if (treeViewState == null)
            treeViewState = new TreeViewState();

        treeView = new ObjectTreeView(treeViewState, this);
        treeView.Reload();
    }

    private bool BuildTree(NodeData node, object obj, string name, string typeName, bool isContainerItem, FieldInfo sourceField = null, PropertyInfo sourceProperty = null)
    {
        if (nextId > 20000)
        {
            Debug.LogWarning("当前对象数据过多！");
            return false;
        }

        if (obj == null)
        {
            if (isContainerItem)
            {
                node.IsClass = false;
                node.Name = name;
                node.Value = "null";
                node.Type = typeName;
                node.OriginalObject = null;
                node.SourceField = sourceField;
                node.SourceProperty = sourceProperty;
            }
            else
            {
                var id = NextTreeId();
                var data = new NodeData(node, id, false, 0, name, "null", typeName);
                data.SourceField = sourceField;
                data.SourceProperty = sourceProperty;
                node.AddChild(data);
            }
            return true;
        }

        var type = obj.GetType();
        var hashCode = obj.GetHashCode();

        // 字符串和简单值类型（int, float, bool 等）作为叶子节点
        if (type == typeof(string) || IsSimpleValueType(type))
        {
            if (isContainerItem) //列表的元素是简单类型，直接将值显示在根节点上，没必要多构建一层
            {
                node.IsClass = false;
                node.Name = name;
                node.Value = obj.ToString();
                node.Type = typeName;
                node.OriginalObject = obj;
                node.SourceField = sourceField;
                node.SourceProperty = sourceProperty;
            }
            else
            {
                var id = NextTreeId();
                var data = new NodeData(node, id, false, 0, name, obj.ToString(), typeName);
                data.OriginalObject = obj;
                data.SourceField = sourceField;
                data.SourceProperty = sourceProperty;
                node.AddChild(data);
            }

            return true;
        }

        // 结构体（struct）展开其字段，但标记为不可编辑
        if (type.IsValueType)
        {
            NodeData structNode;
            if (!isContainerItem)
            {
                var id = NextTreeId();
                structNode = new NodeData(node, id, true, hashCode, name, "", typeName);
                structNode.OriginalObject = obj;
                structNode.SourceField = sourceField;
                structNode.SourceProperty = sourceProperty;
                node.AddChild(structNode);
            }
            else
            {
                structNode = node;
                structNode.Name = name;
                structNode.Value = "";
                structNode.Type = typeName;
                structNode.OriginalObject = obj;
            }

            var fields = GetAllFields(type);
            foreach (var field in fields)
            {
                if (field.Name.Contains("k__BackingField")) continue;

                try
                {
                    var value = field.GetValue(obj);
                    var success = BuildTree(structNode, value, field.Name, GetCSharpTypeName(field.FieldType), false, sourceField: field);
                    if (!success)
                    {
                        return false;
                    }
                }
                catch (Exception e)
                {
                    var id = NextTreeId();
                    var data = new NodeData(structNode, id, false, 0, field.Name, $"获取失败: {e.Message}", "Error");
                    structNode.AddChild(data);
                }
            }

            var props = GetAllProperties(type);
            foreach (var prop in props)
            {
                if (!IsAutoProperty(prop)) continue;

                try
                {
                    var value = prop.GetValue(obj);
                    var success = BuildTree(structNode, value, prop.Name, GetCSharpTypeName(prop.PropertyType), false, sourceProperty: prop);
                    if (!success)
                    {
                        return false;
                    }
                }
                catch (Exception e)
                {
                    var id = NextTreeId();
                    var data = new NodeData(structNode, id, false, 0, prop.Name, $"获取失败: {e.Message}", "Error");
                    structNode.AddChild(data);
                }
            }
            return true;
        }

        //防止循环引用
        if (node.IsCycle(hashCode))
        {
            var id = NextTreeId();
            var data = new NodeData(node, id, false, 0, name, "循环引用", GetCSharpTypeName(type));
            data.SourceField = sourceField;
            data.SourceProperty = sourceProperty;
            node.AddChild(data);
            return true;
        }

        if (obj is IList list)
        {
            NodeData listNode;
            if (!isContainerItem)
            {
                var id = NextTreeId();
                listNode = new NodeData(node, id, true, hashCode, name, $"Count:{list.Count}", GetCSharpTypeName(type));
                listNode.OriginalObject = obj;
                listNode.SourceField = sourceField;
                listNode.SourceProperty = sourceProperty;
                node.AddChild(listNode);
            }
            else
            {
                listNode = node;    //List<List<>> : 当前列表是另一个列表的元素，直接在当前节点上构建，以避免多一层
                listNode.Name = name;
                listNode.Value = $"Count:{list.Count}";
                listNode.Type = typeName;
                listNode.OriginalObject = obj;
            }

            for (int i = 0; i < list.Count; i++)
            {
                try
                {
                    var value = list[i];
                    var valueHashCode = value?.GetHashCode() ?? 0;
                    var valueTypeName = value != null ? GetCSharpTypeName(value.GetType()) : "null";
                    var child = new NodeData(listNode, NextTreeId(), true, valueHashCode, $"[{i}]", "", valueTypeName);
                    child.OriginalObject = value;
                    child.IsContainerEntry = true;
                    child.ContainerKey = i;
                    listNode.AddChild(child);
                    var success = BuildTree(child, value, $"[{i}]", valueTypeName, true);
                    if (!success)
                    {
                        return false;
                    }
                }
                catch (Exception e)
                {
                    listNode.AddChild(new NodeData(listNode, NextTreeId(), false, 0, $"{name}[{i}]", $"获取失败: {e.Message}", "Error"));
                }
            }
            return true;
        }

        if (obj is IDictionary dict)
        {
            NodeData dicNode;
            if (!isContainerItem)
            {
                var id = NextTreeId();
                dicNode = new NodeData(node, id, true, hashCode, name, $"Count:{dict.Count}", typeName);
                dicNode.OriginalObject = obj;
                dicNode.SourceField = sourceField;
                dicNode.SourceProperty = sourceProperty;
                node.AddChild(dicNode);
            }
            else
            {
                dicNode = node; //List<Dic<>>
                dicNode.Name = name;
                dicNode.Value = $"Count:{dict.Count}";
                dicNode.Type = typeName;
                dicNode.OriginalObject = obj;
            }

            foreach (DictionaryEntry entry in dict)
            {
                var key = entry.Key;
                var value = entry.Value;
                var keyStr = key?.ToString() ?? "null";
                var valueHashCode = value?.GetHashCode() ?? 0;
                var valueTypeName = value != null ? GetCSharpTypeName(value.GetType()) : "null";
                var child = new NodeData(dicNode, NextTreeId(), true, valueHashCode, $"[{keyStr}]", "", valueTypeName);
                child.OriginalObject = value;
                child.IsContainerEntry = true;
                child.ContainerKey = key;
                dicNode.AddChild(child);
                var success = BuildTree(child, value, $"[{keyStr}]", valueTypeName, true);
                if (!success)
                {
                    return false;
                }
            }
            return true;
        }

        //class
        {
            NodeData child;
            if (!isContainerItem)
            {
                var id = NextTreeId();
                child = new NodeData(node, id, true, hashCode, name, "", typeName);
                child.OriginalObject = obj;
                child.SourceField = sourceField;
                child.SourceProperty = sourceProperty;
                node.AddChild(child);
            }
            else
            {
                child = node;
                child.Name = name;
                child.Value = "";
                child.Type = typeName;
                child.OriginalObject = obj;
            }

            var fields = GetAllFields(type);
            foreach (var field in fields)
            {
                if (field.Name.Contains("k__BackingField")) continue;

                try
                {
                    var value = field.GetValue(obj);
                    var success = BuildTree(child, value, field.Name, GetCSharpTypeName(field.FieldType), false, sourceField: field);
                    if (!success)
                    {
                        return false;
                    }
                }
                catch (Exception e)
                {
                    var id = NextTreeId();
                    var data = new NodeData(child, id, false, 0, field.Name, $"获取失败: {e.Message}", "Error");
                    child.AddChild(data);
                }
            }

            var props = GetAllProperties(type);
            foreach (var prop in props)
            {
                if (!IsAutoProperty(prop)) continue;

                try
                {
                    var value = prop.GetValue(obj);
                    var success = BuildTree(child, value, prop.Name, GetCSharpTypeName(prop.PropertyType), false, sourceProperty: prop);
                    if (!success)
                    {
                        return false;
                    }
                }
                catch (Exception e)
                {
                    var id = NextTreeId();
                    var data = new NodeData(child, id, false, 0, prop.Name, $"获取失败: {e.Message}", "Error");
                    child.AddChild(data);
                }
            }
        }

        return true;
    }

    private int NextTreeId()
    {
        return nextId++;
    }

    private bool IsAutoProperty(PropertyInfo property)
    {
        var backingFieldName = $"<{property.Name}>k__BackingField";
        var backingField = property.DeclaringType?.GetField(backingFieldName, BindingFlags.NonPublic | BindingFlags.Instance);
        return backingField != null && backingField.GetCustomAttribute<CompilerGeneratedAttribute>() != null;
    }

    private bool IsSimpleValueType(Type type)
    {
        if (!type.IsValueType) return false;
        if (type.IsPrimitive) return true;
        if (type.IsEnum) return true;
        if (type == typeof(decimal)) return true;
        if (type == typeof(DateTime)) return true;
        if (type == typeof(TimeSpan)) return true;
        if (type == typeof(Guid)) return true;
        if (type == typeof(Vector2) || type == typeof(Vector3) || type == typeof(Vector4)) return true;
        if (type == typeof(Quaternion)) return true;
        if (type == typeof(Color) || type == typeof(Color32)) return true;
        if (type == typeof(Rect)) return true;
        if (type == typeof(Bounds)) return true;
        return false;
    }

    private IEnumerable<FieldInfo> GetAllFields(Type type)
    {
        var seen = new HashSet<string>();
        var current = type;
        while (current != null && current != typeof(object))
        {
            var flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly;
            foreach (var f in current.GetFields(flags))
            {
                if (seen.Add(f.Name))
                {
                    yield return f;
                }
            }
            current = current.BaseType;
        }
    }

    private IEnumerable<PropertyInfo> GetAllProperties(Type type)
    {
        var seen = new HashSet<string>();
        var current = type;
        while (current != null && current != typeof(object))
        {
            var flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly;
            foreach (var p in current.GetProperties(flags))
            {
                // 跳过索引器（有参属性），避免误读
                if (p.GetIndexParameters().Length > 0) continue;
                if (seen.Add(p.Name))
                {
                    yield return p;
                }
            }
            current = current.BaseType;
        }
    }

    private string GetCSharpTypeName(Type type)
    {
        if (type == null) return "null";
        if (type == typeof(int)) return "int";
        if (type == typeof(float)) return "float";
        if (type == typeof(double)) return "double";
        if (type == typeof(long)) return "long";
        if (type == typeof(short)) return "short";
        if (type == typeof(byte)) return "byte";
        if (type == typeof(uint)) return "uint";
        if (type == typeof(ulong)) return "ulong";
        if (type == typeof(ushort)) return "ushort";
        if (type == typeof(sbyte)) return "sbyte";
        if (type == typeof(bool)) return "bool";
        if (type == typeof(char)) return "char";
        if (type == typeof(string)) return "string";
        if (type == typeof(decimal)) return "decimal";
        if (type == typeof(object)) return "object";
        if (type == typeof(void)) return "void";

        // 数组
        if (type.IsArray)
        {
            var rank = type.GetArrayRank();
            var brackets = "[" + new string(',', rank - 1) + "]";
            return GetCSharpTypeName(type.GetElementType()) + brackets;
        }

        // 可空类型 Nullable<T>
        var underlying = Nullable.GetUnderlyingType(type);
        if (underlying != null)
        {
            return GetCSharpTypeName(underlying) + "?";
        }

        // 泛型
        if (type.IsGenericType)
        {
            var backtickIdx = type.Name.IndexOf('`');
            var baseName = backtickIdx >= 0 ? type.Name.Substring(0, backtickIdx) : type.Name;
            var args = string.Join(", ", type.GetGenericArguments().Select(GetCSharpTypeName));
            return $"{baseName}<{args}>";
        }

        return type.Name;
    }
}