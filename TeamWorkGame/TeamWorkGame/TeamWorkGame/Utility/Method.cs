////////////////////////////////////////////////////////////
// 重要のメソッド達
// 作成時間：2016/10/1
// 作成者：氷見悠人　
// 最終修正時間：2016/10/14
// 修正者：氷見悠人
/////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TeamWorkGame.Actor;

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
        public static bool MapObstacleCheck(ref Vector2 position, int width, int height, ref Vector2 velocity, ref bool isOnGround, Map map, int[] data)
        {
            bool flag = false;
            Vector2[] nowLRPoints = new Vector2[2];
            Vector2[] nowUDPoints = new Vector2[2];

            Vector2[] nextLRPoints = new Vector2[2];
            Vector2[] nextUDPoints = new Vector2[2];

            Vector2 blockPos = Vector2.Zero;

            //左右移動の判断
            if (velocity.X < 0)
            {
                nowLRPoints[0].X = position.X;
                nowLRPoints[0].Y = position.Y + 1;
                nowLRPoints[1].X = position.X;
                nowLRPoints[1].Y = position.Y + height - 1 - 1;

                nextLRPoints[0] = nowLRPoints[0] + new Vector2(velocity.X, 0);
                nextLRPoints[1] = nowLRPoints[1] + new Vector2(velocity.X, 0);

                if(map.IsInBlock(nextLRPoints[0], ref blockPos, data) || map.IsInBlock(nextLRPoints[1], ref blockPos, data))
                {
                    velocity.X = 0;
                    position.X = blockPos.X + map.BlockSize;
                    flag = true;
                }
            }
            else if(velocity.X > 0)
            {
                nowLRPoints[0].X = position.X + width - 1;
                nowLRPoints[0].Y = position.Y + 1;
                nowLRPoints[1].X = position.X + width - 1;
                nowLRPoints[1].Y = position.Y + height - 1 - 1;

                nextLRPoints[0] = nowLRPoints[0] + new Vector2(velocity.X, 0);
                nextLRPoints[1] = nowLRPoints[1] + new Vector2(velocity.X, 0);

                if (map.IsInBlock(nextLRPoints[0], ref blockPos, data) || map.IsInBlock(nextLRPoints[1], ref blockPos, data))
                {
                    velocity.X = 0;
                    position.X = blockPos.X - width;
                    flag = true;
                }
            }

            //上下移動の判断
            if(velocity.Y < 0)
            {
                nowUDPoints[0].X = position.X + 1;
                nowUDPoints[0].Y = position.Y;
                nowUDPoints[1].X = position.X + width - 1 - 1;
                nowUDPoints[1].Y = position.Y;

                nextUDPoints[0] = nowUDPoints[0] + new Vector2(0, velocity.Y);
                nextUDPoints[1] = nowUDPoints[1] + new Vector2(0, velocity.Y);

                if (map.IsInBlock(nextUDPoints[0], ref blockPos, data) || map.IsInBlock(nextUDPoints[1], ref blockPos, data))
                {
                    velocity.Y = 0;
                    position.Y = blockPos.Y + map.BlockSize;
                    flag = true;
                }
            }
            else if(velocity.Y > 0)
            {
                nowUDPoints[0].X = position.X + 1;
                nowUDPoints[0].Y = position.Y + height - 1;
                nowUDPoints[1].X = position.X + width - 1 - 1;
                nowUDPoints[1].Y = position.Y + height - 1;

                nextUDPoints[0] = nowUDPoints[0] + new Vector2(0, velocity.Y);
                nextUDPoints[1] = nowUDPoints[1] + new Vector2(0, velocity.Y);

                if (map.IsInBlock(nextUDPoints[0], ref blockPos, data) || map.IsInBlock(nextUDPoints[1], ref blockPos, data))
                {
                    velocity.Y = 0;
                    position.Y = blockPos.Y - height;
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

            // || (position2.X <= position1.X + width1 - 1 && position2.X + width2 - 1 >= position1.X)
            // || (position2.Y <= position1.Y + height1 - 1 && position2.Y + height2 - 1 >= position1.Y
            //無駄の判定を消した　BY氷見悠人
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
        public static bool ObstacleCheck(Actor.GameObject self, Actor.GameObject obstacle)
        {
            bool flag = false;

            Vector2 selfNowPositon = self.Position;
            Vector2 selfNowVelocity = self.Velocity;
            Vector2 selfNextPositionH = selfNowPositon + new Vector2(selfNowVelocity.X, 0);
            Vector2 selfNextPositionV = selfNowPositon + new Vector2(0, selfNowVelocity.Y);
            int selfWidth = self.ColSize.Width;
            int selfHeight = self.ColSize.Height;

            Vector2 obstaclePosition = obstacle.Position;
            int obstacleWidth = obstacle.ColSize.Width;
            int obstacleHeight = obstacle.ColSize.Height;

            //横方向の判定
            if (selfNowVelocity.X != 0)
            {
                if (CollisionCheck(selfNextPositionH, selfWidth, selfHeight, obstaclePosition, obstacleWidth, obstacleHeight))
                {
                    flag = true;

                    if (selfNowVelocity.X > 0)
                    {
                        self.PositionX = obstaclePosition.X - selfWidth;
                    }
                    else
                    {
                        self.PositionX = obstaclePosition.X + obstacleWidth;
                    }

                    self.VelocityX = 0;
                }
            }

            //縦方向の判定
            if (selfNowVelocity.Y != 0)
            {
                if (CollisionCheck(selfNextPositionV, selfWidth, selfHeight, obstaclePosition, obstacleWidth, obstacleHeight))
                {
                    flag = true;

                    if (selfNowVelocity.Y > 0)
                    {
                        self.PositionY = obstaclePosition.Y - selfHeight;
                        self.IsOnGround = true;
                    }
                    else
                    {
                        self.PositionY = obstaclePosition.Y + obstacleHeight;
                    }

                    self.VelocityY = 0;
                }
            }

            //if (flag)
            //{
            //    Console.WriteLine("!!!!!!!!!!!!!1");
            //}

            return flag;
        }
    }
}
