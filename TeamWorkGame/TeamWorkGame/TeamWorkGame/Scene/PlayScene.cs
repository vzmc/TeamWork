/////////////////////////////////////////////////
// PlayerScene
// 最終修正時間：2016年10月19日
// By　佐瀬拓海
/////////////////////////////////////////////////
////////////////////////////////////////////////////////////////
//プレイシーン
//作成時間：2016/9/26
//作成者：氷見悠人
//最終更新日：10月13日
//By　氷見悠人
////////////////////////////////////////////////////////////
//最終更新日：10月19日
//By　長谷川修一

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
        private bool isClear;
        private int mapIndex;
        private Player player;
        private Camera camera;
        private Map map;
        private List<Fire> fires;
        //private InputState inputState;    //特にいりません　By　氷見悠人
        private List<GameObject> coals;     //マップに存在した炭の数　By佐瀬拓海
        private List<GameObject> nowCoals;  //現在の炭の数 By佐瀬拓海
        private InputState inputState;
        private ClearSelect clearSelect;    //clear後の選択画面


        public PlayScene(GameDevice gameDevice, int mapIndex = 0)
        {
            this.gameDevice = gameDevice;
            this.mapIndex = mapIndex;
            isEnd = false;
        }

        public void Initialize()
        {
            isEnd = false;
            isClear = false;
            //inputState = new InputState();
            MapManager.SetNowMap(mapIndex);
            map = MapManager.GetNowMapData();
            fires = new List<Fire>();
            coals = new List<GameObject>();
            coals = map.MapThings.FindAll(x => x is Coal);
            nowCoals = new List<GameObject>();
            camera = new Camera();
            clearSelect = new ClearSelect(gameDevice.GetInputState());　//InputStateはGameDeviceからもらいます　By　氷見悠人
            player = new Player(gameDevice.GetInputState(), new Vector2(100, 100), Vector2.Zero, ref fires);
            camera.SetAimPosition(player.Position + new Vector2(32, 32));
            camera.SetLimitView(true);
        }

        public void Initialize(int stageIndex)
        {
            //inputState = new InputState();
            clearSelect = new ClearSelect(gameDevice.GetInputState());　//InputStateはGameDeviceからもらいます　By　氷見悠人
            mapIndex = stageIndex;
            isEnd = false;
            isClear = false;
            MapManager.SetNowMap(mapIndex);
            map = MapManager.GetNowMapData();
            fires = new List<Fire>();
            coals = new List<GameObject>();
            coals = map.MapThings.FindAll(x => x is Coal);
            nowCoals = new List<GameObject>();
            camera = new Camera();
            player = new Player(gameDevice.GetInputState(), new Vector2(100, 100), Vector2.Zero, ref fires);
            camera.SetAimPosition(player.Position + new Vector2(32, 32));
            camera.SetLimitView(true);
        }

        public void Update(GameTime gameTime)
        {
            //inputState.Update();
            if (!isClear)
            {//マップ上の物達の更新
                foreach (var m in map.MapThings)
                {
                    m.Update(gameTime);
                }

                //プレイヤーの更新
                player.Update(gameTime);

                //火の更新
                for (int i = fires.Count - 1; i >= 0; i--)
                {
                    fires[i].Update(gameTime);

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

                //死んだ火を消す
                fires.RemoveAll(x => x.IsDead); // && x.IsOnGround 

                //マップの更新
                map.Update(gameTime);

                //カメラの注視位置を更新
                //camera.SetAimPosition(player.Position + new Vector2(player.ImageSize.Width / 2, player.ImageSize.Height / 2));
                camera.MoveAimPosition(player.Position + new Vector2(player.ImageSize.Width / 2, player.ImageSize.Height / 2));
                //Console.WriteLine(camera.OffSet);
                //マップ上にある炭の数を取得
                nowCoals = map.MapThings.FindAll(x => x is Coal);

                //マップの更新
                map.Update(gameTime);

                if (map.GetGoal() != null)
                    if (map.GetGoal().IsOnFire)
                    {
                        isClear = true;
                        clearSelect.IsClear = true;
                    }
            }
            clearSelect.Update();
            isEnd = clearSelect.IsEnd;  //clear窓口からend状態をとる
        }

        //描画の開始と終了は全部Game1のDrawに移動した
        public void Draw(Renderer renderer)
        {
            //renderer.Begin();

            for (int i = 0; i < map.Data.GetLength(0); i++)
            {
                for (int j = 0; j < map.Data.GetLength(1); j++)
                {
                    //動かないマップチップはここで描画 by 長谷川修一
                    if (MapManager.GetNowMapArr()[i, j] == 1 || MapManager.GetNowMapArr()[i, j] == 2)
                    {
                        renderer.DrawTexture("TileMapSource", map.GetBlockPosition(new BlockIndex(j, i)) + camera.OffSet, GetRect(map.Data[i, j]));
                    }

                }
            }

            //map.MapThings.ForEach(x => x.Draw(renderer, camera.OffSet));
            foreach (var x in map.MapThings)//By　佐瀬 拓海
            {
                if(x is Ice || x is Iron) //溶けて再度固まるブロックは別に描画
                {
                    x.Draw(renderer, camera.OffSet, x.GetAlpha());
                }
                //-----by長谷川修一 10/18
                else if (x is Straw)
                {
                    x.Draw(renderer, camera.OffSet, ((Straw)x).GetScale(), 1.0f);
                }
                else if (x is Tree)
                {
                    x.Draw(renderer, camera.OffSet, ((Tree)x).GetScale(), 1.0f);
                }
                //-----
                else
                {
                    x.Draw(renderer, camera.OffSet);
                }
            }

            player.Draw(renderer, camera.OffSet);

            fires.ForEach(x => x.Draw(renderer, camera.OffSet));

            clearSelect.Draw(renderer);

            //炭の取得数を描画 By佐瀬拓海
            renderer.DrawTexture("coal", new Vector2(1088, 64));
            renderer.DrawNumber("number", new Vector2(1152, 64), coals.Count - nowCoals.Count);

            //renderer.End();
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

        //public SceneType Next()
        //{
        //    return SceneType.Ending;
        //}

        public void ShutDown()
        {

        }

        NextScene IScene.Next()
        {

            //clear画面の選択肢によって、処理する
            NextScene nextScene;
            if (clearSelect.GetSelect == 0)
            {      //Next
                if (mapIndex == MapManager.StageCount - 1)
                {
                    mapIndex = 0;
                }
                else
                {
                    mapIndex++;
                }

                nextScene = new NextScene(SceneType.PlayScene, mapIndex);
            }
            else if (clearSelect.GetSelect == 1)
            {     //RePlay

                nextScene = new NextScene(SceneType.PlayScene, mapIndex);
            }
            else {      //World
                nextScene = new NextScene(SceneType.Stage, -1);
            }

            return nextScene;
        }
    }
}
