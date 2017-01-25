/////////////////////////////////////////////////
// Game1
// �ŏI�C�����ԁF2016�N11��17��
// By�@��
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
        //�t�B�[���h
        private GraphicsDeviceManager graphicsDeviceManager;//�O���t�B�b�N�@��Ǘ���
        private GameDevice gameDevice;                      //�Q�[���f�o�C�X
        private Renderer renderer;                          //�`��I�u�W�F�N�g�̐錾
        private Sound sound;                                //Sound�Ǘ�
        private SceneManager sceneManager;                  //�V�[���Ǘ���

        public Game1()
        {
            //�O���t�B�b�N�@��Ǘ��҂̎��̂𐶐�
            graphicsDeviceManager = new GraphicsDeviceManager(this);
            graphicsDeviceManager.PreferredBackBufferWidth = Parameter.ScreenWidth;       //��ʉ���
            graphicsDeviceManager.PreferredBackBufferHeight = Parameter.ScreenHeight;      //��ʏc��

            //�R���e���c�f�[�^�̕ۑ��t�H���_��Content�ɐݒ�
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
            //�Q�[���f�o�C�X�̎��̐���
            gameDevice = new GameDevice(Content, GraphicsDevice);

            sound = gameDevice.GetSound();

            //�`��I�u�W�F�N�g�̐錾
            renderer = gameDevice.GetRenderer();

            base.Window.Title = "�v�����e�E�X�̉�";

            base.Initialize();  //��΂ɏ�����

            sceneManager = new SceneManager();
            //IScene playScene = new PlayScene(gameDevice);
            sceneManager.Add(SceneType.Load, new Load(gameDevice));

            sceneManager.Add(SceneType.Title, new Title(gameDevice, this.Exit));
            
            //�X�e�[�W�N���X�̒ǉ�
            //�t������
            //�Q�O�P�U�N�P�O���P�Q��
            sceneManager.Add(SceneType.Stage, new Stage(gameDevice));
            sceneManager.Add(SceneType.StageIn, new StageIn(1.2f));

            sceneManager.Add(SceneType.Credit, new Credit(gameDevice));
            //�X�e�[�W�I���̒ǉ�
            //�t������
            //�Q�O�P�U�N�P�O���P�R��
            sceneManager.Add(SceneType.SmallStage, new SmallStage(gameDevice));
            sceneManager.Add(SceneType.PlayScene, new PlayScene(gameDevice, 0));

            //�G���f�B���O���o�ǉ��@by���@2017.1.11
            //sceneManager.Add(SceneType.ToEnd, new ToEnd(1.2f));
            sceneManager.Add(SceneType.Ending, new Ending(gameDevice));


            sceneManager.Change(new NextScene(SceneType.Load, -1));

        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            //Load�V�[�����K�v�ȕ����ɓǂݎ��
            gameDevice.LoadContent();
            
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        /// 
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
            // �I�������@Allows the game to exit
            if ((GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                || (Keyboard.GetState().IsKeyDown(Keys.Escape)))
            {
                this.Exit();
            }
            
            //�Q�[���f�o�C�X�X�V
            gameDevice.Update(gameTime);

            //�V�[���̍X�V
            sceneManager.Update(gameTime);

            base.Update(gameTime);  //��΂ɏ�����
        }

        /// <summary>
        /// �Q�[���S�̂ŕ`��A�`��J�n�ƏI���͂����Ŏ��s����@BY���@10/13
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            //�`��N���A���̐F��ݒ�
            GraphicsDevice.Clear(Color.Black);

            renderer.Begin();
            //�V�[���̕`��
            sceneManager.Draw(gameTime, renderer);

            renderer.End();
            base.Draw(gameTime);    //��΂ɏ�����
        }
    }
}
