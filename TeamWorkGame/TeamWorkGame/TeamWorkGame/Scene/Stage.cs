/////////////////////////////////////////////////
// ステージのクラス
// 最終修正時間：2016年10月13日
// By 葉梨竜太
/////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using TeamWorkGame.Device;
using TeamWorkGame.Actor;
using TeamWorkGame.Def;
using TeamWorkGame.Scene;
using TeamWorkGame.Utility;

namespace TeamWorkGame.Scene
{
    class Stage : IScene
    {
        private InputState inputState;
        private bool isEnd;
        private int mapIndex;
        public List<Vector2> herol;

        public Stage(GameDevice gameDevice)
        {
            inputState = gameDevice.GetInputState();
            herol = new List<Vector2>()
            {
                new Vector2(85,153),
                new Vector2(270,310),
                new Vector2(610,340),
                new Vector2(933,460),
                new Vector2(1110,336),
            };
        }
        public void Initialize()
        {
            isEnd = false;
            mapIndex = 0;
        }

        public void Initialize(int index)
        {
            isEnd = false;
            mapIndex = 0;
        }
               
        //描画の開始と終了は全部Game1のDrawに移動した
        public void Draw(GameTime gameTime, Renderer renderer)
        {

            //renderer.Begin();

            renderer.DrawTexture("worldmap", Vector2.Zero);

            renderer.DrawTexture("hero",herol[mapIndex]);

            //renderer.End();
        }

        public void Update(GameTime gametime)
        {
            if (inputState.IsKeyDown(Keys.Right))
            {
                mapIndex++;
                if (mapIndex > 4)
                {
                    mapIndex = 4;
                }
            }
            else if (inputState.IsKeyDown(Keys.Left))
            {
                mapIndex--;
                if(mapIndex < 0)
                {
                    mapIndex = 0;
                }
            }
            if (inputState.IsKeyDown(Keys.Enter))
            {
                isEnd = true;
            }
        }

        public bool IsEnd()
        {
            return isEnd;
        }

        public NextScene Next()
        {
            //NextScene nextScene = new NextScene(SceneType.PlayScene, mapIndex);
            NextScene nextScene = new NextScene(SceneType.SmallStage, mapIndex * 6);
            return nextScene;
        }

        public void ShutDown()
        {

        }

    }
}
