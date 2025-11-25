using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework.UI
{
    public class BattleTileLayout : MonoBehaviour
    {
        [SerializeField]
        private Vector2 cellSize = new Vector2(1f, 1f);
        [SerializeField]
        private Vector2 spacing = new Vector2(0f, 0f);
        [SerializeField]
        private int rowCount;
        [SerializeField]
        private int colCount;
        [SerializeField]
        private Transform content;

        public int RowCount => rowCount;
        public int ColCount => colCount;

        [Button("Layout")]
        private void Layout()
        {
            if (content == null)
            {
                return;
            }

            if (rowCount <= 0 || colCount <= 0)
            {
                Debug.LogWarning("Row count and column count must be greater than 0");
                return;
            }

            if (content.childCount == 0)
            {
                Debug.LogWarning("No active children to layout");
                return;
            }

            float startX = 0;
            float startZ = 0;

            float totalCellWidth = cellSize.x + spacing.x;
            float totalCellHeight = cellSize.y + spacing.y;

            for (int i = 0; i < content.childCount; i++)
            {
                Transform child = content.GetChild(i);

                int row = i / colCount;
                int col = i % colCount;
                if (row >= rowCount)
                {
                    child.gameObject.SetActive(false);
                }
                else
                {
                    child.gameObject.SetActive(true);

                    float posX = startX + col * totalCellWidth;
                    float posZ = startZ + row * totalCellHeight;
                    child.localPosition = new Vector3(posX, child.localPosition.y, posZ);
                }
            }
        }
    }
}
