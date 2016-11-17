//////////////////////////////////////////////////////
//TitleSceneの選択機能
//作成時間：2016/10/13
//作成者：柏杳
//最終修正時間：2016/11/10
//最終修正者：柏杳
//////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using TeamWorkGame.Device;
using TeamWorkGame.Utility;
using TeamWorkGame.Def;

namespace TeamWorkGame.Scene
{
    class TitleSelect       //柏
    {
        private Timer flashTimer;
        private InputState inputState;

        private float startTextalpha;   //透明度
        private Vector2 startTextPosition;  //座標

        private float worldTextalpha;
        private Vector2 worldTextPosition;

        private float creditTextalpha;
        private Vector2 creditTextPosition;

        private float exitTextalpha;
        private Vector2 exitTextPosition;

        private Vector2 selectPosition1;
        private Vector2 selectPosition2;
        private Vector2 selectPosition3;

        private bool isStarted;
        private int x;

        public TitleSelect(InputState inputState)
        {
            this.inputState = inputState;
            Initialize();
        }

        public void Initialize()
        {
            isStarted = false;
            x = 1;
            startTextalpha = 1;
            worldTextalpha = 1;
            creditTextalpha = 1;
            exitTextalpha = 1;
            flashTimer = new Timer(0.2f);
            startTextPosition = new Vector2(550, 600);
            worldTextPosition = new Vector2(550, 400);
            creditTextPosition = new Vector2(550, 480);
            exitTextPosition = new Vector2(550, 560);
            selectPosition1 = new Vector2(470, 390);
            selectPosition2 = new Vector2(470, 470);
            selectPosition3 = new Vector2(470, 550);
        }

        public void Update()
        {
            Select();
            Flash();
        }

        //点滅する処理
        private void Flash()
        {
            flashTimer.Update();
            if (flashTimer.IsTime())
            {
                if (!isStarted)
                {
                    if (startTextalpha == 1.0f) { startTextalpha = 0.5f; flashTimer.Initialize(); }
                    else { startTextalpha = 1.0f; flashTimer.Initialize(); }
                }
                else {
                    switch (x)
                    {
                        case 1:
                            creditTextalpha = 1.0f;
                            if (worldTextalpha == 1.0f) { worldTextalpha = 0.5f; flashTimer.Initialize(); }
                            else { worldTextalpha = 1.0f; flashTimer.Initialize(); }
                            break;
                        case 2:
                            worldTextalpha = 1.0f;
                            exitTextalpha = 1.0f;
                            if (creditTextalpha == 1.0f) { creditTextalpha = 0.5f; flashTimer.Initialize(); }
                            else { creditTextalpha = 1.0f; flashTimer.Initialize(); }
                            break;
                        case 3:
                            creditTextalpha = 1.0f;
                            if (exitTextalpha == 1.0f) { exitTextalpha = 0.5f; flashTimer.Initialize(); }
                            else { exitTextalpha = 1.0f; flashTimer.Initialize(); }
                            break;
                    }
                }
            }
        }

        //選択肢チェンジ機能
        private void Select()
        {
            if (!isStarted) { return; }
            if (inputState.IsKeyDown(Keys.Down) || inputState.IsKeyDown(Buttons.LeftThumbstickDown))
            {
                if (x == 3) { return; }
                x++;
            }
            else if (inputState.IsKeyDown(Keys.Up) || inputState.IsKeyDown(Buttons.LeftThumbstickUp))
            {
                if (x == 1) { return; }
                x--;
            }
        }

        //選択された選択肢は外に出す
        public int GetSelect
        {
            get { return x; }
        }

        //今映すのはStartかWorldとStaffか、外に表明する
        public bool GetStarted
        {
            get { return isStarted; }
            set { isStarted = value; }
        }


        //状況に合わせて描画する
        public void Draw(Renderer renderer)
        {
            if (!isStarted)
            {
                renderer.DrawTexture("text", startTextPosition, new Rectangle(0, (int)Text.START, Parameter.TextWidth, Parameter.TextHeight), startTextalpha);
                //renderer.DrawTexture("GameStartText", startTextPosition, startTextalpha);
            }
            else {
                //renderer.DrawTexture("WorldText", worldTextPosition, worldTextalpha);
                renderer.DrawTexture("text", worldTextPosition, new Rectangle(0, (int)Text.START, Parameter.TextWidth, Parameter.TextHeight), worldTextalpha);
                //renderer.DrawTexture("StaffText", staffTextPosition, staffTextalpha);
                renderer.DrawTexture("text", creditTextPosition, new Rectangle(0, (int)Text.CREDIT * Parameter.TextHeight, Parameter.TextWidth, Parameter.TextHeight), creditTextalpha);
                renderer.DrawTexture("text", exitTextPosition, new Rectangle(0, (int)Text.EXIT * Parameter.TextHeight, Parameter.TextWidth, Parameter.TextHeight), exitTextalpha);
                switch (x)
                {
                    case 1:
                        renderer.DrawTexture("hero", selectPosition1);
                        break;
                    case 2:
                        renderer.DrawTexture("hero", selectPosition2);
                        break;
                    case 3:
                        renderer.DrawTexture("hero", selectPosition3);
                        break;
                }

            }

        }
    }
}
