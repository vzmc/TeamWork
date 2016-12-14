/////////////////////////
//最終更新日　11/30
//更新者　葉梨竜太
/////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using TeamWorkGame.Device;
using TeamWorkGame.Utility;

namespace TeamWorkGame.Scene
{
    class Load : IScene
    {
        private Renderer renderer;
        private Sound sound;
        private bool endFlag;

        private TextureLoader textureLoader;
        private BGMLoader bgmLoader;
        private SELoader seLoader;

        private int totalResouceNum;

        private Timer timer;
        private Timer timer2;

        private string[,] TextureList()
        {
            string path = "./Texture/";

            string[,] list = new string[,]
            {
                {"hero", path},
                {"light_off", path},
                {"fire", path},
                {"tree", path},
                {"ice", path},
                {"iron", path},
                {"title", path},
                {"clear", path},
                {"goal", path},
                {"coal", path},
                {"straw", path},
                {"worldmap", path},
                {"smallmap", path},
                {"frame", path},
                {"forestBG", path},
                {"FireMeter", path},
                {"iceAnime", path},
                {"ironAnime", path},
                {"wood", path},
                {"playerAnime", path},
                {"standAnime", path},
                {"throwAnime", path},
                {"sand", path},
                {"backGround", path},
                {"woodAnime", path},
                {"strawAnime", path},
                {"text", path},
                {"ground1", path},
                {"ground2", path},
                {"ClearWindow", path},
                {"ClearWindow2", path},
                {"selecter", path},
                {"GameStartText", path},
                {"WorldText", path},
                {"StaffText", path},
                {"balloon", path},
                {"Pause", path},
                {"water", path},
                {"smallmap1", path},
                {"smallmap2", path},
                {"smallmap3", path},
                {"smallmap4", path},
                {"smallmap5", path},
                {"smallmap6", path},
                {"lock", path},
                {"Zback", path},
                {"uparrow", path},
                {"downarrow", path},
                {"bomb",path },
                {"JumpEffect", path},
                {"sign", path},
                {"igniter", path},
                {"spark1", path},
                {"spark2", path},
                {"deathAnime", path},
                {"startText", path},
                {"startText2", path},
                {"stageSelect_UI", path},
                {"stageSelect_UI2", path},
            };

            return list;
        }

        private string[,] BGMList()
        {
            string path = "./Sound/BGM/";
            string[,] list = new string[,]
            {
                {"forest1", path},
                {"worldmap1", path},
                {"village1",path},
            };

            return list;
        }

        private string[,] SEList()
        {
            string path = "./Sound/SE/";
            string[,] list = new string[,]
            {
                {"cancel1", path},
                {"decision1", path},
                {"fire1",path},
            };

            return list;
        }

        public Load(GameDevice gameObject)
        {
            renderer = gameObject.GetRenderer();
            sound = gameObject.GetSound();

            textureLoader = new TextureLoader(renderer, TextureList());
            bgmLoader = new BGMLoader(sound, BGMList());
            seLoader = new SELoader(sound, SEList());
            timer = new Timer(2);
            timer2 = new Timer(2);
        }


        public void Initialize(int index)
        {
            endFlag = false;
            textureLoader.Initialize();
            bgmLoader.Initialize();
            seLoader.Initialize();
            totalResouceNum = textureLoader.Count() + bgmLoader.Count() + seLoader.Count();
            timer.Initialize();
            timer2.Initialize();
        }

        public bool IsEnd()
        {
            return endFlag;
        }

        public void LoadContent()
        {
        }

        public void ShutDown()
        {
        }

        public void UnloadContent()
        {
        }

        public void Update(GameTime gameTime)
        {
            if (!textureLoader.IsEnd())
            {
                textureLoader.Update();
            }
            else if (!bgmLoader.IsEnd())
            {
                bgmLoader.Update();
            }
            else if (!seLoader.IsEnd())
            {
                seLoader.Update();
            }

            if(timer.IsTime() && textureLoader.IsEnd() && bgmLoader.IsEnd() && seLoader.IsEnd())
            {
                timer2.Update();
            }
            if (timer2.IsTime())
            {
                endFlag = true;
            }

            timer.Update();
        }

        public void Draw(GameTime gameTime, Renderer renderer)
        {
            //renderer.DrawTexture("load", new Vector2(20, 20));

            //int currentCount = textureLoader.CurrentCount() + bgmLoader.CurrentCount() + seLoader.CurrentCount();
            //if (totalResouceNum != 0)
            //{
            //    renderer.DrawNumber(
            //        "number",
            //        new Vector2(20, 100),
            //        (int)(currentCount / (float)totalResouceNum * 100.0f));
            //}

            //if (textureLoader.IsEnd() && bgmLoader.IsEnd() && seLoader.IsEnd())
            //{
            //    endFlag = true;
            //}
            if (!timer.IsTime())
            {
                renderer.DrawTexture("logo", Vector2.Zero);
            }
            else
            {
                renderer.DrawTexture("teamlogo", Vector2.Zero);
            }
        }

        public NextScene Next()
        {
            NextScene nextScene = new NextScene(SceneType.Title, -1);
            return nextScene;
        }
    }
}
