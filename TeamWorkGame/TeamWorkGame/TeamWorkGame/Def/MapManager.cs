﻿/////////////////////////////////////////////////
// 最終修正時間：2016年10月12日
// By 長谷川修一
/////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TeamWorkGame.Utility;
using TeamWorkGame.Actor;
using System.Diagnostics;               //Assert

namespace TeamWorkGame.Def
{
    /// <summary>
    /// Map管理
    /// </summary>
    public static class MapManager
    {
        private static int nowMapIndex = -1;
        private static Map map = null;
        private static int blockSize = 64;
        private static List<Action> CreateStage; 

        public static void Stage1()
        {
            List<Actor.GameObject> MapThings = new List<Actor.GameObject>();
            int[,] mapdata = new int[,]
            {
                { -1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1 },
                { -1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1 },
                { -1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1 },
                { -1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1 },
                { -1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1 },
                { -1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1 },
                { -1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1 },
                { -1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,1,1,1,-1,-1,-1,-1,-1,-1 },
                { -1,-1,-1,-1,-1,-1,-1,0,0,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1 },
                { -1,-1,-1,1,1,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1 },
                { -1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1 },
                { 2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2 },
            };

            Light light = new Light(new Vector2(300, 300));
            MapThings.Add(light);

            map = new Map(mapdata, blockSize, MapThings);
        }

        public static void Stage2()
        {
            List<Actor.GameObject> MapThings = new List<Actor.GameObject>();
            int[,] mapdata = new int[,]
            {
                { 1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,1 },
                { 1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,1 },
                { 1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,1 },
                { 1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,1 },
                { 1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,1 },
                { 1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,1 },
                { 1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,1 },
                { 1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,1 },
                { 1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,2,2,-1,-1,1 },
                { 1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,2,2,-1,-1,-1,-1,-1,1 },
                { 1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,2,2,2,-1,-1,-1,-1,-1,-1,-1,-1,1 },
                { 1,-1,-1,-1,-1,-1,-1,-1,-1,-1,2,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,1 },
                { 1,-1,-1,-1,-1,-1,-1,2,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,1 },
                { 1,-1,-1,-1,2,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,1 },
                { 1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,1 },
                { 1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,1 },
            };

            Light light = new Light(new Vector2(64 * 5, 64*(8)));
            Light light2 = new Light(new Vector2(64 * 10, 64 * (7)));

            Ice ice1 = new Ice(new Vector2(64 * 10, 64 * 14), Vector2.Zero);
            Ice ice2 = new Ice(new Vector2(64 * 11, 64 * 14), Vector2.Zero);

            MapThings.Add(light);
            MapThings.Add(light2);
            MapThings.Add(ice1);

            map = new Map(mapdata, blockSize, MapThings);
        }

        public static void Stage3()
        {
            List<Actor.GameObject> MapThings = new List<Actor.GameObject>();
            int[,] mapdata = new int[,]
            {
                { 1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,1 },
                { 1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,1 },
                { 1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,1 },
                { 1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,1 },
                { 1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,2,2,2,1 },
                { 1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,1 },
                { 1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,1 },
                { 1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,1 },
                { 1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,1 },
                { 1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,1 },
                { 1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,1 },
                { 1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,1 },
                { 1,-1,-1,-1,-1,-1,-1,2,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,1 },
                { 1,-1,-1,-1,2,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,1 },
                { 1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,1 },
                { 1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,1,1,1,1,1,1 },

            };

            Light light = new Light(new Vector2(64 * 11, 64 * 10));
            Light light2 = new Light(new Vector2(64 * 16, 64 * 8));
            Light light3 = new Light(new Vector2(64 * 21, 64 * 6));

            //Goal goal = new Goal(new Vector2(64 * 27, 64 * 4 - 44));
            Goal goal = new Goal(new Vector2(64 * 3, 64 * 12 - 44));
            MapThings.Add(goal);
            int x = 14;
            int y = 12;
            for(int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    Ice ice = new Ice(new Vector2(64 * x, 64 * y), Vector2.Zero);
                    MapThings.Add(ice);
                    x++;
                }
                x++;
                y--;
            }
            //鉄ブロックの追加
            //By長谷川修一 2016/10/12
            x = 10;
            y = 14;
            for (int i = 0; i < 10; i++)
            {
                Iron iron = new Iron(new Vector2(64 * x, 64 * y), Vector2.Zero);
                MapThings.Add(iron);
                x++;
            }

            MapThings.Add(light);
            MapThings.Add(light2);
            MapThings.Add(light3);

            map = new Map(mapdata, blockSize, MapThings);
        }


        public static void Init()
        {
            CreateStage = new List<Action>();
            CreateStage.Add(Stage1);
            CreateStage.Add(Stage2);
            CreateStage.Add(Stage3);
        }

        public static void SetNowMap(int i)
        {
            Debug.Assert(
                i >= 0 && i < CreateStage.Count,
                "MapManager->SetNowMapパラメタ範囲外！");
            if (i < 0)
            {
                i = 0;
            }
            else if (i >= CreateStage.Count)
            {
                i = CreateStage.Count;
            }

            //if (nowMapIndex != i)
            //{
                nowMapIndex = i;
                CreateStage[i]();
            //}
        }
       
        public static int[,] GetNowMapArr()
        {
            return map.Data;
        }

        public static Map GetNowMapData()
        {
            return map;
        }
    }
}
