/////////////////////////////////////////////////
// Game1
// 最終修正時間：2016年11月3日
// By　長谷川修一
/////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using TeamWorkGame.Actor;
using TeamWorkGame.Def;
using TeamWorkGame.Device;
using TeamWorkGame.Scene;
using TeamWorkGame.Utility;

namespace TeamWorkGame
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        //フィールド
        private GraphicsDeviceManager graphicsDeviceManager;//グラフィック機器管理者
        private GameDevice gameDevice;                      //ゲームデバイス
        private Renderer renderer;                          //描画オブジェクトの宣言
        private Sound sound;                                //Sound管理
        private SceneManager sceneManager;                  //シーン管理者

        public Game1()
        {
            //グラフィック機器管理者の実体を生成
            graphicsDeviceManager = new GraphicsDeviceManager(this);
            graphicsDeviceManager.PreferredBackBufferWidth = Parameter.ScreenWidth;       //画面横幅
            graphicsDeviceManager.PreferredBackBufferHeight = Parameter.ScreenHeight;      //画面縦幅

            //コンテンツデータの保存フォルダをContentに設定
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            //ゲームデバイスの実体生成
            gameDevice = new GameDevice(Content, GraphicsDevice);

            sound = gameDevice.GetSound();

            //描画オブジェクトの宣言
            renderer = gameDevice.GetRenderer();

            //csv管理のため削除
            //MapManager.Init();
            sceneManager = new SceneManager();
            //IScene playScene = new PlayScene(gameDevice);
            sceneManager.Add(SceneType.Title, new Title(gameDevice));
            
            //ステージクラスの追加
            //葉梨竜太
            //２０１６年１０月１２日
            sceneManager.Add(SceneType.Stage, new Stage(gameDevice));
            //ステージ選択の追加
            //葉梨竜太
            //２０１６年１０月１３日
            sceneManager.Add(SceneType.SmallStage, new SmallStage(gameDevice));
            sceneManager.Add(SceneType.PlayScene, new PlayScene(gameDevice, 0));
            sceneManager.Add(SceneType.Ending, new Ending(gameDevice));


            sceneManager.Change(new NextScene(SceneType.Title, -1));

            base.Window.Title = "プロメテウスの火";

            base.Initialize();  //絶対に消すな
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            renderer.LoadTexture("hero");
            renderer.LoadTexture("light_off");
            //renderer.LoadTexture("light_on");
            renderer.LoadTexture("TileMapSource");
            renderer.LoadTexture("fire");
            renderer.LoadTexture("tree");
            renderer.LoadTexture("ice");
            renderer.LoadTexture("iron");
            renderer.LoadTexture("title");
            renderer.LoadTexture("clear");
            renderer.LoadTexture("goal");
            renderer.LoadTexture("coal");//佐瀬拓海
            renderer.LoadTexture("number");//佐瀬拓海
            renderer.LoadTexture("straw");
            //ワールドマップの追加
            //葉梨竜太
            //２０１６年１０月１２日
            renderer.LoadTexture("worldmap");
            //ステージ選択肢の追加
            //背景の追加
            //葉梨竜太
            //２０１６年１０月１３日
            renderer.LoadTexture("smallmap");
            renderer.LoadTexture("frame");
            renderer.LoadTexture("forestBG");
            

            //by長谷川修一  11/3
            renderer.LoadTexture("FireMeter");
            renderer.LoadTexture("iceAnime");
            renderer.LoadTexture("ironAnime");
            renderer.LoadTexture("wood");
            renderer.LoadTexture("playerAnime");
            renderer.LoadTexture("throwAnime");
            renderer.LoadTexture("sand");
            renderer.LoadTexture("backGround");
            renderer.LoadTexture("woodAnime");
            renderer.LoadTexture("strawAnime");

            //柏
            renderer.LoadTexture("ClearWindow");
            renderer.LoadTexture("ClearWindow2");
            renderer.LoadTexture("selecter");
            renderer.LoadTexture("GameStartText");
            renderer.LoadTexture("WorldText");
            renderer.LoadTexture("StaffText");
            renderer.LoadTexture("balloon");
            

            //renderer.LoadTexture("puddle");

            renderer.LoadTexture("water");

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            renderer.Unload();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // 終了処理　Allows the game to exit
            if ((GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                || (Keyboard.GetState().IsKeyDown(Keys.Escape)))
            {
                this.Exit();
            }

            //ゲームデバイス更新
            gameDevice.Update(gameTime);

            //シーンの更新
            sceneManager.Update(gameTime);

            base.Update(gameTime);  //絶対に消すな
        }

        /// <summary>
        /// ゲーム全体で描画、描画開始と終了はここで実行する　BY張　10/13
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            //描画クリア時の色を設定
            GraphicsDevice.Clear(Color.CornflowerBlue);

            renderer.Begin();
            //シーンの描画
            sceneManager.Draw(gameTime, renderer);

            renderer.End();
            base.Draw(gameTime);    //絶対に消すな
        }
    }
}
