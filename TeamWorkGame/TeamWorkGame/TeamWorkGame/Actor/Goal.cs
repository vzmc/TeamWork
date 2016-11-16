////////////////////////////////////////////////////////////
//マップのゴール
//作成時間：2016/10/1
//作成者：氷見悠人
////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TeamWorkGame.Device;
using TeamWorkGame.Utility;

namespace TeamWorkGame.Actor
{
    public enum GoalState
    {
        NONE = 0,
        APPEARING,
        SHOW,
    }

    public class Goal : GameObject
    {
        private Camera camera;
        private bool isOnFire;
        public bool IsOnFire
        {
            get
            {
                return isOnFire;
            }
            set
            {
                isOnFire = value;
            }
        }

        private bool isComplete;
        public bool IsComplete
        {
            get
            {
                return isComplete;
            }
            set
            {
                isComplete = value;
            }
        }

        private GoalState state;
        public GoalState State
        {
            get
            {
                return state;
            }
        }

        public Goal(Vector2 pos) : base("goal", pos, Vector2.Zero, true, "Goal")
        {
            state = GoalState.NONE;
            alpha = 0;
        }

        public override void Initialize()
        {
            base.Initialize();
            state = GoalState.NONE;
            alpha = 0;
            isComplete = false;
            isOnFire = false;
        }

        protected override Rectangle InitLocalColRect()
        {
            return new Rectangle(14, 32, 36, 32);
        }

        public void SetCamera(Camera camera)
        {
            this.camera = camera;
        }

        public override void Update(GameTime gameTime)
        {
            //Appear状態に移行
            if (state == GoalState.NONE && isComplete)
            {
                state = GoalState.APPEARING;
                camera.SetLimitView(false);
            }
            //Show状態に移行
            else if(state == GoalState.APPEARING)
            {
                alpha += 0.01f;
                if (alpha >= 1.2f)
                {
                    state = GoalState.SHOW;
                    FuncSwitch.AllAnimetionPause = false;
                    camera.SetLimitView(true);
                    alpha = 1.0f;
                }
            }
        }

        public override void EventHandle(GameObject other)
        {
            //Show状態だけ、動く
            if (state == GoalState.SHOW)
            {
                other.Velocity = velocity;
                other.Position = new Vector2(ColRect.Left + ColRect.Width / 2 - other.Width / 2, ColRect.Top - other.ColRect.Height - other.LocalColRect.Top);
                other.IsOnGround = true;
                if (other is Player)
                {
                    isOnFire = true;
                }
            }
        }

        public override void Draw(GameTime gameTime, Renderer renderer, Vector2 offset, float cameraScale)
        {
            //None状態は描画しない
            if (state == GoalState.APPEARING || state == GoalState.SHOW)
            {
                base.Draw(gameTime, renderer, offset, cameraScale);
            }
        }

        //public void PlayEffect()
        //{

        //}
    }
}
