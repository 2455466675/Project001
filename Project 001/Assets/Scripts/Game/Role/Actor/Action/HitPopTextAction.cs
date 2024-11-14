using UnityEngine;

namespace Game.System
{
    /// <summary>
    /// 
    /// </summary>
	public class HitPopTextAction : BaseAction
    {
        [StringInList(typeof(SystemSetting), "Bones")]
        public string targetBone;

        public string prefabPath;

        public override void Execute(Actor actor)
        {
            Transform bone = actor.GetBone(targetBone);
            if (bone == null)
            {
                return;
            }
            GameObject go = GameCore.ResourceManager.LoadAndInstantiate(prefabPath, bone);
            HitPopText popText = go.GetComponent<HitPopText>();
            int value = Random.Range(1, 99999);
            popText.Show(value.ToString());
        }

        public override void Exit()
        {
            
        }
    }
}

