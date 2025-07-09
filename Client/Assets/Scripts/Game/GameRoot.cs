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
        public UIRoot UIRoot => uiRoot; 
        public ActorRoot ActorRoot => actorRoot;
        public MainCamera MainCamera => mainCamera;

        [SerializeField]
        private UIRoot uiRoot;
        [SerializeField]
        private ActorRoot actorRoot;
        [SerializeField]
        private MainCamera mainCamera;

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
