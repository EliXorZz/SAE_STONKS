using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheGame.Manager;

namespace TheGame.Core.Sound
{
    internal class Sound
    {
        private MainGame _game;

        private double _duree;
        private string _namesound;
        private double _cooldown;
        private string _type;
        private SoundEffect _sound;
        private Song _ost;

        public Sound(MainGame game, string type ,double duree, string namesound)
        {
            Game = game;
            Type = type;
            Duree = duree;
            Namesound = namesound;
            Cooldown = 0;
        }

        public MainGame Game
        {
            get
            {
                return this._game;
            }

            set
            {
                this._game = value;
            }
        }

        public double Duree
        {
            get
            {
                return this._duree;
            }

            set
            {
                this._duree = value;
            }
        }

        public string Namesound
        {
            get
            {
                return this._namesound;
            }

            set
            {
                this._namesound = value;
            }
        }

        public double Cooldown
        {
            get
            {
                return this._cooldown;
            }

            set
            {
                this._cooldown = value;
            }
        }

        public SoundEffect Sound1
        {
            get
            {
                return this._sound;
            }

            set
            {
                this._sound = value;
            }
        }

        public string Type
        {
            get
            {
                return this._type;
            }

            set
            {
                this._type = value;
            }
        }

        internal Song Ost
        {
            get
            {
                return this._ost;
            }

            set
            {
                this._ost = value;
            }
        }

        public void LoadSound()
        {
            if (Type == "SE")
                Sound1 = Game.Content.Load<SoundEffect>("Sound/"+Namesound);
            else
                Ost = Game.Content.Load<Song>("Sound/"+Namesound);
        }

        public void Play(GameTime gametime)
        {
            if (Type == "SE")
            {
            float elipt = (float)gametime.ElapsedGameTime.TotalSeconds;

            if (Cooldown >= Duree)
            {
                Cooldown = 0;
                Sound1.Play();
            }

            Cooldown += elipt;
            } else
            {
                if(MediaPlayer.State != MediaState.Playing)
                    MediaPlayer.Play(Ost);
                MediaPlayer.Volume = (float)0.3;
                MediaPlayer.IsRepeating = true;
            }
        }

        

        


        
    }

    
}
