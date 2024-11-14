using Game.UI;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Game.System
{
    /// <summary>
    /// 
    /// </summary>
	public class HitPopText : MonoBehaviour
	{
        [SerializeField]
        private ExtendText text;

        [SerializeField]
        private float waveStrength = 8f; // 波动的强度

        [SerializeField]
        private float waveSpeed = 15f; // 波动的速度

        [SerializeField]
        private float characterSpacing = 0.5f; // 字符之间的波动延迟

        [SerializeField]
        private float time = 0.4f;  //动画时间

        [SerializeField]
        private float duration = 0f;  //动画结束后的提留时间

        public void Show(string value)
        {
            text.text = value;
            StartCoroutine(WaveAnimation());
        }

        private IEnumerator WaveAnimation()
        {
            text.ForceMeshUpdate();
            // 获取文本信息
            TMP_TextInfo textInfo = text.textInfo;

            // 确保文本中有字符
            if (textInfo.characterCount == 0)
                yield break;

            // 获取Mesh并提取顶点数组
            Mesh mesh = text.mesh;
            Vector3[] vertices = mesh.vertices;

            // 当前动画时间
            float elapsedTime = 0f;

            // 遍历每个字符
            while (elapsedTime < time)
            {
                elapsedTime += Time.deltaTime;

                // 遍历每个字符
                for (int i = 0; i < textInfo.characterCount; i++)
                {
                    TMP_CharacterInfo charInfo = textInfo.characterInfo[i];

                    // 如果字符不可见，跳过
                    if (!charInfo.isVisible) continue;

                    // 检查当前字符是否为数字
                    char currentChar = textInfo.characterInfo[i].character;
                    if (!char.IsDigit(currentChar))
                    {
                        // 如果不是数字字符，跳过这个字符的波动处理
                        continue;
                    }

                    // 每个字符有四个顶点 (矩形)
                    int vertexIndex = charInfo.vertexIndex;

                    // 获取字符四个顶点的当前坐标
                    Vector3[] charVertices = new Vector3[4];
                    charVertices[0] = vertices[vertexIndex + 0]; // Bottom Left
                    charVertices[1] = vertices[vertexIndex + 1]; // Top Left
                    charVertices[2] = vertices[vertexIndex + 2]; // Top Right
                    charVertices[3] = vertices[vertexIndex + 3]; // Bottom Right

                    // 计算波动的偏移 (基于字符的索引与时间)
                    float waveOffset = Mathf.Sin(elapsedTime * waveSpeed + i * characterSpacing) * waveStrength;

                    // 应用波动效果到每个顶点的Y坐标
                    charVertices[0].y += waveOffset;
                    charVertices[1].y += waveOffset;
                    charVertices[2].y += waveOffset;
                    charVertices[3].y += waveOffset;

                    // 将计算后的顶点更新回去
                    vertices[vertexIndex + 0] = charVertices[0];
                    vertices[vertexIndex + 1] = charVertices[1];
                    vertices[vertexIndex + 2] = charVertices[2];
                    vertices[vertexIndex + 3] = charVertices[3];
                }

                // 将更改后的顶点信息更新到Mesh中
                mesh.vertices = vertices;
                text.canvasRenderer.SetMesh(mesh); // 更新渲染器
  
                yield return null;
            }

            yield return new WaitForSeconds(duration);

            Destroy(gameObject);
        }
    }
}

