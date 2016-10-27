// 最終修正時間：2016年10月27日
// By 佐瀬拓海

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;   //Assert用

namespace TeamWorkGame.Device
{
    public class Renderer
    {
        //フィールド
        private ContentManager contentManager;  //コンテンツ管理者
        private GraphicsDevice graphicsDevice;  //グラフィック機器
        private SpriteBatch spriteBatch;        //スプライト一括

        //Dictionaryで複数の画像を管理
        private static Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="content">Game1のコンテンツ管理者</param>
        /// <param name="graphics">Game1のグラフィック機器</param>
        public Renderer(ContentManager content, GraphicsDevice graphics)
        {
            contentManager = content;
            graphicsDevice = graphics;
            spriteBatch = new SpriteBatch(graphicsDevice);
        }

        /// <summary>
        /// 画像の読み込み
        /// </summary>
        /// <param name="name">アセット名</param>
        /// <param name="filepath">ファイルまでのパス</param>
        public void LoadTexture(string name, string filepath = "./")
        {
            //ガード節
            //Dictionaryへの２重登録を回避
            if (textures.ContainsKey(name))
            {
#if DEBUG   //DEBUGモードの時のみ有効
                Console.WriteLine("この" + name + "はKeyで、すでに登録しています");
#endif
                //処理終了
                return;
            }

            //画像の読み込みとDictionaryにアセット名と画像を追加
            textures.Add(name, contentManager.Load<Texture2D>(filepath + name));
        }

        /// <summary>
        /// アンロード
        /// </summary>
        public void Unload()
        {
            //Dictionary登録情報をクリア
            textures.Clear();
        }

        /// <summary>
        /// 描画開始
        /// </summary>
        public void Begin()
        {

            spriteBatch.Begin();
        }

        /// <summary>
        /// 描画終了
        /// </summary>
        public void End()
        {
            spriteBatch.End();
        }

        public static Texture2D GetTexture(string name)
        {
            Debug.Assert(
                textures.ContainsKey(name),
                "アセット名が間違えていませんか？\n" +
                "大文字小文字間違ってませんか？\n" +
                "LoadTextureで読み込んでますか？\n" +
                "プログラムを確認してください");
            return textures[name];
        }

        /// <summary>
        /// 描画処理
        /// </summary>
        /// <param name="name">アセット</param>
        /// <param name="position">描画位置</param>
        /// <param name="alpha">透明値（0.0fは完全透明、1.0fは完全不透明）</param>
        public void DrawTexture(string name, Vector2 position, float alpha = 1.0f)
        {
            //登録されているキーがなければエラー表示
            Debug.Assert(
                textures.ContainsKey(name),
                "アセット名が間違えていませんか？\n" +
                "大文字小文字間違ってませんか？\n" +
                "LoadTextureで読み込んでますか？\n" +
                "プログラムを確認してください");
            spriteBatch.Draw(textures[name], position, Color.White * alpha);
        }

        /// <summary>
        /// 画像の描画（指定範囲）
        /// </summary>
        /// <param name="name">アセット名</param>
        /// <param name="position">位置</param>
        /// <param name="rect">画像の切り出し範囲</param>
        /// <param name="alpha">透明値</param>
        public void DrawTexture(string name, Vector2 position, Rectangle rect, float alpha = 1.0f)
        {
            Debug.Assert(
                textures.ContainsKey(name),
                "アセット名が間違えていませんか？\n" +
                "大文字小文字間違ってませんか？\n" +
                "LoadTextureで読み込んでますか？\n" +
                "プログラムを確認してください");

            spriteBatch.Draw(
                textures[name], //画像
                position,       //位置
                rect,           //の指定範囲
                Color.White * alpha);
        }

