/////////////////////////////////////////////////
// 木のクラス
// 作成時間：2016年10月12日
// By 長谷川修一
// 最終修正時間：2016年10月20日
// By 氷見悠人
/////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TeamWorkGame.Device;
using TeamWorkGame.Def;
using TeamWorkGame.Utility;

namespace TeamWorkGame.Actor
{
    public class Tree : GameObject
    {
        private Timer timer;
        private bool isToDeath;
        private float scale;
        public Tree(Vector2 pos)
            : base("tree", pos, Vector2.Zero,false, "Tree")
        {

        }

        public override void Initialize()
        {
            base.Initialize();
            timer = new Timer(2.0f);
            isToDeath = false;
            scale = 1.0f;
        }

        public void ToDeath()
        {
            if(!isToDeath)
            {
                isToDeath = true;
            }
        }

        public override void Update(GameTime gameTime)
        {
            if(isToDeath)
            {
                timer.Update();
                if(timer.IsTime())
                {
                    IsDead = true;
                }
                
            }
        }

        /// <summary>
        /// 描画の再定義（Scale追加）　by　氷見悠人　2016/10/20
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="renderer"></param>
        /// <param name="offset"></param>
        public override void Draw(GameTime gameTime, Renderer renderer, Vector2 offset)
        {
            renderer.DrawTexture(name, position + offset, scale, alpha);
        }


        public override void EventHandle(GameObject other)
        {
            if(other is Fire)
            {
                other.IsDead = true;
            }
            name = "fire";
            IsTrigger = true;
            scale = 4.4f;
            ToDeath();
        }

        public float GetScale()
        {
            return scale;
        }
    }
}
