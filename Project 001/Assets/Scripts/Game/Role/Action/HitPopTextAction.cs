using UnityEngine;

namespace Game.System
{
    /// <summary>
    /// 
    /// </summary>
	public class HitPopTextAction : ActorBaseAction
    {
        [StringInList(typeof(SystemSetting), "Bones")]
        public string targetBone;

        public string prefabPath;

        public override void Execute(Actor actor, params object[] actionArgs)
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
    }
}

