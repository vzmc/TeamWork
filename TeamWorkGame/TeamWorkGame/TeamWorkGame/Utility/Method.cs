////////////////////////////////////////////////////////////
// 重要のメソッド達
// 作成時間：2016/10/1
// 作成者：氷見悠人　
// 最終修正時間：2016/11/02
// 修正者:佐瀬拓海　CreateGimicksにSANDを追加
/////////////////////////////////////////////////////////
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TeamWorkGame.Actor;
using TeamWorkGame.Def;
using System.Diagnostics;   //Assert用


namespace TeamWorkGame.Utility
{
    public static class Method
    {
        /// <summary>
        /// Mapの障害物との衝突判定
        /// </summary>
        /// <param name="position">自分の位置、書き出す</param>
        /// <param name="width">自分の衝突横幅</param>
        /// <param name="height">自分の衝突高さ</param>
        /// <param name="velocity">自分の移動量、書き出す</param>
        /// <param name="map">所在地図</param>
        /// <param name="data">衝突地形のデータ</param>
        /// <returns>衝突したかどうか</returns>
        public static bool MapObstacleCheck(ref Vector2 position, Rectangle localColRect, ref Vector2 velocity, ref bool isOnGround, Map map, int[] data)
        {
            bool flag = false;

            Rectangle colRect = new Rectangle(localColRect.X + (int)position.X, localColRect.Y + (int)position.Y, localColRect.Width, localColRect.Height);

            Vector2[] nowLRPoints = new Vector2[2];
            Vector2[] nowUDPoints = new Vector2[2];

            Vector2[] nextLRPoints = new Vector2[2];
            Vector2[] nextUDPoints = new Vector2[2];

            Vector2 blockPos = Vector2.Zero;

            //左右移動の判断
            if (velocity.X < 0)
            {
                nowLRPoints[0].X = colRect.Left;
                nowLRPoints[0].Y = colRect.Top + 1;
                nowLRPoints[1].X = colRect.Left;
                nowLRPoints[1].Y = colRect.Bottom - 1;

                nextLRPoints[0] = nowLRPoints[0] + new Vector2(velocity.X, 0);
                nextLRPoints[1] = nowLRPoints[1] + new Vector2(velocity.X, 0);

                if(map.IsInBlock(nextLRPoints[0], ref blockPos, data) || map.IsInBlock(nextLRPoints[1], ref blockPos, data))
                {
                    velocity.X = 0;
                    position.X = blockPos.X + map.BlockSize - localColRect.X;
                    flag = true;
                }
            }
            else if(velocity.X > 0)
            {
                nowLRPoints[0].X = colRect.Right;
                nowLRPoints[0].Y = colRect.Top + 1;
                nowLRPoints[1].X = colRect.Right;
                nowLRPoints[1].Y = colRect.Bottom - 1;

                nextLRPoints[0] = nowLRPoints[0] + new Vector2(velocity.X, 0);
                nextLRPoints[1] = nowLRPoints[1] + new Vector2(velocity.X, 0);

                if (map.IsInBlock(nextLRPoints[0], ref blockPos, data) || map.IsInBlock(nextLRPoints[1], ref blockPos, data))
                {
                    velocity.X = 0;
                    position.X = blockPos.X - (localColRect.X + localColRect.Width);
                    flag = true;
                }
            }

            //上下移動の判断
            if(velocity.Y < 0)
            {
                nowUDPoints[0].X = colRect.Left + 1;
                nowUDPoints[0].Y = colRect.Top;
                nowUDPoints[1].X = colRect.Right - 1;
                nowUDPoints[1].Y = colRect.Top;

                nextUDPoints[0] = nowUDPoints[0] + new Vector2(0, (float)Math.Ceiling(velocity.Y));
                nextUDPoints[1] = nowUDPoints[1] + new Vector2(0, (float)Math.Ceiling(velocity.Y));

                if (map.IsInBlock(nextUDPoints[0], ref blockPos, data) || map.IsInBlock(nextUDPoints[1], ref blockPos, data))
                {
                    velocity.Y = 0;
                    position.Y = blockPos.Y + map.BlockSize - localColRect.Y;
                    flag = true;
                }
            }
            else if(velocity.Y > 0)
            {
                nowUDPoints[0].X = colRect.Left + 1;
                nowUDPoints[0].Y = colRect.Bottom;
                nowUDPoints[1].X = colRect.Right - 1;
                nowUDPoints[1].Y = colRect.Bottom;

                nextUDPoints[0] = nowUDPoints[0] + new Vector2(0, (float)Math.Ceiling(velocity.Y));
                nextUDPoints[1] = nowUDPoints[1] + new Vector2(0, (float)Math.Ceiling(velocity.Y));

                if (map.IsInBlock(nextUDPoints[0], ref blockPos, data) || map.IsInBlock(nextUDPoints[1], ref blockPos, data))
                {
                    velocity.Y = 0;
                    position.Y = blockPos.Y - (localColRect.Y + localColRect.Height);
                    isOnGround = true;
                    flag = true;
                }
                else
                {
                    isOnGround = false;
                }
            }

            return flag;
        }

