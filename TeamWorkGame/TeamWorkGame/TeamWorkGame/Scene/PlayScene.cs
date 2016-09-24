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
    class PlayScene : IScene
    {
        private GameDevice gameDevice;
        private bool isEnd;
        private int mapIndex;
        private Player player;
        private Camera camera;
        private Map map;

        public PlayScene(GameDevice gameDevice, int mapIndex = 0)
        {
            this.gameDevice = gameDevice;
            this.mapIndex = mapIndex;
            isEnd = false;
        }
        public void Initialize()
        {
            isEnd = false;
            MapManager.SetNowMap(mapIndex);
            map = MapManager.GetNowMapData();
            camera = gameDevice.GetCamera();
            camera.UpDateMap();
            player = new Player(gameDevice.GetInputState(), camera, new Vector2(100, 100), Vector2.Zero);

            camera.SetAimPosition(player.GetPosition() + new Vector2(32, 32));
            camera.SetLimitView(true);
            //camera.SetLimitView(true);

        }

        public void Update(GameTime gameTime)
        {
            player.Update(gameTime);
            camera.SetAimPosition(player.GetPosition() + new Vector2(32, 32));
            Console.WriteLine(camera.OffSet);
        }


        public void Draw(Renderer renderer)
        {
            renderer.Begin();

            for (int i = 0; i < map.Data.GetLength(0); i++)
            {
                for (int j = 0; j < map.Data.GetLength(1); j++)
                {
                    if (MapManager.GetNowMapArr()[i, j] != -1)
                        renderer.DrawTexture("TileMapSource", map.GetBlockPosition(new BlockIndex(j, i)) + camera.OffSet, GetRect(map.Data[i, j]));
                }
            }
            player.Draw(renderer);

            renderer.End();

        }

        public Rectangle GetRect(int num)
        {
            Rectangle rect = new Rectangle(num % 4 * 64, num / 4 * 64, 64, 64);
            return rect;
        }


        public bool IsEnd()
        {
            return isEnd;
        }

        public Scene Next()
        {
            throw new NotImplementedException();
        }

        public void ShutDown()
        {
            throw new NotImplementedException();
        }


    }
}
