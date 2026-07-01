using System.Collections.Generic;
using GameFramework.Logic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace GameFramework.EditorTools
{
    // 阵形编辑器：在场景中摆放/勾选点位后，采集为 BattleFormationCfg 资产；重复 id 覆盖、新 id 追加
    [CustomEditor(typeof(BattleFormationAuthoring))]
    public class BattleFormationAuthoringEditor : Editor
    {
        private const string CfgDir = "Assets/Bundles/Common";
        private const string CfgPath = "Assets/Bundles/Common/BattleFormationCfg.asset";

        private SerializedProperty formationIDProp;
        private SerializedProperty playerRootProp;
        private SerializedProperty enemyRootProp;

        private void OnEnable()
        {
            formationIDProp = serializedObject.FindProperty("formationID");
            playerRootProp = serializedObject.FindProperty("playerRoot");
            enemyRootProp = serializedObject.FindProperty("enemyRoot");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(formationIDProp, new GUIContent("阵形ID"));
            EditorGUILayout.PropertyField(playerRootProp, new GUIContent("玩家根节点"));
            EditorGUILayout.PropertyField(enemyRootProp, new GUIContent("敌人根节点"));
            serializedObject.ApplyModifiedProperties();

            BattleFormationAuthoring authoring = (BattleFormationAuthoring)target;

            EditorGUILayout.Space(8);
            DrawGroup("玩家点位", authoring.PlayerRoot, 1);
            EditorGUILayout.Space(8);
            DrawGroup("敌人点位", authoring.EnemyRoot, 2);

            EditorGUILayout.Space(12);
            using (new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button("保存到配置", GUILayout.Height(30)))
                {
                    SaveToConfig(authoring);
                }

                if (GUILayout.Button("从配置加载", GUILayout.Height(30)))
                {
                    LoadFromConfig(authoring);
                }
            }
        }

        private void DrawGroup(string title, Transform root, int camp)
        {
            EditorGUILayout.LabelField(title, EditorStyles.boldLabel);
            if (root == null)
            {
                EditorGUILayout.HelpBox("请先指定该阵营的根节点", MessageType.Info);
                return;
            }

            for (int i = 0; i < root.childCount; i++)
            {
                Transform child = root.GetChild(i);
                BattleFormationPoint point = child.GetComponent<BattleFormationPoint>();
                using (new EditorGUILayout.HorizontalScope())
                {
                    EditorGUILayout.LabelField($"[{i}] {child.name}", GUILayout.Width(150));

                    bool valid = point != null ? point.Valid : true;
                    EditorGUI.BeginChangeCheck();
                    bool newValid = EditorGUILayout.ToggleLeft("有效", valid, GUILayout.Width(60));
                    if (EditorGUI.EndChangeCheck())
                    {
                        // 首次勾选时补上标记组件，使有效性可随场景持久化
                        if (point == null)
                        {
                            point = Undo.AddComponent<BattleFormationPoint>(child.gameObject);
                        }

                        Undo.RecordObject(point, "修改点位有效性");
                        point.Valid = newValid;
                        EditorUtility.SetDirty(point);
                    }

                    if (GUILayout.Button("选中", GUILayout.Width(50)))
                    {
                        Selection.activeGameObject = child.gameObject;
                    }
                }
            }

            if (GUILayout.Button($"添加{title}"))
            {
                AddPoint(root);
            }
        }

        private void AddPoint(Transform root)
        {
            GameObject go = new GameObject($"Point_{root.childCount}");
            Undo.RegisterCreatedObjectUndo(go, "添加阵形点位");
            go.transform.SetParent(root, false);
            go.transform.localPosition = Vector3.zero;
            go.AddComponent<BattleFormationPoint>();
            Selection.activeGameObject = go;
            MarkSceneDirty(root);
        }

        private void SaveToConfig(BattleFormationAuthoring authoring)
        {
            if (authoring.PlayerRoot == null || authoring.EnemyRoot == null)
            {
                EditorUtility.DisplayDialog("提示", "请先指定玩家根节点与敌人根节点", "确定");
                return;
            }

            BattleFormationCfg cfg = LoadOrCreateConfig();

            BattleFormationCfg.FormationData data = cfg.GetFormation(authoring.FormationID);
            bool isNew = data == null;
            if (isNew)
            {
                data = new BattleFormationCfg.FormationData();
                data.formationID = authoring.FormationID;
            }

            data.playerSites = CollectSites(authoring.PlayerRoot);
            data.enemySites = CollectSites(authoring.EnemyRoot);

            // 重复 id 直接就地覆盖，新的 id 追加到末尾
            if (isNew)
            {
                cfg.Formations.Add(data);
            }
            cfg.Formations.Sort((a, b) => a.formationID.CompareTo(b.formationID));
            EditorUtility.SetDirty(cfg);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log($"[阵形] 已保存 id={authoring.FormationID}（玩家 {data.playerSites.Count} 点 / 敌人 {data.enemySites.Count} 点）到 {CfgPath}");
        }

        private void LoadFromConfig(BattleFormationAuthoring authoring)
        {
            if (authoring.PlayerRoot == null || authoring.EnemyRoot == null)
            {
                EditorUtility.DisplayDialog("提示", "请先指定玩家根节点与敌人根节点", "确定");
                return;
            }

            BattleFormationCfg cfg = AssetDatabase.LoadAssetAtPath<BattleFormationCfg>(CfgPath);
            if (cfg == null)
            {
                EditorUtility.DisplayDialog("提示", $"未找到配置资产：{CfgPath}", "确定");
                return;
            }

            BattleFormationCfg.FormationData data = cfg.GetFormation(authoring.FormationID);
            if (data == null)
            {
                EditorUtility.DisplayDialog("提示", $"配置中不存在 id={authoring.FormationID} 的阵形", "确定");
                return;
            }

            ApplySites(authoring.PlayerRoot, data.playerSites);
            ApplySites(authoring.EnemyRoot, data.enemySites);
            MarkSceneDirty(authoring.transform);
            Debug.Log($"[阵形] 已将 id={authoring.FormationID} 的配置加载回场景");
        }

        private List<BattleFormationCfg.SiteData> CollectSites(Transform root)
        {
            List<BattleFormationCfg.SiteData> list = new List<BattleFormationCfg.SiteData>();
            for (int i = 0; i < root.childCount; i++)
            {
                Transform child = root.GetChild(i);
                BattleFormationPoint point = child.GetComponent<BattleFormationPoint>();
                BattleFormationCfg.SiteData site = new BattleFormationCfg.SiteData();
                site.index = i;
                site.valid = point != null ? point.Valid : true;
                // 存世界坐标，运行时会直接赋给表现节点的 position
                site.position = child.position;
                list.Add(site);
            }

            return list;
        }

        private void ApplySites(Transform root, List<BattleFormationCfg.SiteData> sites)
        {
            // 先删多余、再补不足，使场景点位数量与配置对齐
            for (int i = root.childCount - 1; i >= sites.Count; i--)
            {
                Undo.DestroyObjectImmediate(root.GetChild(i).gameObject);
            }

            while (root.childCount < sites.Count)
            {
                GameObject go = new GameObject();
                Undo.RegisterCreatedObjectUndo(go, "加载阵形点位");
                go.transform.SetParent(root, false);
                go.AddComponent<BattleFormationPoint>();
            }

            for (int i = 0; i < sites.Count; i++)
            {
                Transform child = root.GetChild(i);
                child.name = $"Point_{i}";
                Undo.RecordObject(child, "加载阵形点位");
                child.position = sites[i].position;

                BattleFormationPoint point = child.GetComponent<BattleFormationPoint>();
                if (point == null)
                {
                    point = Undo.AddComponent<BattleFormationPoint>(child.gameObject);
                }

                Undo.RecordObject(point, "加载阵形点位");
                point.Valid = sites[i].valid;
                EditorUtility.SetDirty(point);
            }
        }

        private BattleFormationCfg LoadOrCreateConfig()
        {
            BattleFormationCfg cfg = AssetDatabase.LoadAssetAtPath<BattleFormationCfg>(CfgPath);
            if (cfg != null)
            {
                return cfg;
            }

            // Common 目录通常已存在，这里兜底创建，避免首次使用因缺目录而失败
            if (!AssetDatabase.IsValidFolder(CfgDir))
            {
                if (!AssetDatabase.IsValidFolder("Assets/Bundles"))
                {
                    AssetDatabase.CreateFolder("Assets", "Bundles");
                }

                AssetDatabase.CreateFolder("Assets/Bundles", "Common");
            }

            cfg = ScriptableObject.CreateInstance<BattleFormationCfg>();
            AssetDatabase.CreateAsset(cfg, CfgPath);
            AssetDatabase.SaveAssets();
            return cfg;
        }

        private void MarkSceneDirty(Transform t)
        {
            if (!Application.isPlaying)
            {
                EditorSceneManager.MarkSceneDirty(t.gameObject.scene);
            }
        }
    }
}
