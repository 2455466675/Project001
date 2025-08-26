using UnityEngine;

namespace GameFramework
{
    /// <summary>
    /// 
    /// </summary>
    public class GameRoot : MonoBehaviour
    {
        public static GameRoot Instance { get; private set; }

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
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {            

        }

        private void FixedUpdate()
        {

        }
    }
}
