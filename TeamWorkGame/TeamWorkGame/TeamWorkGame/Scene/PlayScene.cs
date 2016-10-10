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
        private List<Fire> fires;

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
            fires = new List<Fire>();
            camera = new Camera();
            player = new Player(gameDevice.GetInputState(), new Vector2(100, 100), Vector2.Zero, ref fires);
            camera.SetAimPosition(player.Position + new Vector2(32, 32));
            camera.SetLimitView(true);
        }


        public void Update(GameTime gameTime)
        {
            foreach (var m in map.MapThings)
            {
                m.Update(gameTime);
            }

            player.Update(gameTime);


            foreach (var x in fires)
            {
                x.Update(gameTime);

                //foreach (var m in map.MapThings)
                //{
                //    x.CollisionCheck(m);
                //}

                //if (player.CollisionCheck(x))
                //{
                //    //x.IsReturn = true;
                //}
                //else
                //{
                //    //x.IsReturn = false;
                //}
            }

            fires.RemoveAll(x =>  x.IsDead); // && x.IsOnGround 

            map.Update(gameTime);

            camera.SetAimPosition(player.Position + new Vector2(player.ImageSize.Width/2, player.ImageSize.Height/2));
            //Console.WriteLine(camera.OffSet);

            if(map.GetGoal().IsOnFire)
            {
                isEnd = true;
            }
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

            map.MapThings.ForEach(x => x.Draw(renderer, camera.OffSet));

            player.Draw(renderer, camera.OffSet);

            fires.ForEach(x => x.Draw(renderer, camera.OffSet));

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
            return Scene.Ending;
        }

        public void ShutDown()
        {
            
        }
    }
}
