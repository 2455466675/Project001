using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game 
{
    public class SceneGrid : MonoBehaviour
    {
        [SerializeField]
        private int row;
        [SerializeField]
        private int col;
        [SerializeField]
        private float w;
        [SerializeField]
        private float h;

        [SerializeField]
        private TileItem tileItem;

        private TileItem[,] tiles;

        [Button("Init")]
        private void Init() 
        {
            int childCount = transform.childCount;
            for (int i = childCount - 1; i >= 0; i--) 
            {                
                DestroyImmediate(transform.GetChild(i).gameObject);
            }

            tiles = new TileItem[row, col];

            int count = row * col;
            for (int i = 0; i < col; i++)
            {
                for (int j = 0; j < row; j++) 
                {
                    TileItem item = Instantiate(tileItem, transform);

                    float x = w * j;
                    float z = h * i;

                    item.transform.localPosition = new Vector3(x, 0, z);
                    item.name = string.Format("TileItem:({0},{1})", j, i);
                    tiles[j, i] = item;
                }
            }            
        }
    }
}