        /// <summary>
        /// 物達の衝突判定（四角形同士）
        /// </summary>
        /// <param name="position1"></param>
        /// <param name="width1"></param>
        /// <param name="height1"></param>
        /// <param name="position2"></param>
        /// <param name="width2"></param>
        /// <param name="height2"></param>
        /// <returns>判定結果</returns>
        public static bool CollisionCheck(Vector2 position1, int width1, int height1, Vector2 position2, int width2, int height2)
        {
            bool flag = false;

            if ((position1.X <= position2.X + width2 - 1 && position1.X + width1 - 1 >= position2.X))
            {
                if((position1.Y <= position2.Y + height2 - 1 && position1.Y + height1 - 1 >= position2.Y))
                {
                    flag = true;
                }
            }

            return flag;
        }

        /// <summary>
        /// 障害物との判定
        /// </summary>
        /// <param name="self">自身</param>
        /// <param name="obstacle">障害物</param>
        /// <returns></returns>
        public static bool ObstacleCheck(GameObject self, GameObject obstacle)
        {
            bool flag = false;
            Vector2 selfNowPositon = self.Position;
            Vector2 selfNowVelocity = self.Velocity;
            Vector2 selfNextPositionH = selfNowPositon + new Vector2(selfNowVelocity.X, 0);
            Vector2 selfNextPositionV = selfNowPositon + new Vector2(0, (float)Math.Ceiling(selfNowVelocity.Y));

            Rectangle selfLocalColRect = self.LocalColRect;

            Rectangle selfNextColRectH = new Rectangle(self.LocalColRect.X + (int)selfNextPositionH.X, self.LocalColRect.Y + (int)selfNextPositionH.Y, self.LocalColRect.Width, self.LocalColRect.Height);
            Rectangle selfNextColRectV = new Rectangle(self.LocalColRect.X + (int)selfNextPositionV.X, self.LocalColRect.Y + (int)selfNextPositionV.Y, self.LocalColRect.Width, self.LocalColRect.Height);


            Vector2 obstaclePosition = obstacle.Position;
            Rectangle obstacleColRect = obstacle.ColRect;
            int obstacleWidth = obstacle.ColRect.Width;
            int obstacleHeight = obstacle.ColRect.Height;

            //横方向の判定
            if (selfNowVelocity.X != 0)
            {
                if (selfNextColRectH.Intersects(obstacleColRect))
                {
                    flag = true;

                    if (selfNowVelocity.X > 0)
                    {
                        self.PositionX = obstacleColRect.X - (selfLocalColRect.X + selfLocalColRect.Width);
                    }
                    else
                    {
                        self.PositionX = obstacleColRect.X + obstacleColRect.Width - selfLocalColRect.X;
                    }

                    self.VelocityX = 0;
                }
            }

            //縦方向の判定
            //if (selfNowVelocity.Y != 0)
            //{
                if (selfNextColRectV.Intersects(obstacleColRect))
                {
                    flag = true;

                    if (selfNowVelocity.Y >= 0)
                    {
                        self.PositionY = obstacleColRect.Y - (selfLocalColRect.Y + selfLocalColRect.Height);
                        self.IsOnGround = true;
                    }
                    else
                    {
                        self.PositionY = obstacleColRect.Y + obstacleColRect.Height - selfLocalColRect.Y;
                    }

                    self.VelocityY = 0;
                }
            //}

            //if (flag)
            //{
            //    Console.WriteLine("!!!!!!!!!!!!!1");
            //}

            return flag;
        }


        public static Vector2 PlayerStartPosition(int[,] mapdata) {
            for (int i = 0; i < mapdata.GetLength(0); i++)
            {
                for (int j = 0; j < mapdata.GetLength(1); j++)
                {
                    switch (mapdata[i, j])
                    {
                        case (int)GimmickType.PLAYER:
                            {
                                return new Vector2(j * 64, i * 64);
                            }
                    }
                }
            }
            return new Vector2(1 * 64, 0);
        }



