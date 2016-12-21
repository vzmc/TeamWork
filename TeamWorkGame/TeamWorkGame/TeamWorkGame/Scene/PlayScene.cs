/////////////////////////////////////////////////
//プレイシーン
//作成時間：2016/9/26
//作成者：氷見悠人
// 最終修正時間：2016年12月14日
// By　by柏　ＳＥ実装
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
        private InputState input;
        private Sound sound;
        private bool isEnd;     //終了、Fade開始
        private bool isTrueEnd; //Fade終了、Scene終了
        private bool isClear;
        private bool isPause;   //一時停止状態　By　氷見悠人
        private bool isView;        //カメラ操作中　By　氷見悠人

        private float fadein;

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
        private Goal goal;

        private Timer StartTimer;
        //float zoomRate = 0.01f;
        //bool isDrawed = false;

        //柏
        private StageSever stageSever;
        private int playTime;


        public PlayScene(GameDevice gameDevice, int mapIndex = 0)
        {
            this.gameDevice = gameDevice;
            sound = this.gameDevice.GetSound();
            input = this.gameDevice.GetInputState();
            this.mapIndex = mapIndex;
            //isEnd = false;
        }

        public void Initialize(int stageIndex)
        {
            StartTimer = new Timer(1f);
            mapIndex = stageIndex;
            isEnd = false;
            isClear = false;
            //葉梨竜太
            isOver = false;
            isPause = false;    //一時停止状態　By　氷見悠人
            //isStarting = true;
            isView = false;

            fadein = 1.0f;

            //全局Animation一時停止のスイッチ　By　氷見悠人
            FuncSwitch.AllAnimetionPause = false;

            MapManager.SetNowMap(mapIndex, sound);  // by柏　SE実装 2016.12.14
            map = MapManager.GetNowMapData();
            fires = new List<Fire>();
            waterLines = new List<WaterLine>();
            foreach (var ice in map.MapThings.FindAll(x => x is Ice))
            {
                ((Ice)ice).SetWaters(waterLines);
            }
            coals = new List<GameObject>();
            coals = map.MapThings.FindAll(x => x is Coal);
            nowCoals = new List<GameObject>();

            //氷見悠人
            player = new Player(gameDevice, MapManager.PlayerStartPosition(), Vector2.Zero, ref fires, ref waterLines, isView);
            
            clearSelect = new ClearSelect(gameDevice.GetInputState(), player, sound);   //by 柏　2016.12.14 ＳＥ実装

            camera = new Camera(player.Position + new Vector2(32, 32), Parameter.CameraScale, true);

            fireMeter = new FireMeter();

            //Goalを取得とCamera設置  By　氷見悠人
            goal = map.GetGoal();
            goal.SetCamera(camera);

            //柏
            stageSever = gameDevice.GetStageSever();
            playTime = 0;

            //PlayBGM
            sound.PlayBGM("forest1");
        }

        //Goal出現用の状態変換　By　氷見悠人
        public void ChangeGoalStage(GameTime gameTime)
        {
            if (goal.State == GoalState.NONE && nowCoals.Count == 0)
            {
                camera.SetLimitView(false);
                goal.IsComplete = true;
            }
        }

        private void CameraControl()
        {
            Vector2 pos = camera.AimPosition;
            pos += input.RightVelocity() * 15;
            camera.SetAimPosition(pos);
        }

        public void Update(GameTime gameTime)
        {
            //if(fadein > 0)
            //{
            //    fadein -= (float)(gameTime.ElapsedGameTime.TotalSeconds * 1f);
            //    if(fadein < 0)
            //    {
            //        fadein = 0;
            //    }
            //}

            StartTimer.Update();
            if (StartTimer.IsTime())
            {
                StartTimer.Stop();
            }


            //死んでいないと更新する
            if (!isClear && !isOver && !isPause)
            {
                //カメラ操作中、別の操作は不可
                if (input.CheckDownKey(Keys.RightControl, Buttons.LeftShoulder))
                {
                    isView = true;
                    player.IsView = isView;
                }
                else
                {
                    isView = false;
                    player.IsView = isView;
                }

                if (isView)
                {
                    CameraControl();
                }

                //Goal出現の時に、全画面の更新を一時停止、Goalの演出だけをする By 氷見悠人
                if (goal.State == GoalState.APPEARING)
                {
                    FuncSwitch.AllAnimetionPause = true;

                    goal.Update(gameTime);
                    camera.MoveAimPosition(goal.Position + new Vector2(goal.Width / 2, goal.Height / 2));
                }
                else
                {
                    FuncSwitch.AllAnimetionPause = false;
                    playTime++;

                    for (int i = 0; i < map.MapThings.Count; i++)
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
                    foreach (var w in waterLines)
                    {
                        w.Update(gameTime);
                    }

                    waterLines.RemoveAll(x => x.IsDead);

                    //マップの更新
                    map.Update(gameTime);

                    //カメラの注視位置を更新
                    //camera.SetAimPosition(player.Position + new Vector2(player.ImageSize.Width / 2, player.ImageSize.Height / 2));

                    if (!isView)
                    {
                        camera.MoveAimPosition(player.Position + new Vector2(player.Width / 2, player.Height / 2));
                    }
                    //Console.WriteLine(camera.OffSet);
                    
                    //マップ上にある炭の数を取得
                    nowCoals = map.MapThings.FindAll(x => x is Coal);

                    ChangeGoalStage(gameTime);
                    //マップの更新
                    //map.Update(gameTime);

                    if (map.GetGoal() != null)
                        if (map.GetGoal().IsOnFire)
                        {
                            //柏
                            stageSever.ClearStage = mapIndex;
                            stageSever.PlayTime = playTime / 60;
                            stageSever.CurrentStage = mapIndex;
                            stageSever.Charcoal = coals.Count - nowCoals.Count;
                            stageSever.SaveStageData();

                            sound.PlaySE("GameClear");  //by柏 SE実装 2016.12.14
                            isClear = true;
                            clearSelect.IsClear = true;
                            //FuncSwitch.AllAnimetionPause = true;
                        }

                    //葉梨竜太
                    if (player.IsDead)
                    {
                        isOver = true;
                        //isClear = true;
                        //clearSelect.IsClear = true;
                    }
                }
            }
            else
            {
                FuncSwitch.AllAnimetionPause = false;
                sound.StopBGM();
            }

            //　ClearWindow2が出るように変更(KeyもQに変更)　By佐瀬拓海
            if (!player.IsDead && !isClear)
            {
                if (gameDevice.GetInputState().CheckTriggerKey(Keys.Q, Parameter.MenuButton))
                {
                    if (isOver == false)
                    {
                        sound.PlaySE("pauseMenu");  //by柏　2016.12.14 ＳＥ実装

                        //全体Animationを一時停止
                        isPause = true;
                        clearSelect.IsPause = isPause;
                        clearSelect.GetSelect = 1;
                        isOver = true;
                    }
                    else if (isOver == true)
                    {
                        sound.PlaySE("pauseMenu");  //by柏　2016.12.14 ＳＥ実装

                        //全体Animationを一時停止解除
                        isPause = false;
                        clearSelect.IsPause = isPause;
                        isOver = false;
                    }
                }
            }

            clearSelect.Update();
            isEnd = clearSelect.IsEnd;  //clear窓口からend状態をとる
            if (isOver)                 //SceneのisOverで判断する
            {
                if (player.IsDead)
                {
                    //player.Death();
                }
                if (isPause) {
                    clearSelect.IsClear = true;
                    return;
                }
                else {
                    if (clearSelect.IsClear) { return; }
                }
                if (clearSelect.IsClear) { return; }
                clearSelect.Initialize();
                clearSelect.IsClear = true;
            }
            else if (!isOver && !isClear && !isPause)   //isOver = falseでゲーム画面に戻れる By佐瀬拓海
            {
                clearSelect.Initialize();
                clearSelect.IsClear = false;
                sound.PlayBGM("forest1");
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
                    if (MapManager.GetNowMapArr()[i, j] == 1)
                    {
                        renderer.DrawTexture("ground2", map.GetBlockPosition(new BlockIndex(j, i)) * camera.Scale + camera.OffSet, camera.Scale, 1.0f);
                    }
                    if (MapManager.GetNowMapArr()[i, j] == 2)
                    {
                        renderer.DrawTexture("ground1", map.GetBlockPosition(new BlockIndex(j, i)) * camera.Scale + camera.OffSet, camera.Scale, 1.0f);
                    }
                    //if (MapManager.GetNowMapArr()[i, j] == 1 || MapManager.GetNowMapArr()[i, j] == 2)
                    //{
                    //    renderer.DrawTexture("TileMapSource", map.GetBlockPosition(new BlockIndex(j, i)) * camera.Scale + camera.OffSet, GetRect(map.Data[i, j]), camera.Scale, 1.0f);
                    //}
                }
            }

            foreach (var x in map.MapThings)//By　佐瀬 拓海
            {
                x.Draw(gameTime, renderer, camera.OffSet, camera.Scale);
            }

            player.Draw(gameTime, renderer, camera.OffSet, camera.Scale);

            fires.ForEach(x => x.Draw(gameTime, renderer, camera.OffSet, camera.Scale));

            waterLines.ForEach(x => x.Draw(gameTime, renderer, camera.OffSet, camera.Scale));

            clearSelect.Draw(gameTime, renderer, camera.Scale); //ClearSelectの引数を変更したためこちらも変更

            fireMeter.Draw(renderer, player);

            //炭の取得数を描画 By佐瀬拓海
            renderer.DrawTexture("coal", new Vector2(1024, 16));
            renderer.DrawNumber2("number", new Vector2(1122, 16), coals.Count - nowCoals.Count, 2);
            renderer.DrawNumber("number", new Vector2(1154, 16), "/", 1);
            renderer.DrawNumber2("number", new Vector2(1218, 16), coals.Count, 2);

            //playTimeの表示(柏)
            //int[] playtime = stageSever.TimeCalculat(playTime/60);
            //renderer.DrawNumber("number", new Vector2(1152, 128), playtime[0]);
            //renderer.DrawNumber("number", new Vector2(1182, 128), "/", 1);
            //renderer.DrawNumber("number", new Vector2(1216, 128), playtime[1]);

            //if (fadein > 0)
            //{
            if (!StartTimer.IsTime())
            {
                float scale = 1.5f;
                renderer.DrawTexture("text", new Vector2((Parameter.ScreenWidth - Parameter.TextWidth * scale) / 2, (Parameter.ScreenHeight - Parameter.TextHeight * scale) / 2), new Rectangle(0, 0, Parameter.TextWidth, Parameter.TextHeight), scale, 1);
            }   //renderer.DrawTexture("fadein", Vector2.Zero, fadein);
            //}
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

        public void ShutDown()
        {
            sound.StopBGM();
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

            sound.PlaySE("decision1");

            return nextScene;
        }
    }
}
