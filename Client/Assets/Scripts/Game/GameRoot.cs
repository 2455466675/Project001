using Game.GSystem;
using Game.UI;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// 
    /// </summary>
    public class GameRoot : MonoBehaviour
    {
        public UIRoot UIRoot;
        public ActorRoot ActorRoot;
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