        //by木材追加 長谷川修一 10/27
        /// <summary>
        /// ギミック設置
        /// </summary>
        /// <param name="mapdata">マップデータの二元配列</param>
        /// <param name="MapThings">マップ上の物のList</param>
        public static void CreateGimmicks(int[,] mapdata, List<GameObject> MapThings)
        {
            for (int i = 0; i < mapdata.GetLength(0); i++)
            {
                for (int j = 0; j < mapdata.GetLength(1); j++)
                {
                    switch (mapdata[i, j])
                    {
                        case (int)GimmickType.ICE:
                            {
                                Ice ice = new Ice(new Vector2(j * 64, i * 64), Vector2.Zero);
                                MapThings.Add(ice);
                                break;
                            }
                        case (int)GimmickType.IRON:
                            {
                                Iron iron = new Iron(new Vector2(j * 64, i * 64), Vector2.Zero);
                                MapThings.Add(iron);
                                break;
                            }
                        case (int)GimmickType.LIGHT:
                            {
                                Light light = new Light(new Vector2(j * 64, i * 64));
                                MapThings.Add(light);
                                break;
                            }
                        case (int)GimmickType.STRAW:
                            {
                                Straw straw = new Straw(new Vector2(j * 64, i * 64));
                                MapThings.Add(straw);
                                break;
                            }
                        case (int)GimmickType.COAL:
                            {
                                Coal coal = new Coal(new Vector2(j * 64, i * 64), Vector2.Zero);
                                MapThings.Add(coal);
                                break;
                            }
                        case (int)GimmickType.GOAL:
                            {
                                Goal goal = new Goal(new Vector2(j * 64, i * 64));
                                MapThings.Add(goal);
                                break;
                            }
                        case (int)GimmickType.WATER:
                            {
                                Water water = new Water(new Vector2(j * 64, i * 64), Vector2.Zero);
                                MapThings.Add(water);
                                break;
                            }
                        case (int)GimmickType.WOOD:
                            {
                                Wood wood = new Wood(new Vector2(j * 64, i * 64));
                                MapThings.Add(wood);
                                break;
                            }
                        case (int)GimmickType.TREE:
                            {
                                Tree tree = new Tree(new Vector2(j * 64, i * 64));
                                MapThings.Add(tree);
                                break;
                            }
                        case (int)GimmickType.SAND:
                            {
                                Sand sand = new Sand(new Vector2(j * 64, i * 64), Vector2.Zero);
                                MapThings.Add(sand);
                                break;
                            }
                        case (int)GimmickType.BALLOON:
                            {
                                Balloon balloon = new Balloon(new Vector2(j * 64, i * 64), Vector2.Zero);
                                MapThings.Add(balloon);
                                break;
                            }
                    }
                }
            }
        }

        /// <summary>
        /// ファイルからStageの情報を読み取る BY 氷見悠人　2016/10/31
        /// </summary>
        /// <param name="bigIndex"></param>
        /// <param name="smallIndex"></param>
        /// <returns></returns>
        public static int[,] GetMapdataFromFile(int bigIndex, int smallIndex)
        {
            int[,] mapdatas;

            string stagePath = StageDef.StagePath + bigIndex + "-" + smallIndex + ".csv";
            int width;
            List<string[]> lines = new List<string[]>();

            if (!File.Exists(stagePath))
            {
                if (File.Exists(StageDef.StagePath + "0-0.csv"))
                {
                    stagePath = StageDef.StagePath + "0-0.csv";
                    Console.WriteLine(string.Format("{0}がありません！", stagePath));
                }
                else
                {
                    throw new FileNotFoundException(string.Format("1つのStageもありません！！！　{0}の中をチェックしなさい！", StageDef.StagePath), StageDef.StagePath + "0-0.csv");
                }
            }

            using (StreamReader reader = new StreamReader(stagePath))
            {
                string line = reader.ReadLine();
                string[] lineSp;
                if (line != null)
                {
                    lineSp = line.Split(',');
                    width = lineSp.Length;
                }
                else
                {
                    throw new Exception(string.Format("Stage{0}-{1}の内容がありません！", bigIndex, smallIndex));
                }

                while (true)
                {
                    lines.Add(lineSp);
                    if (lineSp.Length != width)
                    {
                        throw new Exception(string.Format("Stage{0}-{1}の行の長さが一致していません！", bigIndex, smallIndex));
                    }
                    line = reader.ReadLine();
                    if (line != null && line != "")
                    {
                        lineSp = line.Split(',');
                    }
                    else
                    {
                        break;
                    }
                }

                mapdatas = new int[lines.Count, width];
                for (int i = 0; i < lines.Count; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        mapdatas[i, j] = int.Parse(lines[i][j]);
                    }
                }
            }
           
            return mapdatas;
        }
    }
}
