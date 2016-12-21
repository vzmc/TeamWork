﻿//////////////////////////////////////////////////////////////////
// シーンの管理
// 作成時間：2016/9/23
// 作成者：氷見悠人
// 修正時間：2016/12/21　　Scene終了のFadeInOut
//////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TeamWorkGame.Device;
using TeamWorkGame.Utility;

namespace TeamWorkGame.Scene
{
    class SceneManager
    {
        //複数のシーン
        private Dictionary<SceneType, IScene> scenes = new Dictionary<SceneType, IScene>();

        //現在のシーン
        private IScene currentScene = null;

        private float alpha;
        private float fadeTime;

        private void DoFadeEffect(bool isFadeOut)
        {
            if (isFadeOut)
            {
                alpha += 1 / (60 * fadeTime);
                if(alpha > 1)
                {
                    alpha = 1;
                }
            }
            else
            {
                alpha -= 1 / (60 * fadeTime);
                if(alpha < 0)
                {
                    alpha = 0;
                }
            }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SceneManager()
        {
            fadeTime = 0.2f;
            alpha = 0.0f;
        }

        public void Add(SceneType name, IScene scene)
        {
            if (scenes.ContainsKey(name))
            {
                //すでに同じ名前でシーンが追加されてれば終了
                return;
            }
            else
            {
                scenes.Add(name, scene);
            }
        }

        public void Change(NextScene nxetScene)
        {
            if (currentScene != null)
            {
                currentScene.ShutDown();
            }

            currentScene = scenes[nxetScene.sceneType];

            currentScene.Initialize(nxetScene.stageIndex);
        }

        public void Update(GameTime gameTime)
        {
            //シーンが全くなければ終了
            if (currentScene == null)
            {
                return;
            }
            else
            {
                //シーン終了か？
                if (currentScene.IsEnd())
                {
                    if (alpha >= 1)
                    {
                        //次のシーンへ
                        Change(currentScene.Next());
                    }
                    else
                    {
                        DoFadeEffect(true);
                    }
                }
                else
                {
                    if (alpha > 0)
                    {
                        DoFadeEffect(false);
                    }
                    //更新
                    currentScene.Update(gameTime);
                }
            }
        }

        public void Draw(GameTime gameTime, Renderer renderer)
        {
            if (currentScene == null)
            {
                return;
            }
            else
            {
                currentScene.Draw(gameTime, renderer);
            }

            if(alpha > 0)
            {
                renderer.DrawTexture("fadein", Vector2.Zero, alpha);
            }
        }
    }
}
