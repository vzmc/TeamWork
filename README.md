# TeamWork
こちらは2Dの横スクロールアクションパズルゲームです。</br>
「Release」フォルダー内はビルド済みの実行ファイルです。</br>
ゲーム内容の説明は「仕様書」フォルダー内のファイルをご覧ください。</br>
Stage設計は「Tiled」というソフトを使いました。「StageDesign」フォルダー内は設計のファイルがあります。</br>
</br>
制作チーム：</br>
　プランナ　　　　　　1名</br>
　メインプログラマー　1名（私）</br>
　プログラマー　　　　4名</br>
　グラフィック　　　　2名</br>
</br>
開発環境：　</br>　
  Visual Studio2015 + XNA 4.05 + C#</br>
 </br>
開発期間：</br>
　3ヶ月の中毎週2日、毎日8時間</br>
</br>
担当部分：</br>
　基盤とするプログラムの設計：</br>
　　Game基盤：Game1</br>
　　Scene管理：SceneManager、NextScene、SceneType</br>
　　Device管理：GameDevice、Camera、Input、Renderer、Sound、Animation、AnimationPlayer、BGMLoader、SELoader、TextureLoader、Timer</br>
　　Map管理：MapManager、Map、GimmickType、StageDef</br>
　　重要メソッド達：Method</br>
　　パラメーター管理：Parameter、FuncSwitch</br>
　　</br>
　ベースクラス、構造体の設計：</br>
　　IScene、GameObject、Loader、Size</br>
</br>
　一部Sceneの実装：</br>
　　Title、PlayScene、StageIn、Load</br>
　</br>
　一部GameObjectの実装：</br>
　　Player、Water、WaterLine、Fire、FireDust、FireEnergy、Light、Ice、Goal</br>
　</br>
　コード全体の管理、リファクタリング、Debug</br>
