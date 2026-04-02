using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework.Core
{
    /// <summary>
    /// 
    /// </summary>
    public class GameRoot : MonoBehaviour
    {
        private static GameRoot instance;

        [SerializeField]
        private List<GameNode> nodes;

        internal static async UniTask LoadGameRoot()
        {
            await Game.Resources.LoadAndInstantiateAsync("Assets/Bundles/Common/GameRoot", null);
        }

        public static T GetNode<T>() where T : GameNode
        {
            if (instance == null)
            {
                return default;
            }
            return instance.GetNodeInner<T>();
        }

        private void Awake()
        {
            if (instance != null) 
            {
                return;
            }

            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {       
            Game.Update(Time.deltaTime);
        }

        private void FixedUpdate()
        {
            Game.FixedUpdate(Time.fixedDeltaTime);
        }

        private void LateUpdate()
        {
            Game.LateUpdate(Time.deltaTime);
        }

        private T GetNodeInner<T>() where T : GameNode
        {
            if (nodes == null || nodes.Count == 0) 
            {
                return default;
            }

            Type type = typeof(T);
            foreach (var node in nodes)
            {
                if (node != null && node.GetType() == type) 
                {
                    return node as T;
                }
            }
            return default;
        }
    }
}