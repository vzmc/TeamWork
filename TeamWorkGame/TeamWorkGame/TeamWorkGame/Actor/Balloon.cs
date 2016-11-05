using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TeamWorkGame.Device;
using TeamWorkGame.Def;
using TeamWorkGame.Utility;
using Microsoft.Xna.Framework.Graphics;

namespace TeamWorkGame.Actor
{

    public class Balloon : GameObject
    {
        private Timer timer;
        private int flyLevel;
        private Vector2 startPosition;
        private bool playerIsOn;
        //private Animation animation;
        //private AnimationPlayer animationPlayer;
        //private bool IsAnimation = false;

        public Balloon(Vector2 pos, Vector2 velo)
            : base("balloon", pos, velo, false, "balloon")
        {
            //animationPlayer = new AnimationPlayer();

            startPosition = pos;
        }

        public override void Initialize()
        {
            base.Initialize();

            flyLevel = 0;
            playerIsOn = false;
        }

        public override void Update(GameTime gameTime)
        {
            AliveUpdate();
            //animationPlayer.PlayAnimation(animation);

            if (!playerIsOn) {
                if (PositionY >= startPosition.Y) { return; }
                PositionY++;
            }
        }

        public bool IsPlayerOn {
            get { return playerIsOn; }
            set { playerIsOn = value; }
        }


        public override void AliveEvent(GameObject other)
        {
            if (other is Player)
            {
                if (((Player)other).IsOnBalloon)
                {
                    other.Position = position;
                    if (startPosition.Y - PositionY == Height * flyLevel) { return; }
                    int velo = (int)(startPosition.Y - PositionY - Height * flyLevel);
                    if (velo > 0)
                    {
                        PositionY++;
                    }
                    else {
                        PositionY--;
                    }
                }
            }
        }

        /// <summary>
        /// 描画の再定義（透明値を追加）　By　氷見悠人　2016/10/20
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="renderer"></param>
        /// <param name="offset"></param>
        public override void Draw(GameTime gameTime, Renderer renderer, Vector2 offset)
        {
            renderer.DrawTexture(name, position + offset, alpha);
            //if (IsAnimation)
            //{
            //    animationPlayer.Draw(gameTime, renderer, position + offset, SpriteEffects.None);
            //    IsAnimation = animationPlayer.Reset(isShow);
            //}
        }

        public override void EventHandle(GameObject other)
        {
            //火の数によって行動する
            if (other is Player)
            {
                if (!playerIsOn) {;
                    ((Player)other).IsOnBalloon = true;
                    ((Player)other).IsOnGround = true;
                    playerIsOn = true;
                }

                flyLevel = ((Player)other).FireNum;
                AliveEvent(other);
                //IsAnimation = true;
            }
        }
    }

}
