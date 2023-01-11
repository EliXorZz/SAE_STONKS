using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna;
using Microsoft.Xna.Framework.Audio;

namespace TheGame.Manager
{
    public class SoundManager
    {

        private MainGame _game;

        SoundEffect _swordsound;
        SoundEffect _walksound;

        public void Initial(MainGame game)
        {
            //_swordsound = game.Content.Load<SoundEffect>("sword");
        }

        public void PlayEffectSword()
        {
            _swordsound.Play();
        }
        
    }
}
