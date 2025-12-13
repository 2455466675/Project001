using Cysharp.Threading.Tasks;
using LITJson;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework.Core
{
    [GameModule]
    public class ActionManager : IGameModule_AsyncInit, IUpdate
    {
        private Dictionary<string, string> map;
        private Dictionary<string, ActionItem> pool;

        private List<ActionPlayer> newPlayers;
        private List<ActionPlayer> oldPlayers;
        private List<ActionPlayer> players;

        public async UniTask Init()
        {
            pool = new Dictionary<string, ActionItem>();
            newPlayers = new List<ActionPlayer>();
            oldPlayers = new List<ActionPlayer>();
            players = new List<ActionPlayer>();

            await LoadMap();
        }

        public ActionHandle Play(string name, ActionData actionData)
        {
            ActionPlayer player = CreatePlayer(name);
            if (player == null)
            {
                MDebug.Error("play fail : ", name);
                return null;
            }

            player.Reset(actionData);
            player.Start();

            if (player.IsProcessing)
            {
                newPlayers.Add(player);
            }

            return player.CreateHandle();
        }

        public void Update()
        {
            if (newPlayers.Count > 0)
            {
                players.AddRange(newPlayers);
                newPlayers.Clear();
            }

            if (players.Count > 0)
            {
                for (int i = 0; i < players.Count; i++)
                {
                    ActionPlayer player = players[i];
                    if (!player.IsProcessing)
                    {
                        oldPlayers.Add(player);
                    }
                    else
                    {
                        player.Update(Game.DeltaTime);
                        if (player.IsCompleted)
                        {
                            oldPlayers.Add(player);
                        }
                    }
                }
            }

            if (oldPlayers.Count > 0)
            {
                for (int i = 0; i < oldPlayers.Count; i++)
                {
                    players.Remove(oldPlayers[i]);
                }
                oldPlayers.Clear();
            }
        }

        private ActionPlayer CreatePlayer(string name)
        {
            ActionItem item;
            if (pool.ContainsKey(name))
            {
                item = pool[name];
            }
            else
            {
                if (!map.ContainsKey(name))
                {
                    return null;
                }

                string path = map[name];
                if (string.IsNullOrEmpty(path))
                {
                    return null;
                }

                item = Game.GetModule<AssetsManager>().LoadAsset<ActionItem>(path);
                pool.Add(name, item);
            }

            return new ActionPlayer(item.commands); ;
        }

        private async UniTask LoadMap()
        {
            map = new Dictionary<string, string>();

            var path = "Assets/Bundles/Common/ActionsMap.json";
            var asset = await Game.GetModule<AssetsManager>().LoadAssetAsync<TextAsset>(path);

            if (asset != null)
            {
                var json = asset.text;
                var wrapper = JsonMapper.ToObject<ActionItemObjectListWrapper>(json);

                foreach (var item in wrapper.items)
                {
                    map[item.key] = item.value;
                }
            }
        }
    }
}
