//////////////////////////////////////////////////////////////////
// シーンの管理
// 作成時間：2016/9/23
// 作成者：氷見悠人
//////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TeamWorkGame.Device;

namespace TeamWorkGame.Scene
{
    class SceneManager
    {
        //複数のシーン
        private Dictionary<SceneType, IScene> scenes = new Dictionary<SceneType, IScene>();

        //現在のシーン
        private IScene currentScene = null;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SceneManager()
        {
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
            if (nxetScene.stageIndex >= 0)
            {
                currentScene.Initialize(nxetScene.stageIndex);
            }
            else
            {
                currentScene.Initialize();
            }
        }

        public void Change(SceneType name)
        {
            if (currentScene != null)
            {
                currentScene.ShutDown();
            }

            currentScene = scenes[name];
            currentScene.Initialize();
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
                //更新
                currentScene.Update(gameTime);
                //シーン終了か？
                if (currentScene.IsEnd())
                {
                    //次のシーンへ
                    Change(currentScene.Next());
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
        }
    }

}
