using UnityEngine;

namespace Game
{
    /// <summary>
    /// 
    /// </summary>
    public class GameRoot : MonoBehaviour
    {
        private void Start()
        {
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
    }
}
