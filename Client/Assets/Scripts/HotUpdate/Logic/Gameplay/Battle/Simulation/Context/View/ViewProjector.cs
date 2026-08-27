using Cysharp.Threading.Tasks;
using System.Collections.Generic;

namespace GameFramework.Logic
{
    public interface IViewProjector
    {
        void NextBatch();
        void Record(BattleViewCommand command);
        UniTask Flush();
    }

    public class ViewProjector : IViewProjector
    {
        private readonly List<BattleViewCommand> commands;

        private int currentBatch;

        public ViewProjector()
        {
            commands = new List<BattleViewCommand>();
            currentBatch = 0;
        }

        public void NextBatch()
        {
            currentBatch++;
        }

        public void Record(BattleViewCommand command)
        {
            command.Batch = currentBatch;
            commands.Add(command);
        }

        public async UniTask Flush()
        {
            while (commands.Count > 0)
            {
                int batch = commands[0].Batch;
                var list = commands.FindAll(c => c.Batch == batch);

                var group = new List<UniTask>(list.Count);
                for (int i = 0; i < list.Count; i++)
                {
                    group.Add(list[i].Play());
                }

                await UniTask.WhenAll(group);

                commands.RemoveAll(c => c.Batch == batch);
            }

            //commands.Clear();
        }
    }
}
