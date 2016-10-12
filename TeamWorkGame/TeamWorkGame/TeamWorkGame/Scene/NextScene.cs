using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TeamWorkGame.Scene
{
    struct NextScene
    {
        public SceneType sceneType;
        public int stageIndex;

        public NextScene(SceneType scene, int index)
        {
            sceneType = scene;
            stageIndex = index;
        }
    }
}
