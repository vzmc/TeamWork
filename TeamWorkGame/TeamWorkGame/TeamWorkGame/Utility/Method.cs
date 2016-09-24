using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TeamWorkGame.Utility
{
    public static class Method
    {
        /// <summary>
        /// Mapの障害物との衝突判定
        /// </summary>
        /// <param name="position"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="velocity"></param>
        /// <param name="map"></param>
        /// <param name="data"></param>
        /// <returns></returns>
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
    }
}