        //by 長谷川修一 10/13
        /// <summary>
        /// 描画処理(拡大縮小)
        /// </summary>
        /// <param name="name">アセット</param>
        /// <param name="position">位置</param>
        /// <param name="scale">拡大率</param>
        /// <param name="alpha">透明度</param>
        public void DrawTexture(string name, Vector2 position, float scale, float alpha = 1.0f)
        {
            //登録されているキーがなければエラー表示
            Debug.Assert(
                textures.ContainsKey(name),
                "アセット名が間違えていませんか？\n" +
                "大文字小文字間違ってませんか？\n" +
                "LoadTextureで読み込んでますか？\n" +
                "プログラムを確認してください");

            spriteBatch.Draw(
                textures[name],
                position,
                new Rectangle(0, 0, textures[name].Width, textures[name].Height),
                Color.White * alpha,
                0.0f,
                Vector2.Zero,
                scale,
                SpriteEffects.None,
                0);
        }

        /// <summary>
        /// アニメーションの描画用の
        /// </summary>
        /// <param name="texture">"画像"</param>
        /// <param name="position">描画位置</param>
        /// <param name="range">切り出し範囲</param>
        /// <param name="spriteEffects">向き</param>
        /// <param name="alpha">透明値</param>
        /// <param name="rotation">回転角度</param>
        /// <param name="scale">スケール</param>
        public void DrawTexture(Texture2D texture, Vector2 position, Rectangle range, SpriteEffects spriteEffects, float alpha = 1.0f, float rotation = 0.0f, float scale = 1.0f)
        {
            //登録されているキーがなければエラー表示
            //Debug.Assert(
            //    textures.ContainsKey(name),
            //    "アセット名が間違えていませんか？\n" +
            //    "大文字小文字間違ってませんか？\n" +
            //    "loadtextureで読み込んでますか？\n" +
            //    "プログラムを確認してください");

            spriteBatch.Draw(texture, position, range, Color.White * alpha, rotation, Vector2.Zero, scale, spriteEffects, 0.0f);
        }

        /// <summary>
        /// 数字の描画（整数のみ版）
        /// </summary>
        /// <param name="name">アセット名</param>
        /// <param name="position">位置</param>
        /// <param name="number">表示したい数字（整数）</param>
        /// <param name="alpha">透明値</param>
        public void DrawNumber(string name, Vector2 position, int number, float alpha = 1.0f)
        {
            Debug.Assert(
                textures.ContainsKey(name),
                "アセット名が間違えていませんか？\n" +
                "大文字小文字間違ってませんか？\n" +
                "LoadTextureで読み込んでますか？\n" +
                "プログラムを確認してください");

            //マイナスの数は０
            if (number < 0)
            {
                number = 0;
            }

            foreach (var n in number.ToString())
            {
                spriteBatch.Draw(
                    textures[name],
                    position,
                    new Rectangle((n - '0') * 32, 0, 32, 64),
                    Color.White * alpha
                    );
                position.X += 32;
            }
        }

        /// <summary>
        /// 数字の描画（詳細版）
        /// </summary>
        /// <param name="name">アセット名</param>
        /// <param name="position">位置</param>
        /// <param name="number">表示したい数字（文字列でもらう）</param>
        /// <param name="digit">桁数</param>
        /// <param name="alpha">透明値</param>
        public void DrawNumber(string name, Vector2 position, string number, int digit, float alpha = 1.0f)
        {
            Debug.Assert(
                textures.ContainsKey(name),
                "アセット名が間違えていませんか？\n" +
                "大文字小文字間違ってませんか？\n" +
                "LoadTextureで読み込んでますか？\n" +
                "プログラムを確認してください");

            for (int i = 0; i < digit; i++)
            {
                if (number[i] == '.')
                {
                    //幅をかけて座標を求め、１文字を絵から切り出す
                    spriteBatch.Draw(
                        textures[name],
                        position,
                        new Rectangle(10 * 32, 0, 32, 64),
                        Color.White * alpha
                        );
                }
                else if(number[i] == '/')// "/"を描画する
                {
                    spriteBatch.Draw(
                        textures[name],
                        position,
                        new Rectangle(11 * 32, 0, 32, 64),
                        Color.White * alpha
                        );
                }
                else
                {
                    //１文字分の数値を数値文字で取得
                    char n = number[i];

                    //幅をかけて座標を求め、１文字を絵から切り出す
                    spriteBatch.Draw(
                        textures[name],
                        position,
                        new Rectangle((n - '0') * 32, 0, 32, 64),
                        Color.White * alpha
                        );
                }
                position.X += 32;
            }
        }
    }
}
