using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework
{
    /// <summary>
    /// 
    /// </summary>
    public class GameRoot : MonoBehaviour
    {
        private static GameRoot instance;

        [SerializeField]
        private List<GameNode> nodes;

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
            Game.Update();
        }

        private void FixedUpdate()
        {
            Game.FixedUpdate();
        }

        private void LateUpdate()
        {
            Game.LateUpdate();
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