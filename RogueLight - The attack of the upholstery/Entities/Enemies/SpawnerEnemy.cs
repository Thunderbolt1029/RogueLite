using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueLight___The_attack_of_the_upholstery
{
    class SpawnerEnemy : Enemy
    {
        float TimeUntilNextSpawn, SpawnTime;

        float AnimationPercentage = 0.1f;
        float CloseToSpawningTime { get => AnimationPercentage * SpawnTime; }
        float JustSpawnedTime { get => (1 - AnimationPercentage) * SpawnTime; }

        SpriteType.Spawner TextureState;
        public override Texture2D ActiveTexture { get => Textures[(int)TextureState]; }

        List<Texture2D> EnemyTextures = new List<Texture2D>();

        public SpawnerEnemy(List<Texture2D> SpawnerTextures, List<Texture2D> EnemyTextures, float SpawnTime, Vector2 Centre)
        {
            Textures = SpawnerTextures;
            this.EnemyTextures = EnemyTextures;

            TextureState = SpriteType.Spawner.Idle;

            this.SpawnTime = SpawnTime;
            TimeUntilNextSpawn = SpawnTime / 2;

            Health = 50;

            this.Centre = Centre;
        }

        public override void Update(GameTime gameTime)
        {
            if (TimeUntilNextSpawn > 0)
                TimeUntilNextSpawn -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (TimeUntilNextSpawn <= 0)
            {
                SpawnSwarmerEnemy();
                TimeUntilNextSpawn = SpawnTime;
            }

            if (TimeUntilNextSpawn < CloseToSpawningTime || TimeUntilNextSpawn > JustSpawnedTime)
            {
                if (TextureState == SpriteType.Spawner.Idle)
                    TextureState = SpriteType.Spawner.Spawning;
            }
            else if (TextureState == SpriteType.Spawner.Spawning)
                TextureState = SpriteType.Spawner.Idle;


            base.Update(gameTime);
        }

        void SpawnSwarmerEnemy()
        {
            Vector2 RandomClosePos = Centre + AngleToVector((float)Globals.random.NextDouble() * MathHelper.TwoPi);

            Globals.Entities.Add(new SwarmerEnemy(EnemyTextures, 0.3f * Globals.WindowScale, RandomClosePos, 1f + (float)((Globals.random.NextDouble() * 2 - 0.5) * 0.3)));
        }
    }
}
