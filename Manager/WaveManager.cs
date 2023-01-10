using System;
using System.Diagnostics;
using TheGame.Core;

namespace TheGame.Manager
{
    public class WaveManager
    {
        private Random _random;

        private MainGame _game;
        private int _round;

        public WaveManager(MainGame game)
        {
            _random = new Random();

            _game = game;
            _round = 0;
        }

        private void SpawnMonstersWave(int waveType, int number)
        {
            for (int i = 0; i < number; i++)
            {
                if (waveType == 1)
                {
                    Goblin goblin = new Goblin(_game);
                    _game.MonsterManager.CreateMonster(goblin);
                }

                if (waveType == 2)
                {
                    Squeleton squeleton = new Squeleton(_game);
                    _game.MonsterManager.CreateMonster(squeleton);
                }
            }

            if (waveType == 3)
            {
                int nGolbin = _random.Next(1, number + 1);
                int nSqueleton = number - nGolbin;

                for (int i = 0; i < nGolbin; i++)
                {
                    Goblin goblin = new Goblin(_game);
                    _game.MonsterManager.CreateMonster(goblin);
                }

                for (int i = 0; i < nSqueleton; i++)
                {
                    Squeleton squeleton = new Squeleton(_game);
                    _game.MonsterManager.CreateMonster(squeleton);
                }
            }
        }

        public void Update()
        {
            if (_game.MonsterManager.Monsters.Count == 0)
            {
                int waveType = _random.Next(1, 3 + 1);
                int number = _random.Next(1, 6);

                SpawnMonstersWave(waveType, number);

                _round += 1;
            }
        }
    }
}
