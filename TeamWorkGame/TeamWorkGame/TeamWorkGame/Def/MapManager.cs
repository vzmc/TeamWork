﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TeamWorkGame.Utility;
using System.Diagnostics;               //Assert


namespace TeamWorkGame.Def
{
    public static class MapManager
    {
        private static List<int[,]> mapdatas = new List<int[,]>();
        private static int nowMapIndex = 0;
        private static Map map = null;
        private static int blockSize = 64;

        public static void Init()
        { 
            int[,] mapdata1 = new int[,]
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
            mapdatas.Add(mapdata1);

            int[,] mapdata2 = new int[,]
            {
                { 1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,1 },
                { 1,-1,-1,-1,-1,0,0,0,0,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,0,0,0,1 },
                { 1,-1,-1,-1,0,0,-1,-1,-1,-1,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,0,-1,-1,-1,1 },
                { 1,-1,0,0,-1,-1,-1,-1,-1,-1,-1,0,0,0,-1,-1,-1,-1,-1,0,-1,-1,-1,-1,1 },
                { 1,0,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,1 },
                { 1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,0,0,0,0,-1,-1,-1,-1,-1,1 },
                { 1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,0,-1,-1,-1,-1,0,-1,-1,-1,-1,1 },
                { 1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,0,-1,-1,-1,-1,-1,-1,0,-1,-1,-1,1 },
                { 1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,1 },
                { 1,-1,-1,-1,-1,-1,-1,0,0,0,0,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,0,0,1 },
                { 1,-1,-1,-1,-1,-1,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,1 },
                { 1,-1,-1,-1,-1,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,0,0,0,-1,-1,-1,1 },
                { 1,-1,-1,-1,0,-1,-1,-1,-1,-1,-1,-1,-1,0,0,0,-1,-1,-1,-1,-1,-1,-1,-1,1 },
                { 1,-1,-1,0,-1,-1,-1,-1,0,0,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,1 },
                { 1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,1 },
                { 1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,1 },
            };
            mapdatas.Add(mapdata2);


            map = new Map(mapdatas[nowMapIndex], blockSize);
        }

        public static void SetNowMap(int i)
        {
            Debug.Assert(
                i >=0 && i < mapdatas.Count,
                "MapManager->SetNowMapパラメタ範囲外！");
            if (i < 0)
            {
                i = 0;
            }
            else if(i >= mapdatas.Count)
            {
                i = mapdatas.Count;
            }

            if (nowMapIndex != i)
            {
                nowMapIndex = i;
                map = new Map(mapdatas[nowMapIndex], blockSize);
            }
        }
       
        public static int[,] GetNowMapArr()
        {
            return mapdatas[nowMapIndex];
        }

        public static Map GetNowMapData()
        {
            return map;
        }
    }
}
