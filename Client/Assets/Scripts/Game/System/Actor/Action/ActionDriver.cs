using System.Collections.Generic;
using UnityEngine;

namespace Game.GSystem
{
    /// <summary>
    /// 
    /// </summary>
    public class ActionDriver : MonoBehaviour
    {
        [SerializeField]
        private ActionSource[] sources;

        private List<ActionPlayer> newPlayers;
        private List<ActionPlayer> oldPlayers;
        private List<ActionPlayer> players;

        private void Awake()
        {
            newPlayers = new List<ActionPlayer>();
            oldPlayers = new List<ActionPlayer>();
            players = new List<ActionPlayer>();
        }

        public ActionHandle PlayAction(int hash, Actor actor, object userData)
        {        
            ActionPlayer player = GetActionPlayer(hash);
            if (player == null) 
            {
                return null;
            }

            player.Start(actor, userData);

            if (player.IsProcessing)
            {
                newPlayers.Add(player);
            }

            return player.CreateHandle();
        }

        private ActionPlayer GetActionPlayer(int hash) 
        {
            if (sources == null || sources.Length == 0)
            {
                return null;
            }

            for (int i = 0; i < sources.Length; i++)
            {
                ActionSource source = sources[i];
                if (source != null) 
                {
                    ActionPlayer player = source.CreatePlayer(hash);
                    if (player != null)
                    {
                        return player;
                    }
                }
            }

            return null;
        }

        private void Update()
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
                        player.Update(Time.deltaTime);
                        if (player.IsCompleted)
                        {
                            oldPlayers.Add(player);
                        }
                    }
                }
            }

            if (oldPlayers.Count > 0) 
            {
                for(int i = 0; i < oldPlayers.Count; i++) 
                {
                    players.Remove(oldPlayers[i]);
                }
                oldPlayers.Clear();
            }
        }

        private void OnDestroy()
        {
            if (players.Count > 0)
            {
                for (int i = 0; i < players.Count; i++)
                {
                    ActionPlayer player = players[i];
                    player.Complete();
                }
            }
            players.Clear();
            newPlayers.Clear();
            oldPlayers.Clear();
        }
    }
}
