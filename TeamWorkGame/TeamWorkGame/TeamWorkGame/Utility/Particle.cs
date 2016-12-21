///作成日：2016.12.21
///作成者：柏
///作成内容： パーティクル表現用クラス
///最後修正内容：。。
///最後修正者：。。
///最後修正日：。。

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TeamWorkGame.Device;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TeamWorkGame.Utility
{
    class Particle
    {
        private string name;
        private Vector2 position;
        private Vector2 startPosition;
        private float size;
        private int timer;
        private bool isDead;
        private Vector2 velocity;

        public Particle(Vector2 velocity)
        {
            name = "firework";
            position = new Vector2(210, 105);
            startPosition = position;
            isDead = false;
            size = 5.0f;
            timer = 0;
            this.velocity = velocity;
        }

        public void Update()
        {
            timer++;
            position += velocity;
            if (timer % 2 == 0)
            {
                size += 0.1f;
            }

            float distence = (position.X - startPosition.X) * (position.X - startPosition.X) + (position.Y - startPosition.Y) * (position.Y - startPosition.Y);
            if (distence >= 4000)
            {
                isDead = true;
            }

        }

        public void Draw(Renderer renderer)
        {
            if (isDead) return;
            renderer.DrawTexture(name, position, size);
        }

        public bool IsDead
        {
            get { return isDead; }
        }

    }
}
