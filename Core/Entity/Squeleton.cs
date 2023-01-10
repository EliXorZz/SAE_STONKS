using System;

namespace TheGame.Core
{
    public class Squeleton : Monster
    {
        public Squeleton(MainGame game) : base(game, "squelette", (float) new Random().Next(2, 6) / 10, 50, 25, 5000) {}
    }
}
