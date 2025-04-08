using UnityEngine;

namespace Game
{
    /// <summary>
    /// 
    /// </summary>
    public class GameWorldObject : MonoBehaviour
    {
        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            //GameWorld.Tick(Time.deltaTime);
        }
    }
}
