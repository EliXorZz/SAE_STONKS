using System;

namespace TheGame.Core
{
    public class Goblin : Monster
    {
        public Goblin(MainGame game) : base(game, "goblin", (float) new Random().Next(5, 9) / 10, 30, 15, 3500) {}
    }
}
