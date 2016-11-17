/////////////////////////////////////////////
//　クリア画面
//  作成者：柏杳
//
//  最終更新日 11月16日
//  By佐瀬拓海
/////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using TeamWorkGame.Device;
using TeamWorkGame.Actor;
using TeamWorkGame.Def;
using TeamWorkGame.Utility;

namespace TeamWorkGame.Scene
{
    class ClearSelect
    {
        private InputState inputState;  //入力チェック
        private int select;     //選択肢
        private List<Vector2> selected; //選択肢登録
        private bool isClear;   //clear状態
        private bool isEnd;     //選択完了状態
        private Player player;
        //Animation関連
        private Animation standAnime;
        private AnimationPlayer animePlayer;
        private SpriteEffects flip = SpriteEffects.None;

        private Vector2 worldTextPosition;
        private Vector2 retryTextPosition;
        private Vector2 nextTextPosition;

        public ClearSelect(InputState inputState, Player player)
        {
            this.inputState = inputState;
            this.player = player;
            Initialize();
        }

        public void Initialize()
        {
            if (player.IsDead)
            {
                selected = new List<Vector2>() {
                Vector2.Zero,
                new Vector2(470,440),
                new Vector2(470,540),
                };
                select = 1;
            }
            else {
                selected = new List<Vector2>() {
                new Vector2(470,340),
                new Vector2(470,440),
                new Vector2(470,540),
                };
                select = 0;
            }

            worldTextPosition = new Vector2(550, 550);
            retryTextPosition = new Vector2(550, 450);
            nextTextPosition = new Vector2(550, 350);

            standAnime = new Animation(Renderer.GetTexture("standAnime"), 0.1f, true);//StandAnimeの実体生成

            isClear = false;
            isEnd = false;
        }

        public void Update()
        {


            if (isClear)    //clear状態だけ選択有効
            {
                if (inputState.CheckTriggerKey(Keys.Up, Buttons.LeftThumbstickUp))
                {
                    if (player.IsDead)
                    {
                        if (select == 1) { return; }
                    }
                    else {
                        if (select == 0) { return; }
                    }
                    select--;
                }
                else if (inputState.CheckTriggerKey(Keys.Down, Buttons.LeftThumbstickDown))
                {
                    if (select == 2) { return; }
                    select++;
                }


                //選択確定
                else if (inputState.CheckTriggerKey(Parameter.MenuKey, Parameter.JumpButton))
                {
                    isEnd = true;
                }
            }
        }


        public bool IsClear
        {
            get { return isClear; }
            set { isClear = value; }
        }

        public int GetSelect
        {
            get { return select; }
        }

        public bool IsEnd
        {
            get { return isEnd; }
        }

        public void Draw(GameTime gameTime, Renderer renderer, float cameraScale)
        { //Animationをさせるため引数を変更
            if (isClear)
            {
                if (player.IsDead)
                {
                    renderer.DrawTexture("text", retryTextPosition, new Rectangle(0, (int)Text.RETRY * Parameter.TextHeight, Parameter.TextWidth, Parameter.TextHeight));
                    renderer.DrawTexture("text", worldTextPosition, new Rectangle(0, (int)Text.WORLD * Parameter.TextHeight, Parameter.TextWidth, Parameter.TextHeight));
                    //renderer.DrawTexture("ClearWindow2", new Vector2(350, 250));
                }
                else
                {
                    //renderer.DrawTexture("ClearWindow", new Vector2(350, 250));
                    renderer.DrawTexture("clear", new Vector2(1280 / 2 - 472 / 2, 100));
                    renderer.DrawTexture("text", nextTextPosition, new Rectangle(0, (int)Text.NEXT * Parameter.TextHeight, Parameter.TextWidth, Parameter.TextHeight));
                    renderer.DrawTexture("text", retryTextPosition, new Rectangle(0, (int)Text.RETRY * Parameter.TextHeight, Parameter.TextWidth, Parameter.TextHeight));
                    renderer.DrawTexture("text", worldTextPosition, new Rectangle(0, (int)Text.WORLD * Parameter.TextHeight, Parameter.TextWidth, Parameter.TextHeight));

                }
                //Selecterをキャラ画像に変更
                animePlayer.PlayAnimation(standAnime);
                animePlayer.Draw(gameTime, renderer, selected[select], flip, cameraScale);


            }
        }
    }
}
