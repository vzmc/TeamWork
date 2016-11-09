/////////////////////////////////////////////////
//プレイシーン
//作成時間：2016/9/26
//作成者：氷見悠人
// 最終修正時間：2016年11月09日
// By　柏
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
    class PlayScene : IScene
    {
        private GameDevice gameDevice;
        private bool isEnd;
        private bool isClear;

        //葉梨竜太
        private bool isOver;
        private int mapIndex;
        private Player player;
        private Camera camera;
        private Map map;
        private List<Fire> fires;
        private List<WaterLine> waterLines;
        private List<GameObject> coals;     //マップに存在した炭の数　By佐瀬拓海
        private List<GameObject> nowCoals;  //現在の炭の数 By佐瀬拓海
        private ClearSelect clearSelect;    //clear後の選択画面
        private FireMeter fireMeter;

        //柏
        private StageSever stageSever;
        private int playTime;


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
            //葉梨竜太
            isOver = false;
            //inputState = new InputState();
            MapManager.SetNowMap(mapIndex);
            map = MapManager.GetNowMapData();
            fires = new List<Fire>();
            waterLines = new List<WaterLine>();
            coals = new List<GameObject>();
            coals = map.MapThings.FindAll(x => x is Coal);
            nowCoals = new List<GameObject>();
            camera = new Camera();
            player = new Player(gameDevice.GetInputState(), new Vector2(100, 100), Vector2.Zero, ref fires, ref waterLines);
            clearSelect = new ClearSelect(gameDevice.GetInputState(), player);　//InputStateはGameDeviceからもらいます　By　氷見悠人
            camera.SetAimPosition(player.Position + new Vector2(32, 32));
            camera.SetLimitView(true);
            fireMeter = new FireMeter();

            //柏
            stageSever = gameDevice.GetStageSever();
            playTime = 0;
        }

        public void Initialize(int stageIndex)
        {
            mapIndex = stageIndex;
            isEnd = false;
            isClear = false;
            //葉梨竜太
            isOver = false;
            MapManager.SetNowMap(mapIndex);
            map = MapManager.GetNowMapData();
            fires = new List<Fire>();
            waterLines = new List<WaterLine>();
            foreach(var ice in map.MapThings.FindAll(x => x is Ice))
            {
                ((Ice)ice).SetWaters(waterLines);
            }
            coals = new List<GameObject>();
            coals = map.MapThings.FindAll(x => x is Coal);
            nowCoals = new List<GameObject>();
            camera = new Camera();
            //player = new Player(gameDevice.GetInputState(), new Vector2(100, 100), Vector2.Zero, ref fires, ref waterLines);　player自動生成のために削除

            //柏
            player = new Player(gameDevice.GetInputState(), MapManager.PlayerStartPosition(), Vector2.Zero, ref fires, ref waterLines);

            //葉梨竜太
            clearSelect = new ClearSelect(gameDevice.GetInputState(), player);　//InputStateはGameDeviceからもらいます　By　氷見悠人
            camera.SetAimPosition(player.Position + new Vector2(32, 32));
            camera.SetLimitView(true);
            fireMeter = new FireMeter();

            //柏
            stageSever = gameDevice.GetStageSever();
            playTime = 0;
        }

        public void Update(GameTime gameTime)
        {
            playTime++;
            //inputState.Update();
            //死んでいないと更新する
            if (!isClear && !isOver)
            {
                for(int i = 0; i < map.MapThings.Count; i++)
                {
                    map.MapThings[i].Update(gameTime);
                }

                //プレイヤーの更新
                player.Update(gameTime);

                //火の更新
                for (int i = fires.Count - 1; i >= 0; i--)
                {
                    fires[i].Update(gameTime);
                }

                //死んだ火を消す
                fires.RemoveAll(x => x.IsDead); // && x.IsOnGround 

                //水の更新
                foreach(var w in waterLines)
                {
                    w.Update(gameTime);
                }

                waterLines.RemoveAll(x => x.IsDead);

                //マップの更新
                map.Update(gameTime);

                //カメラの注視位置を更新
                //camera.SetAimPosition(player.Position + new Vector2(player.ImageSize.Width / 2, player.ImageSize.Height / 2));
                camera.MoveAimPosition(player.Position + new Vector2(player.Width / 2, player.Height / 2));
                //Console.WriteLine(camera.OffSet);
                //マップ上にある炭の数を取得
                nowCoals = map.MapThings.FindAll(x => x is Coal);


                //マップの更新
                map.Update(gameTime);

                if (map.GetGoal() != null)
                    if (map.GetGoal().IsOnFire)
                    {
                        //柏
                        stageSever.ClearStage = mapIndex;
                        stageSever.PlayTime = playTime / 60;
                        stageSever.CurrentStage = mapIndex;
                        stageSever.Charcoal = coals.Count - nowCoals.Count;
                        stageSever.SaveStageData();

                        isClear = true;
                        clearSelect.IsClear = true;
                    }

                //葉梨竜太
                if (player.IsDead)
                {
                    isOver = true;
                    //isClear = true;
                    //clearSelect.IsClear = true;
                }
            }
            //　ClearWindow2が出るように変更(KeyもQに変更)　By佐瀬拓海
            if (gameDevice.GetInputState().CheckTriggerKey(Keys.Q, Parameter.MenuButton))
            {
                if (isOver == false)
                {
                    isOver = true;
                    player.IsDead = true;
                }
                else if (isOver == true)
                {
                    isOver = false;
                    player.IsDead = false;
                }
            }

            clearSelect.Update();
            isEnd = clearSelect.IsEnd;  //clear窓口からend状態をとる
            if (isOver)                 //SceneのisOverで判断する
            {
                if (clearSelect.IsClear == true) { return; }
                clearSelect.Initialize();
                clearSelect.IsClear = true;
            }
            else if (isOver == false && isClear == false)   //isOver = falseでゲーム画面に戻れる By佐瀬拓海
            {
                clearSelect.Initialize();
                clearSelect.IsClear = false;
            }

            
        }

        //描画の開始と終了は全部Game1のDrawに移動した
        public void Draw(GameTime gameTime, Renderer renderer)
        {
            renderer.DrawTexture("backGround", Vector2.Zero);

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

            foreach (var x in map.MapThings)//By　佐瀬 拓海
            {
                x.Draw(gameTime, renderer, camera.OffSet);
            }

            player.Draw(gameTime, renderer, camera.OffSet);

            fires.ForEach(x => x.Draw(gameTime, renderer, camera.OffSet));

            waterLines.ForEach(x => x.Draw(gameTime, renderer, camera.OffSet));

            clearSelect.Draw(renderer);

            fireMeter.Draw(renderer, player);

            //炭の取得数を描画 By佐瀬拓海
            renderer.DrawTexture("coal", new Vector2(1088, 64));
            renderer.DrawNumber("number", new Vector2(1152, 64), coals.Count - nowCoals.Count);
            renderer.DrawNumber("number", new Vector2(1182, 64), "/", 1);
            renderer.DrawNumber("number", new Vector2(1216, 64), coals.Count);

            //playTimeの表示(柏)
            int[] playtime = stageSever.TimeCalculat(playTime/60);
            renderer.DrawNumber("number", new Vector2(1152, 128), playtime[0]);
            renderer.DrawNumber("number", new Vector2(1182, 128), "/", 1);
            renderer.DrawNumber("number", new Vector2(1216, 128), playtime[1]);
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
                if (mapIndex == StageDef.BigIndexMax * StageDef.SmallIndexMax - 1)
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
            else
            {     //World
                nextScene = new NextScene(SceneType.Stage, -1);
            }

            return nextScene;
        }
    }
}
