using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TeamWorkGame.Device;
using TeamWorkGame.Def;

namespace TeamWorkGame.Scene
{
    class StageIn : IScene
    {
        private string word;
        private bool isEnd;

        public void Draw(GameTime gameTime, Renderer renderer)
        {
            throw new NotImplementedException();
        }

        public void Initialize(int index = -1)
        {
            if(index <= 0)
            {
                int bigIndex = index / StageDef.SmallIndexMax + 1;
                int smallIndex = index % StageDef.SmallIndexMax + 1;

                word = bigIndex + " - " + smallIndex;
            }
        }

        public bool IsEnd()
        {
            throw new NotImplementedException();
        }

        public NextScene Next()
        {
            throw new NotImplementedException();
        }

        public void ShutDown()
        {
            throw new NotImplementedException();
        }

        public void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}
