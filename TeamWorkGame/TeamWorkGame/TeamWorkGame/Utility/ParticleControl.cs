using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TeamWorkGame.Device;

namespace TeamWorkGame.Utility
{
    class ParticleControl
    {
        private Timer addTimer;
        private List<Particle> firworks;
        private Timer fireTimer;
        private bool fireSwitch;
        private float[,] fireVelocity = new float[,]{
            { 0,-1.5f}, { 0.5f,-1.2f }, { 1,-1 }, { 1.5f,-1.2f},
            { 1.5f,0}, { 0.5f,1.2f }, { 1,1 }, { 1.5f,1.2f },
            { 0,1.5f}, { -0.5f,1.2f }, { -1,1 }, { -1.5f,1.2f },
            { -1.5f,0}, { -0.5f,-1.2f }, { -1,-1 }, { -1.5f,-1.2f },
            };

        public ParticleControl()
        {
            addTimer = new Timer(0.2f);
            firworks = new List<Particle>();
            fireTimer = new Timer(1.0f);
            fireSwitch = false;
        }


        public void AddFireworks(Particle firework)
        {
            if (addTimer.IsTime())
            {
                firworks.Add(firework);
                addTimer.Initialize();
            }
        }

        public void Update()
        {
            addTimer.Update();
            firworks.ForEach(f =>
            {
                f.Update();
                if (f.IsDead) { fireSwitch = true; }
            });
            firworks.RemoveAll(f => f.IsDead);
            if (fireSwitch) { Fire(); }
        }


        private void Fire()
        {
            fireTimer.Update();
            for (int j = 0; j < fireVelocity.GetLength(0); j++)
            {
                AddFireworks(new Particle(3 * new Vector2(fireVelocity[j, 0], fireVelocity[j, 1])));
            }

            if (addTimer.IsTime())
            {
                addTimer.Initialize();
            }

            if (fireTimer.IsTime())
            {
                fireTimer.Initialize();
                fireSwitch = false;
            }
        }


        public void Draw(Renderer renderer)
        {
            firworks.ForEach(f => f.Draw(renderer));
        }
    }
}
