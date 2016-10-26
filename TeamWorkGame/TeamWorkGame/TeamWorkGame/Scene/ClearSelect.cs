/////////////////////////////////////////////
//　クリア画面
//  作成者：柏杳
/////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using TeamWorkGame.Device;
using TeamWorkGame.Actor;
using TeamWorkGame.Def;

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

        public ClearSelect(InputState inputState, Player player) {
            this.inputState = inputState;
            this.player = player;
            Initialize();
        }

        public void Initialize() {
            if (player.IsDead)
            {
                selected = new List<Vector2>() {
                Vector2.Zero,
                new Vector2(400,350),
                new Vector2(400,480),
                };
                select = 1;
            }
            else {
                selected = new List<Vector2>() {
                new Vector2(400,300),
                new Vector2(400,420),
                new Vector2(400,540),
                };
                select = 0;
            }

            isClear = false;
            isEnd = false;
        }

        public void Update() {


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


        public bool IsClear {
            get{ return isClear; }
            set { isClear = value; }
        }

        public int GetSelect {
            get { return select; }
        }

        public bool IsEnd {
            get { return isEnd; }
        }

        public void Draw(Renderer renderer) {
            if (isClear)
            {
                if (player.IsDead)
                {
                    renderer.DrawTexture("ClearWindow2", new Vector2(350, 250));
                }
                else {
                    renderer.DrawTexture("ClearWindow", new Vector2(350, 250));
                }

                renderer.DrawTexture("selecter", selected[select]);
            }
        }
    }
}
