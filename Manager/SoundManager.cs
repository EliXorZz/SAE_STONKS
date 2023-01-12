using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using TheGame.Core;
using TheGame.Core.Sound;

namespace TheGame.Manager
{
    public class SoundManager
    {
        private MainGame _game;
        private List<Sound> _sounds;

        public SoundManager(MainGame game)
        {
            _game = game;
            _sounds = new List<Sound>();

            _sounds.Add(new Sound(_game, "SE", 1.8, "sword"));
            _sounds.Add(new Sound(_game, "SE", 0.42, "pas"));
            _sounds.Add(new Sound(_game, "OST", 0, "main"));
            _sounds.Add(new Sound(_game, "SE", 0.2, "click"));
        }

        public void LoadContent(MainGame game)
        {
            foreach (Sound i in _sounds)
                i.LoadSound();
            
        }

        public void PlayEffect(string name, GameTime gametime)
        {
            foreach(Sound i in _sounds)
            {
                if (i.Namesound == name)
                {
                    i.Play(gametime);
                    return;
                }
                    
            }
        }

        public void Stop()
        {
            MediaPlayer.Stop();
        }


    }
}
