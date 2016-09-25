/////////////////////////////////////////////////
// 火のクラス
// 作成時間：2016年9月25日
// By 氷見悠人
// 最終修正時間：2016年9月25日
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
    class Fire : Character
    {
        private Map map;
        private float gForce;
        private bool isOnGround;                //地上にいるか？

        public Fire(Vector2 position, Vector2 velocity) : base("fire", 64, 64)
        {
            Initialize(position, velocity);
        }

        public override void Initialize(Vector2 position, Vector2 velocity)
        {
            this.position = position;
            this.velocity = velocity;
            gForce = Parameter.GForce;
            isOnGround = false;
            map = MapManager.GetNowMapData();
        }

        public override void Update(GameTime gameTime)
        {
            velocity.Y += gForce;
            Method.MapObstacleCheck(ref position, width, height, ref velocity, ref isOnGround, map, new int[] { 0, 1, 2 });
            if(isOnGround)
            {
                velocity = Vector2.Zero;
            }
            position += velocity;
        }

        /// <summary>
        /// 描画
        /// </summary>
        /// <param name="renderer"></param>
        public override void Draw(Renderer renderer, Vector2 offset)
        {
            renderer.DrawTexture(name, position + offset);
        }
    }
}
