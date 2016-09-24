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
        private Dictionary<Scene, IScene> scenes = new Dictionary<Scene, IScene>();

        //現在のシーン
        private IScene currentScene = null;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SceneManager()
        {
        }

        public void Add(Scene name, IScene scene)
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

        public void Change(Scene name)
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

        public void Draw(Renderer renderer)
        {
            if (currentScene == null)
            {
                return;
            }
            else
            {
                currentScene.Draw(renderer);
            }
        }
    }

}
