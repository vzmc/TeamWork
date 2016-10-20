﻿////////////////////////////////////////////////////////////////////////////////
// 水滝のクラス
// 作成者：氷見悠人
//////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TeamWorkGame.Device;
using TeamWorkGame.Def;
using TeamWorkGame.Scene;
using TeamWorkGame.Utility;

namespace TeamWorkGame.Actor
{
    public class WaterLine
    {
        private List<Water> waters; //沢山の水
        private Size waterSize;
        private Vector2 position;   //左上の座標
        private Timer timer;
        private Map mapdata;
        private bool isCollisioned;

        public int WaterCount
        {
            get
            {
                return waters.Count;
            }
        }

        public List<Water> Waters
        {
            get
            {
                return waters;
            }
        }

        public WaterLine(Vector2 pos)
        {
            position = pos;
            waters = new List<Water>();
            mapdata = MapManager.GetNowMapData();
            timer = new Timer(0.3f);
            Initialize();
        }

        public void Initialize()
        {
            waters.Clear();
            waters.Add(new Water(position, Vector2.Zero));
            waterSize = waters[0].ColSize;
            timer.Initialize();
        }

        public void Update(GameTime gameTime)
        {
            timer.Update();
            if (timer.IsTime())
            {
                if (isCollisioned)
                {
                    RemoveWater();
                }
                else
                {
                    MakeWater();
                    isCollisioned = CheckNextMapColision() || CheckNextThingsColision();
                }
                timer.Initialize(); 
            }
        }

        public void Draw(GameTime gameTime, Renderer renderer, Vector2 offset) 
        {
            foreach(var w in waters)
            {
                w.Draw(gameTime, renderer, offset);
            }
        }

        private void RemoveWater()
        {
            if(waters.Count > 0)
            {
                waters.RemoveAt(0);
            }
        }

        private void MakeWater()
        {
            Water water = new Water(position + (new Vector2(0, 64)) * waters.Count, Vector2.Zero);
            waters.Add(water);
        }

        private bool CheckNextThingsColision()
        {
            bool flag = false;
            Vector2 pos = position + (new Vector2(0, 64)) * waters.Count;

            foreach (var x in mapdata.MapThings.FindAll(y => !y.IsTrigger))
            {
                if (Method.CollisionCheck(pos, waterSize.Width, waterSize.Height, x.Position, x.ColSize.Width, x.ColSize.Height))
                {
                    flag = true;
                }
            }

            return flag;
        }

        private bool CheckNextMapColision()
        {
            bool flag = false;
            Vector2 pos = position + (new Vector2(0, 64)) * waters.Count;
            int nextPosType = mapdata.GetBlockData(pos);

            foreach (var x in Parameter.BackGrounds)
            {
                if (nextPosType == (int)x)
                {
                    flag = true;
                    break;
                }
            }

            return flag;
        }
    }
}
