using Game.UI;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game 
{
    public class SceneGridView : View
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

        [SerializeField]
        private TileItem[] items;

        public TileItem GetTileItem(int x, int y) 
        {
            int index = (x * row) + y;
            if (index < 0 || index >= items.Length)
            {
                return null;
            }
            return items[index];
        }

        [Button("Init")]
        private void Init() 
        {
#if UNITY_EDITOR            
            int childCount = transform.childCount;
            for (int i = childCount - 1; i >= 0; i--) 
            {                
                DestroyImmediate(transform.GetChild(i).gameObject);
            }

            items = new TileItem[col * row];

            for (int i = 0; i < col; i++)
            {
                for (int j = 0; j < row; j++)
                {
                    TileItem item = Instantiate(tileItem, transform);

                    float x = w * i;
                    float z = h * j;

                    item.transform.localPosition = new Vector3(x, 0, z);
                    item.SetIndex(i, j);
                    items[(i * row) + j] = item;
                }
            }
#endif
        }
    }
}