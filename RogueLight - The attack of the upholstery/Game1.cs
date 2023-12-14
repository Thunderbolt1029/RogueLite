using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Data.SQLite;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace RogueLight___The_attack_of_the_upholstery
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        // Some stuff needed for MonoGame
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        SoundEffect Talking, RatIsHere;

        //                                          Spawner, Swarmer, RandomShooter, Guard
        double[] EnemySpawnChances = new double[] { 0.4f,    1,       1,             0.8f };

        Run[] Runs;
        int TotalPastRunPages, PastRunsPage, RunsPerPage = 20;
        string EntryBoxContents = "";

        #region Texture Variables
        List<Texture2D> PlayerTextures = new List<Texture2D>();
        List<Texture2D> EnemyTextures = new List<Texture2D>();
        List<Texture2D> ChestTextures = new List<Texture2D>();
        List<Texture2D> SpawnerTextures = new List<Texture2D>();
        List<Texture2D> MimicTextures = new List<Texture2D>();
        Dictionary<GuardEnemy.State, Texture2D> GuardTextures = new Dictionary<GuardEnemy.State, Texture2D>();
        List<Texture2D> NailedClothTextures = new List<Texture2D>();

        Texture2D HeartFull, HeartEmpty;
        Texture2D GradientTexture;
        Texture2D RoomHighlight;
        Texture2D PlaceHolderTexture;

        Texture2D MainMenuBackground;
        Texture2D RatBackground;
        Texture2D Rat;
        #endregion

        #region Initialization of Monogame, Textures, and other variables
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            Globals.GameWindowWidth = Window.ClientBounds.Width;
            Globals.GameWindowHeight = Window.ClientBounds.Height;

            // Sets the screen to full screen if DebugMode is not enabled else set to 1296x864
            if (Globals.DebugModeEnabled)
            {
                graphics.PreferredBackBufferWidth = 1296; // 1080 / 1296
                graphics.PreferredBackBufferHeight = 864; // 720 / 864
                graphics.IsFullScreen = false;
            }
            else
            {
                graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
                graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
                graphics.HardwareModeSwitch = true;

                graphics.IsFullScreen = true;
                graphics.ApplyChanges();
            }
            Globals.GameWindowWidth = Window.ClientBounds.Width;
            Globals.GameWindowHeight = Window.ClientBounds.Height;

            graphics.ApplyChanges();

            // Sets mouse cursor to custom cursor
            Mouse.SetCursor(MouseCursor.FromTexture2D(Content.Load<Texture2D>("MouseCursor"), 15, 15));
            IsMouseVisible = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            Globals.GameWindowWidth = graphics.GraphicsDevice.Viewport.Width;
            Globals.GameWindowHeight = graphics.GraphicsDevice.Viewport.Height;

            base.Initialize();
        }


        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            Talking = Content.Load<SoundEffect>("SFX_Talking");
            RatIsHere = Content.Load<SoundEffect>("SFX_RatIsHere");

            RatBackground = Content.Load<Texture2D>("Misc_RatBackground");
            Rat = Content.Load<Texture2D>("Misc_Rat");

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Import fonts
            Fonts.Debug = new Font(Content.Load<SpriteFont>("Font_ComicSans"), Color.Lime);
            Fonts.Health = new Font(Content.Load<SpriteFont>("Font_ComicSans"), Color.Red);
            Fonts.Leaderboard = new Font(Content.Load<SpriteFont>("Font_ComicSans"), Color.White);
            Fonts.PreviousRun = new Font(Content.Load<SpriteFont>("Font_ComicSans"), Color.Black);
            Fonts.TitleButtons = new Font(Content.Load<SpriteFont>("Font_Himalaya"), Color.White, AlignmentType.BottomLeft, new Vector2(10, 0));

            //Fonts.IWasCrazyOnce = new Font(Content.Load<SpriteFont>("Font_Papyrus"), Color.Red);
            //Fonts.YoureCrazy = new Font(Content.Load<SpriteFont>("Font_Determination"), Color.White);

            // Import Player textures
            for (int i = 0; i < (int)SpriteType.Looking.Count * (int)CharacterType.Count; i++)
                PlayerTextures.Add(Content.Load<Texture2D>($"Player_{(CharacterType)(i / (int)SpriteType.Looking.Count)}Looking{(SpriteType.Looking)(i % (int)SpriteType.Looking.Count)}"));
            
            // Import Pickup textures
            for (int i = 0; i < Pickup.Pickups.Length; i++)
                for (int j = 0; j < Pickup.Pickups[i].Length; j++)
                    Pickup.PickupTextures.Add(Pickup.Pickups[i][j], Content.Load<Texture2D>($"Pickup_{Pickup.Pickups[i][j].Name}"));
            for (int i = 0; i < Pickup.SpecialPickups.Length; i++)
                Pickup.PickupTextures.Add(Pickup.SpecialPickups[i], Content.Load<Texture2D>($"Pickup_{Pickup.SpecialPickups[i].Name}"));


            // Import Enemy textures
            // Looking textures
            for (int i = 0; i < (int)SpriteType.Looking.Count; i++)
                EnemyTextures.Add(Content.Load<Texture2D>($"Enemy_Looking{(SpriteType.Looking)i}"));

            // Chest textures
            for (int i = 0; i < (int)SpriteType.Chest.Count; i++)
                ChestTextures.Add(Content.Load<Texture2D>($"Chest_{(SpriteType.Chest)i}"));

            // Spawner textures
            for (int i = 0; i < (int)SpriteType.Spawner.Count; i++)
                SpawnerTextures.Add(Content.Load<Texture2D>($"Enemy_Spawner{(SpriteType.Spawner)i}"));

            // Mimic texture
            MimicTextures.Add(Content.Load<Texture2D>("Boss_Mimic"));

            // Guard textures
            for (int i = 0; i < GuardEnemy.StatesCount; i++)
                GuardTextures.Add((GuardEnemy.State)i, Content.Load<Texture2D>($"Enemy_Guard{(GuardEnemy.State)i}"));

            // Nailed Cloth textures
            for (int i = 0; i < (int)NailedCloth.NailedClothState.Count; i++)
                NailedClothTextures.Add(Content.Load<Texture2D>($"Boss_NailedCloth{(NailedCloth.NailedClothState)i}"));


            // Import Weapon textures
            for (int i = 0; i < (int)WeaponType.Count; i++)
                try
                {
                    Weapon.WeaponTextures.Add((WeaponType)i, Content.Load<Texture2D>($"Weapon_{(WeaponType)i}"));
                }
                catch
                {
                    // remove this when actually added the other weapons <------------------------  <---------------------  <------------------  <---------------------------  <----------------------------------  <-------------------------  <------------------  <-------------  <----------
                }


            // Import Door Texture
            Door.Texture = Content.Load<Texture2D>("Room_Door");

            // Import each of the door textures
            for (int i = 0; i < (int)ProjectileType.Count; i++)
                Projectile.ProjectileTextures.Add((ProjectileType)i, Content.Load<Texture2D>($"Projectile_{(ProjectileType)i}"));

            // Import both heart empty and full textures
            HeartFull = Content.Load<Texture2D>("Health_HeartFull");
            HeartEmpty = Content.Load<Texture2D>("Health_HeartEmpty");

            // Import Boss health bar textures
            Globals.BossHealthBar[0] = Content.Load<Texture2D>("Health_BossBarEmpty");
            Globals.BossHealthBar[1] = Content.Load<Texture2D>("Health_BossBarFull");

            // Import the texture for a white to transparent gradient - could be done procedurally
            GradientTexture = Content.Load<Texture2D>("Misc_Gradient");

            // Import texture for the highlight overlay in the minimap
            RoomHighlight = Content.Load<Texture2D>("Misc_RoomHighlight");

            // Import the picture in the back of the main menu
            MainMenuBackground = Content.Load<Texture2D>("Menu_MainMenuBackground");

            // Setup Portal
            Portal.Texture = Content.Load<Texture2D>("Misc_Portal");
            Portal.LoadNewLevel = LoadNewLevel;

            // Import PlaceHolderTexture
            PlaceHolderTexture = Content.Load<Texture2D>("Misc_PlaceHolder");

            // Import music
            MediaPlayer.IsRepeating = true;

            Sounds.MainMenuTheme = Content.Load<Song>("Music_MainMenu");
            Sounds.MainTheme = Content.Load<Song>("Music_MainTheme");
            Sounds.DeathTheme = Content.Load<Song>("Music_Death");

            InitialiseGlobals();
            InitialiseSQL();

            MediaPlayer.Volume = 1.0f;
            MediaPlayer.Play(Sounds.MainMenuTheme);
        }

        /// <summary>
        /// Initialzes variables in the Globals static class
        /// </summary>
        void InitialiseGlobals()
        {
            // Creates a new 1x1 texture then sets it's colour to white
            Globals.ColorTexture = new Texture2D(GraphicsDevice, 1, 1);
            Globals.ColorTexture.SetData(new[] { Color.White });

            // Imports all the RoomTextures and puts them into the dictionary
            for (int i = 0; i <= Globals.TotalNoStages; i++)
                Globals.RoomTextures[i] = Content.Load<Texture2D>($"Misc_Level{i}Background");

            // Sets the Collisions for each RoomLayout e.g. walls or spikes
            Globals.RoomCollisionBoxes = new Rectangle[]
                {
                    // Four walls
                    new Rectangle(0, 0, 440, 50),
                    new Rectangle(640, 0, 440, 50),
                    new Rectangle(0, 670, 440, 50),
                    new Rectangle(640, 670, 440, 50),
                    new Rectangle(0, 0, 50, 260),
                    new Rectangle(0, 460, 50, 260),
                    new Rectangle(1030, 0, 50, 260),
                    new Rectangle(1030, 460, 50, 260)
                };

            // Scales the size of the Rectangles by the size of the window
            for (int i = 0; i < Globals.RoomCollisionBoxes.Length; i++)
            {
                Rectangle rect = new Rectangle();

                rect.X = Globals.GameWindowRectangle.Left + (int)(Globals.RoomCollisionBoxes[i].X * Globals.WindowScale);
                rect.Y = Globals.GameWindowRectangle.Top + (int)(Globals.RoomCollisionBoxes[i].Y * Globals.WindowScale);
                rect.Width = (int)(Globals.RoomCollisionBoxes[i].Width * Globals.WindowScale);
                rect.Height = (int)(Globals.RoomCollisionBoxes[i].Height * Globals.WindowScale);

                Globals.RoomCollisionBoxes[i] = rect;
            }

            // Generates the buttons for each of the gamestates
            Texture2D MainMenuButtonTexture = Content.Load<Texture2D>("Menu_MainMenuButton");
            Globals.GameStateButtons.Add(GameState.Menu, new Button[] {
                    new Button(SelectSaveGame, new Rectangle(20, 350, 350, 80), MainMenuButtonTexture, "Start", Fonts.TitleButtons),
                    new Button(PastRuns, new Rectangle(20, 450, 350, 80), MainMenuButtonTexture, "Past Runs", Fonts.TitleButtons),
                    new Button(Exit, new Rectangle(20, 550, 350, 80), MainMenuButtonTexture, "Quit", Fonts.TitleButtons),
                });
            Globals.GameStateButtons.Add(GameState.InGame, new Button[] {
                    // Only to be displayed in the lobby
                    new Button(() => ChangeCharacter(CharacterType.Guy), new Rectangle(10, 50, 120, 20), Color.Gray, "Guy", Fonts.Debug),
                    new Button(() => ChangeCharacter(CharacterType.GuyWithHat), new Rectangle(10, 75, 120, 20), Color.Gray, "Guy with hat", Fonts.Debug),
                    new Button(() => ChangeCharacter(CharacterType.GuyWithHelmet), new Rectangle(10, 100, 120, 20), Color.Gray, "Guy with helmet", Fonts.Debug),
                    new Button(() => ChangeCharacter(CharacterType.GuyWithFangs), new Rectangle(10, 125, 120, 20), Color.Gray, "Guy with fangs", Fonts.Debug),
                    
                    
                    new Button(() => ChangeWeapon(WeaponType.Pistol, 0), new Rectangle(10, 210, 55, 20), Color.Gray, "Pistol", Fonts.Debug),
                    new Button(() => ChangeWeapon(WeaponType.Dagger, 0), new Rectangle(10, 235, 55, 20), Color.Gray, "Dagger", Fonts.Debug),
                    //new Button(() => throw new NotImplementedException() /*ChangeWeapon(WeaponType.Whip, 0)*/, new Rectangle(10, 260, 55, 20), Color.Gray, "Whip", Fonts.Debug),

                    new Button(() => ChangeWeapon(WeaponType.Pistol, 1), new Rectangle(75, 210, 55, 20), Color.Gray, "Pistol", Fonts.Debug),
                    new Button(() => ChangeWeapon(WeaponType.Dagger, 1), new Rectangle(75, 235, 55, 20), Color.Gray, "Dagger", Fonts.Debug),
                    //new Button(() => throw new NotImplementedException() /*ChangeWeapon(WeaponType.Whip, 1)*/, new Rectangle(75, 260, 55, 20), Color.Gray, "Whip", Fonts.Debug),

                    new Button(() => ChangeWeapon(WeaponType.Bow), new Rectangle(10, 300, 120, 20), Color.Gray, "Bow", Fonts.Debug),
                    new Button(() => ChangeWeapon(WeaponType.Staff), new Rectangle(10, 325, 120, 20), Color.Gray, "Staff", Fonts.Debug),
                    new Button(() => ChangeWeapon(WeaponType.Axe), new Rectangle(10, 350, 120, 20), Color.Gray, "Axe", Fonts.Debug),

                    // Add rest of buttons down here
            });
            Globals.GameStateButtons.Add(GameState.Paused, new Button[] {
                    new Button(Unpause, new Rectangle(540, 200, 200, 100), Color.Gray, "Resume", Fonts.Debug),
                    new Button(() => ExitGame(false), new Rectangle(540, 350, 200, 100), Color.Gray, "Exit", Fonts.Debug)
                });
            Globals.GameStateButtons.Add(GameState.SaveGamesMenu, new Button[] {
                });
            Globals.GameStateButtons.Add(GameState.PastRuns, new Button[] {
                });

            Globals.CurrentGameState = GameState.Menu;

            // Again scales each of each BoundingBox by the size of the window
            foreach (Button[] buttons in Globals.GameStateButtons.Values)
                foreach (Button button in buttons)
                    button.BoundingBox = new Rectangle((int)(button.BoundingBox.X * Globals.WindowScale), (int)(button.BoundingBox.Y * Globals.WindowScale), (int)(button.BoundingBox.Width * Globals.WindowScale), (int)(button.BoundingBox.Height * Globals.WindowScale));
        }
        #endregion

        #region Update methods
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            UpdateInputs();

            if (Globals.SecretRoom)
            {
                SecretRoomUpdate(gameTime);
                return;
            }

            // Updates the game dependant on the current state
            switch (Globals.CurrentGameState)
            {
                case GameState.Menu:
                    UpdateMenu(gameTime);
                    break;

                case GameState.InGame:
                    UpdateGame(gameTime);
                    break;

                case GameState.Paused:
                    UpdatePauseMenu(gameTime);
                    break;

                case GameState.SaveGamesMenu:
                    UpdateSaveGamesMenu(gameTime);
                    break;

                case GameState.PastRuns:
                    UpdatePreviousRunMenu(gameTime);
                    break;
            }

            base.Update(gameTime);
        }

        /// <summary> 
        /// Updates the global variables for the current input states 
        /// </summary>
        void UpdateInputs()
        {
            Globals.PreviousKeyboardState = Globals.CurrentKeyboardState;
            Globals.CurrentKeyboardState = Keyboard.GetState();

            Globals.PreviousMouseState = Globals.CurrentMouseState;
            Globals.CurrentMouseState = Mouse.GetState();

            Globals.PreviousGamePadState = Globals.CurrentGamePadState;
            Globals.CurrentGamePadState = GamePad.GetState(PlayerIndex.One);
        }

        void UpdateGame(GameTime gameTime)
        {
            // Update each button that needs to be updated in the current GameState
            var buttons = Globals.Stage == 0 ? Globals.CurrentGameStateButtons : Globals.CurrentGameStateButtons.Skip(13);
            foreach (Button button in buttons)
                button.Update(gameTime);

            foreach (Component component in Globals.Components)
                component.Update(gameTime);

            // If player has died don't update until space button has been pressed
            if (Globals.YouDied || Globals.GameWon)
            {
                if (Globals.FirstFrameWonDeath)
                {
                    Globals.FirstFrameWonDeath = false;
                    SaveNewRun(Globals.CurrentSaveGameID, Globals.RunNo, Globals.RunTime, Globals.NoBossCoinsCollected, Globals.Score, Globals.GameWon);
                }
                if (Globals.CurrentKeyboardState.IsKeyDown(Keys.Escape) && Globals.PreviousKeyboardState.IsKeyUp(Keys.Escape))
                {
                    ExitGame(true);
                    StartGame();
                }
                return;
            }

            // Pauses when escape key has been pressed
            if (Globals.CurrentKeyboardState.IsKeyDown(Keys.Escape) && Globals.PreviousKeyboardState.IsKeyUp(Keys.Escape)) // <------------------------- change this to pause screen at some point
                Pause();

            // Update all entities
            for (int i = 0; i < Globals.Entities.Count; i++)
                Globals.Entities[i].Update(gameTime);

            // Updates BuyableItems if in a shop room
            if (Globals.CurrentRoom.Type == RoomType.Shop)
                foreach (BuyableItem buyableItem in Globals.CurrentRoom.BuyableItems)
                    buyableItem.Update(gameTime);

            foreach (Pickup pickup in Globals.CurrentRoom.Pickups)
                pickup.Update(gameTime);

            Globals.CurrentRoom.Pickups.RemoveAll(x => x.PickedUp);

            // Remove entities from the list if need to be removed
            Globals.Entities.RemoveAll(x => x.isRemoved);

            Globals.Components.RemoveAll(x => x is Animation && (x as Animation).ToRemove);
            Globals.Components.RemoveAll(x => x is NotificationPopup && (x as NotificationPopup).ToRemove);

            // Removes closed doors so the player can leave the room after all the enemies have been killed
            if (Globals.CountEnemiesInCurrentRoom == 0 && !Globals.CurrentRoom.Cleared)
                OnRoomClear();

            // Check whether the player is close enough to the edge of the screen to activate the movement between rooms
            if (Globals.player.CheckMoveRoom(out MoveDirection? direction))
                MoveBetweenRooms((MoveDirection)direction);
        }

        void UpdateMenu(GameTime gameTime)
        {
            // Quit game if escape has been pressed
            if (Globals.CurrentKeyboardState.IsKeyDown(Keys.Escape) && Globals.PreviousKeyboardState.IsKeyUp(Keys.Escape))
                Exit();

            // Update each button that needs to be updated in the current GameState
            foreach (Button button in Globals.CurrentGameStateButtons)
                button.Update(gameTime);
        }

        void UpdatePauseMenu(GameTime gameTime)
        {
            if (Globals.CurrentKeyboardState.IsKeyDown(Keys.Escape) && Globals.PreviousKeyboardState.IsKeyUp(Keys.Escape))
                Unpause();

            // Update each button that needs to be updated in the current GameState
            foreach (Button button in Globals.CurrentGameStateButtons)
                button.Update(gameTime);
        }

        void UpdateSaveGamesMenu(GameTime gameTime)
        {
            // Quit game if escape has been pressed
            if (Globals.CurrentKeyboardState.IsKeyDown(Keys.Escape) && Globals.PreviousKeyboardState.IsKeyUp(Keys.Escape))
                ExitToMenuWithoutReset();

            // Update each button that needs to be updated in the current GameState
            foreach (Button button in Globals.CurrentGameStateButtons)
                button.Update(gameTime);
        }

        void UpdatePreviousRunMenu(GameTime gameTime)
        {
            // Escape to menu if escape has been pressed
            if (Globals.CurrentKeyboardState.IsKeyDown(Keys.Escape) && Globals.PreviousKeyboardState.IsKeyUp(Keys.Escape))
                ExitToMenuWithoutReset();

            // Update each button that needs to be updated in the current GameState
            foreach (Button button in Globals.CurrentGameStateButtons)
                button.Update(gameTime);

            foreach (Component component in Globals.Components)
                component.Update(gameTime);

            if (Globals.Components.Count > 0 && (Globals.Components.First() as EntryBox).Text != EntryBoxContents)
            {
                EntryBoxContents = (Globals.Components.First() as EntryBox).Text;
                ReloadPastRunPage();
            }
        }

        #endregion

        #region Draw methods
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            // Clear previous frame
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();

            if (Globals.SecretRoom)
                SecretRoomDraw();
            else
                // Draws the correct view for the current gamestate
                switch (Globals.CurrentGameState)
                {
                    case GameState.Menu:
                        DrawMenu();
                        break;

                    case GameState.InGame:
                        DrawGame();
                        break;

                    case GameState.Paused:
                        DrawPauseMenu();
                        break;

                    case GameState.SaveGamesMenu:
                        DrawSaveGamesMenu();
                        break;

                    case GameState.PastRuns:
                        DrawPreviousRunMenu();
                        break;
                }

            spriteBatch.End();
            base.Draw(gameTime);
        }

        void DrawGame()
        {
            spriteBatch.Draw(Globals.ColorTexture, Globals.ScreenRectangle, new Color(15, 15, 15));

            // Draws Background
            spriteBatch.Draw(Globals.RoomTextures[Globals.Stage] ?? Globals.RoomTextures[Globals.TotalNoStages + 1], Globals.GameWindowRectangle, Color.White);

            // Draws each door
            foreach (Door door in Globals.ActiveDoors)
                door.Draw(spriteBatch);

            // Draws BuyableItems if in a shop room
            if (Globals.CurrentRoom.Type == RoomType.Shop)
                foreach (BuyableItem buyableItem in Globals.CurrentRoom.BuyableItems)
                    buyableItem.Draw(spriteBatch);

            foreach (Pickup pickup in Globals.CurrentRoom.Pickups)
                pickup.Draw(spriteBatch);

            // Draws each entity
            foreach (Entity entity in Globals.Entities)
                entity.Draw(spriteBatch);

            // Draws Hitboxes and the like when Debug mode is enabled
            if (Globals.DebugModeEnabled)
            {
                // Room Collision Boxes
                foreach (Rectangle CollisionBox in Globals.RoomCollisionBoxes)
                    spriteBatch.Draw(Globals.ColorTexture, CollisionBox, Color.Blue);

                // Door Collision Boxes
                foreach (Door door in Globals.ActiveDoors)
                    spriteBatch.Draw(Globals.ColorTexture, door.CollisionBox, Color.DarkBlue);

                // Draw the entity collison boxes
                foreach (Entity entity in Globals.Entities)
                {
                    Color color;

                    if (entity is Player)
                        color = Color.Goldenrod; // Draw the player with a gold hitbox
                    else if (entity is Enemy)
                        color = Color.Red; // Draw enemies with a red hitbox
                    else
                        color = Color.Fuchsia; // Draw any other entity (usually projectiles) with a fuschia hitbox

                    // Draw the hitbox with the current colour to the screen
                    spriteBatch.Draw(Globals.ColorTexture, entity.HitBox, color);
                }

                // Centre Lines
                spriteBatch.Draw(Globals.ColorTexture, new Rectangle(Globals.GameWindowRectangle.Width / 2, 0, 1, Globals.GameWindowRectangle.Height), Color.Red);
                spriteBatch.Draw(Globals.ColorTexture, new Rectangle(0, Globals.GameWindowRectangle.Height / 2, Globals.GameWindowRectangle.Width, 1), Color.Red);
            }



            #region --- UI ---
            // Red screen flash on take damage
            float Opacity = Globals.player.TimeSinceDamageTake / 0.2f;
            int Size = (int)(50 * Globals.WindowScale);
            int SizeOnTwo = Size / 2;
            int MaxScreenSize = Math.Max(Globals.GameWindowWidth, Globals.GameWindowHeight);
            Vector2 Origin = new Vector2(128f, 0.5f);
            Rectangle SourceRectangle = new Rectangle(Math.Min((int)(Opacity * 256), 256), 0, 256, 1);

            if (Globals.player.HeartsRemaining <= 1)
            {
                // Red outline always when at low health
                spriteBatch.Draw(GradientTexture, new Rectangle(Globals.ScreenRectangle.Left + SizeOnTwo, Globals.ScreenRectangle.Center.Y, Size, MaxScreenSize), null, Color.Red, MathHelper.PiOver2 * 0, Origin, SpriteEffects.None, 0f);  // Left
                spriteBatch.Draw(GradientTexture, new Rectangle(Globals.ScreenRectangle.Center.X, Globals.ScreenRectangle.Top + SizeOnTwo, Size, MaxScreenSize), null, Color.Red, MathHelper.PiOver2 * 1, Origin, SpriteEffects.None, 0f);    // Top
                spriteBatch.Draw(GradientTexture, new Rectangle(Globals.ScreenRectangle.Right - SizeOnTwo, Globals.ScreenRectangle.Center.Y, Size, MaxScreenSize), null, Color.Red, MathHelper.PiOver2 * 2, Origin, SpriteEffects.None, 0f); // Right
                spriteBatch.Draw(GradientTexture, new Rectangle(Globals.ScreenRectangle.Center.X, Globals.ScreenRectangle.Bottom - SizeOnTwo, Size, MaxScreenSize), null, Color.Red, MathHelper.PiOver2 * 3, Origin, SpriteEffects.None, 0f); // Bottom
            }
            else
            {
                // Red flash on taking damage
                spriteBatch.Draw(GradientTexture, new Rectangle(Globals.ScreenRectangle.Left + SizeOnTwo, Globals.ScreenRectangle.Center.Y, Size, MaxScreenSize), SourceRectangle, Color.Red, MathHelper.PiOver2 * 0, Origin, SpriteEffects.None, 0f);  // Left
                spriteBatch.Draw(GradientTexture, new Rectangle(Globals.ScreenRectangle.Center.X, Globals.ScreenRectangle.Top + SizeOnTwo, Size, MaxScreenSize), SourceRectangle, Color.Red, MathHelper.PiOver2 * 1, Origin, SpriteEffects.None, 0f);    // Top
                spriteBatch.Draw(GradientTexture, new Rectangle(Globals.ScreenRectangle.Right - SizeOnTwo, Globals.ScreenRectangle.Center.Y, Size, MaxScreenSize), SourceRectangle, Color.Red, MathHelper.PiOver2 * 2, Origin, SpriteEffects.None, 0f); // Right
                spriteBatch.Draw(GradientTexture, new Rectangle(Globals.ScreenRectangle.Center.X, Globals.ScreenRectangle.Bottom - SizeOnTwo, Size, MaxScreenSize), SourceRectangle, Color.Red, MathHelper.PiOver2 * 3, Origin, SpriteEffects.None, 0f); // Bottom
            }

            if (Globals.Stage != 0)
                // Draw the minimap in the top right of the screen
                DrawMiniMap(new Rectangle(Globals.GameWindowWidth - 10 * 20 - 10, 10, 20, 20));

            // Dash UI
            Rectangle OutRect;
            float t = 1 - MathHelper.Clamp(Globals.player.DashReloadTimeRemaining / Globals.player.DashReloadTime, 0, 1);
            if (t == 0 || t == 1) OutRect = new Rectangle(256, 0, 1, 1);
            else if (t == 0.5) OutRect = new Rectangle(0, 0, 256, 1);
            else OutRect = new Rectangle((int)((t >= 0.5 && t <= 1) ? (t - 0.5) * 2 * 256 : 0), 0, (int)((t >= 0 && t <= 0.5) ? 2 * t * 256 : 256), 1);
            spriteBatch.Draw(GradientTexture, new Rectangle(0, 0, (int)Globals.player.TotalHearts * 47 + 20, 75), OutRect, Color.LightBlue);
            if (Globals.DebugModeEnabled)
            {
                float DTR = Globals.player.DashReloadTimeRemaining;
                spriteBatch.DrawString(Fonts.Debug.SpriteFont, DTR == 0 ? "Dash Ready!" : Math.Round(DTR * 1000).ToString(), new Vector2(Globals.player.TotalHearts * 47 + 30, 25), Fonts.Debug.Color);
            }

            // Health UI
            for (int i = 0; i < Globals.player.TotalHearts; i++)
            {
                Texture2D texture;
                if (Globals.player.HeartsRemaining > i)
                    texture = HeartFull;
                else
                    texture = HeartEmpty;

                spriteBatch.Draw(texture, new Rectangle(10 + i * 47, 10, 55, 55), Color.White);
            }

            // Weapon UI
            if (Globals.player.EquippedWeapons[0] != null && Globals.player.EquippedWeapons[0] is TwoHandedWeapon)
            {
                TwoHandedWeapon weapon = Globals.player.EquippedWeapons[0] as TwoHandedWeapon;

                spriteBatch.Draw(PlaceHolderTexture, new Rectangle(10, Globals.ScreenRectangle.Bottom - 60, 50, 50), Color.White);
                spriteBatch.Draw(weapon.texture, new Rectangle(15, Globals.ScreenRectangle.Bottom - 55, 40, 40), Color.White);
                spriteBatch.Draw(GradientTexture, new Rectangle(10, Globals.ScreenRectangle.Bottom - 60, 50, 50), new Rectangle((int)Math.Round((1 - weapon.MainAttackReloadTimeRemaining / weapon.MainAttackReloadTime) * 256), 0, 1, 1), Color.White);

                spriteBatch.Draw(PlaceHolderTexture, new Rectangle(70, Globals.ScreenRectangle.Bottom - 60, 50, 50), Color.White);
                spriteBatch.Draw(weapon.texture, new Rectangle(75, Globals.ScreenRectangle.Bottom - 55, 40, 40), Color.White);
                spriteBatch.Draw(GradientTexture, new Rectangle(70, Globals.ScreenRectangle.Bottom - 60, 50, 50), new Rectangle((int)Math.Round((1 - weapon.SecondaryAttackReloadTimeRemaining / weapon.SecondaryAttackReloadTime) * 256), 0, 1, 1), Color.White);
            }
            else
            {
                if (Globals.player.EquippedWeapons[0] != null && Globals.player.EquippedWeapons[0] is SingleHandedWeapon)
                {
                    SingleHandedWeapon weapon = Globals.player.EquippedWeapons[0] as SingleHandedWeapon;

                    spriteBatch.Draw(PlaceHolderTexture, new Rectangle(10, Globals.ScreenRectangle.Bottom - 60, 50, 50), Color.White);
                    spriteBatch.Draw(weapon.texture, new Rectangle(15, Globals.ScreenRectangle.Bottom - 55, 40, 40), Color.White);
                    spriteBatch.Draw(GradientTexture, new Rectangle(10, Globals.ScreenRectangle.Bottom - 60, 50, 50), new Rectangle((int)Math.Round((1 - weapon.AttackReloadTimeRemaining / weapon.AttackReloadTime) * 256), 0, 1, 1), Color.White);
                }
                if (Globals.player.EquippedWeapons[1] != null && Globals.player.EquippedWeapons[1] is SingleHandedWeapon)
                {
                    SingleHandedWeapon weapon = Globals.player.EquippedWeapons[1] as SingleHandedWeapon;

                    spriteBatch.Draw(PlaceHolderTexture, new Rectangle(70, Globals.ScreenRectangle.Bottom - 60, 50, 50), Color.White);
                    spriteBatch.Draw(weapon.texture, new Rectangle(75, Globals.ScreenRectangle.Bottom - 55, 40, 40), Color.White);
                    spriteBatch.Draw(GradientTexture, new Rectangle(70, Globals.ScreenRectangle.Bottom - 60, 50, 50), new Rectangle((int)Math.Round((1 - weapon.AttackReloadTimeRemaining / weapon.AttackReloadTime) * 256), 0, 1, 1), Color.White);
                }
            }
            #endregion

            // Draw all of the components
            foreach (Component component in Globals.Components)
                component.Draw(spriteBatch);

            // Draw you died bar
            if (Globals.YouDied)
            {
                spriteBatch.Draw(Globals.ColorTexture, new Rectangle(0, 0, Globals.GameWindowWidth, Globals.GameWindowHeight), new Color(0, 0, 0, 150)); // "Grey out" the background

                spriteBatch.DrawString(Fonts.TitleButtons.SpriteFont, "You Lost !", Globals.GameWindowRectangle.Center.ToVector2() - Fonts.TitleButtons.SpriteFont.MeasureString("You Lost !") / 2 - new Vector2(0, 200), Fonts.TitleButtons.Color); // Big you win title

                // Prepare strings to print to the screen
                string RunNo = $"Run number: {Globals.RunNo}";
                string RunTime = $"Run time: {Globals.RunTime.Minutes}:{Globals.RunTime.Seconds}.{Globals.RunTime.Milliseconds}";
                string NoBossCoinsCollected = $"Boss Coins Collected: {Globals.NoBossCoinsCollected}";
                string TotalBossCoins = $"Total Boss Coins: {Globals.NoBossCoins}";
                string Score = $"Score: {Globals.Score}";

                // Print strings to the screen in their respective places
                spriteBatch.DrawString(Fonts.Leaderboard.SpriteFont, RunNo, Globals.GameWindowRectangle.Center.ToVector2() - Fonts.Leaderboard.SpriteFont.MeasureString(RunNo) / 2 - new Vector2(0, 125), Fonts.Leaderboard.Color);
                spriteBatch.DrawString(Fonts.Leaderboard.SpriteFont, RunTime, Globals.GameWindowRectangle.Center.ToVector2() - Fonts.Leaderboard.SpriteFont.MeasureString(RunTime) / 2 - new Vector2(0, 100), Fonts.Leaderboard.Color);
                spriteBatch.DrawString(Fonts.Leaderboard.SpriteFont, NoBossCoinsCollected, Globals.GameWindowRectangle.Center.ToVector2() - Fonts.Leaderboard.SpriteFont.MeasureString(NoBossCoinsCollected) / 2 - new Vector2(0, 75), Fonts.Leaderboard.Color);
                spriteBatch.DrawString(Fonts.Leaderboard.SpriteFont, TotalBossCoins, Globals.GameWindowRectangle.Center.ToVector2() - Fonts.Leaderboard.SpriteFont.MeasureString(TotalBossCoins) / 2 - new Vector2(0, 50), Fonts.Leaderboard.Color);
                spriteBatch.DrawString(Fonts.Leaderboard.SpriteFont, Score, Globals.GameWindowRectangle.Center.ToVector2() - Fonts.Leaderboard.SpriteFont.MeasureString(Score) / 2 - new Vector2(0, 25), Fonts.Leaderboard.Color);
            }

            // Draw win screen
            if (Globals.GameWon)
            {
                spriteBatch.Draw(Globals.ColorTexture, new Rectangle(0, 0, Globals.GameWindowWidth, Globals.GameWindowHeight), new Color(0, 0, 0, 150)); // "Grey out" the background

                spriteBatch.DrawString(Fonts.TitleButtons.SpriteFont, "You Won !", Globals.GameWindowRectangle.Center.ToVector2() - Fonts.TitleButtons.SpriteFont.MeasureString("You Won !") / 2 - new Vector2(0, 200), Fonts.TitleButtons.Color); // Big you win title

                // Prepare strings to print to the screen
                string RunNo = $"Run number: {Globals.RunNo}";
                string RunTime = $"Run time: {Globals.RunTime.Minutes}:{Globals.RunTime.Seconds}.{Globals.RunTime.Milliseconds}";
                string NoBossCoinsCollected = $"Boss Coins Collected: {Globals.NoBossCoinsCollected}";
                string TotalBossCoins = $"Total Boss Coins: {Globals.NoBossCoins}";
                string Score = $"Score: {Globals.Score}";

                // Print strings to the screen in their respective places
                spriteBatch.DrawString(Fonts.Leaderboard.SpriteFont, RunNo, Globals.GameWindowRectangle.Center.ToVector2() - Fonts.Leaderboard.SpriteFont.MeasureString(RunNo) / 2 - new Vector2(0, 125), Fonts.Leaderboard.Color);
                spriteBatch.DrawString(Fonts.Leaderboard.SpriteFont, RunTime, Globals.GameWindowRectangle.Center.ToVector2() - Fonts.Leaderboard.SpriteFont.MeasureString(RunTime) / 2 - new Vector2(0, 100), Fonts.Leaderboard.Color);
                spriteBatch.DrawString(Fonts.Leaderboard.SpriteFont, NoBossCoinsCollected, Globals.GameWindowRectangle.Center.ToVector2() - Fonts.Leaderboard.SpriteFont.MeasureString(NoBossCoinsCollected) / 2 - new Vector2(0, 75), Fonts.Leaderboard.Color);
                spriteBatch.DrawString(Fonts.Leaderboard.SpriteFont, TotalBossCoins, Globals.GameWindowRectangle.Center.ToVector2() - Fonts.Leaderboard.SpriteFont.MeasureString(TotalBossCoins) / 2 - new Vector2(0, 50), Fonts.Leaderboard.Color);
                spriteBatch.DrawString(Fonts.Leaderboard.SpriteFont, Score, Globals.GameWindowRectangle.Center.ToVector2() - Fonts.Leaderboard.SpriteFont.MeasureString(Score) / 2 - new Vector2(0, 25), Fonts.Leaderboard.Color);
            }

            // Draw all of the buttons for this gamestate
            var buttons = Globals.Stage == 0 ? Globals.CurrentGameStateButtons : Globals.CurrentGameStateButtons.Skip(13);
            foreach (Button button in buttons)
                button.Draw(spriteBatch);
        }

        void DrawMenu()
        {
            // Draw background
            spriteBatch.Draw(MainMenuBackground, new Rectangle(0, 0, Globals.ScreenRectangle.Width, Globals.ScreenRectangle.Height), Color.White);

            // Draw all of the buttons for this gamestate
            foreach (Button button in Globals.CurrentGameStateButtons)
                button.Draw(spriteBatch);
        }

        void DrawPauseMenu()
        {
            DrawGame();

            spriteBatch.Draw(Globals.ColorTexture, new Rectangle(0, 0, Globals.GameWindowWidth, Globals.GameWindowHeight), new Color(0, 0, 0, 150)); // "Grey out" the background

            Vector2 padding = new Vector2(20, 20);
            Vector2 Spacing = new Vector2(10, 10);
            int DrawAmountY = 5;
            List<Pickup> PlayerNormalPickups = Globals.player.Pickups.Where(pickup => !pickup.Special).ToList();
            for (int i = 0; i < PlayerNormalPickups.Count; i++)
            {
                Pickup pickup = PlayerNormalPickups[i];
                Vector2 pos = new Vector2(padding.X + i / DrawAmountY * 64 + i / DrawAmountY * padding.X, padding.Y + i % DrawAmountY * 64 + i % DrawAmountY * padding.Y);

                spriteBatch.Draw(pickup.Texture, pos, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            }


            foreach (Button button in Globals.CurrentGameStateButtons)
                button.Draw(spriteBatch);
        }

        void DrawSaveGamesMenu()
        {
            // Draw all of the buttons for this gamestate
            foreach (Button button in Globals.CurrentGameStateButtons)
                button.Draw(spriteBatch);
        }

        void DrawPreviousRunMenu()
        {
            spriteBatch.Draw(Globals.ColorTexture, Globals.ScreenRectangle, new Color(79, 79, 79));
            spriteBatch.Draw(Globals.ColorTexture, new Rectangle(new Point(15, 15), (Globals.ScreenRectangle.Size.ToVector2() - new Vector2(30)).ToPoint()), Color.LightGray);

            // Draw all of the buttons for this gamestate
            foreach (Button button in Globals.CurrentGameStateButtons)
                button.Draw(spriteBatch);

            foreach (Component component in Globals.Components)
                component.Draw(spriteBatch);

            float XSeperation = ((float)Globals.ScreenRectangle.Width - 40) / 6f;

            spriteBatch.Draw(Globals.ColorTexture, new Rectangle(20, 150, Globals.ScreenRectangle.Width - 40, 1), Color.Black);
            for (int i = 0; i < 7; i++)
                spriteBatch.Draw(Globals.ColorTexture, new Rectangle((int)Math.Round(20 + i * XSeperation), 90, 1, Globals.ScreenRectangle.Height - 110), Color.Black);
            
            spriteBatch.DrawString(Fonts.PreviousRun.SpriteFont, "SaveGameID", new Vector2(20 + XSeperation / 2, 120) - Fonts.PreviousRun.SpriteFont.MeasureString("SaveGameID") / 2, Color.Black);
            spriteBatch.DrawString(Fonts.PreviousRun.SpriteFont, "RunNo", new Vector2(20 + XSeperation * 3 / 2, 120) - Fonts.PreviousRun.SpriteFont.MeasureString("RunNo") / 2, Color.Black);
            spriteBatch.DrawString(Fonts.PreviousRun.SpriteFont, "RunTime", new Vector2(20 + XSeperation * 5 / 2, 120) - Fonts.PreviousRun.SpriteFont.MeasureString("RunTime") / 2, Color.Black);
            spriteBatch.DrawString(Fonts.PreviousRun.SpriteFont, "NoBossCoinsCollected", new Vector2(20 + XSeperation * 7 / 2, 120) - Fonts.PreviousRun.SpriteFont.MeasureString("NoBossCoinsCollected") / 2, Color.Black);
            spriteBatch.DrawString(Fonts.PreviousRun.SpriteFont, "Score", new Vector2(20 + XSeperation * 9 / 2, 120) - Fonts.PreviousRun.SpriteFont.MeasureString("Score") / 2, Color.Black);
            spriteBatch.DrawString(Fonts.PreviousRun.SpriteFont, "RunWon?", new Vector2(20 + XSeperation * 11 / 2, 120) - Fonts.PreviousRun.SpriteFont.MeasureString("RunWon?") / 2, Color.Black);

            for (int i = 0; i < Math.Min(Runs.Skip(RunsPerPage * (PastRunsPage - 1)).Count(), RunsPerPage); i++)
            {
                spriteBatch.DrawString(Fonts.PreviousRun.SpriteFont, Runs[i + RunsPerPage * (PastRunsPage - 1)].SaveGameID.ToString(), new Vector2(20 + XSeperation / 2, 180 + i * 40) - Fonts.PreviousRun.SpriteFont.MeasureString(Runs[i + RunsPerPage * (PastRunsPage - 1)].SaveGameID.ToString()) / 2, Color.Black);
                spriteBatch.DrawString(Fonts.PreviousRun.SpriteFont, Runs[i + RunsPerPage * (PastRunsPage - 1)].RunNo.ToString(), new Vector2(20 + XSeperation * 3 / 2, 180 + i * 40) - Fonts.PreviousRun.SpriteFont.MeasureString(Runs[i + RunsPerPage * (PastRunsPage - 1)].RunNo.ToString()) / 2, Color.Black);
                spriteBatch.DrawString(Fonts.PreviousRun.SpriteFont, Runs[i + RunsPerPage * (PastRunsPage - 1)].RunTime.ToString(), new Vector2(20 + XSeperation * 5 / 2, 180 + i * 40) - Fonts.PreviousRun.SpriteFont.MeasureString(Runs[i + RunsPerPage * (PastRunsPage - 1)].RunTime.ToString()) / 2, Color.Black);
                spriteBatch.DrawString(Fonts.PreviousRun.SpriteFont, Runs[i + RunsPerPage * (PastRunsPage - 1)].NoBossCoinsCollected.ToString(), new Vector2(20 + XSeperation * 7 / 2, 180 + i * 40) - Fonts.PreviousRun.SpriteFont.MeasureString(Runs[i + RunsPerPage * (PastRunsPage - 1)].NoBossCoinsCollected.ToString()) / 2, Color.Black);
                spriteBatch.DrawString(Fonts.PreviousRun.SpriteFont, Runs[i + RunsPerPage * (PastRunsPage - 1)].Score.ToString(), new Vector2(20 + XSeperation * 9 / 2, 180 + i * 40) - Fonts.PreviousRun.SpriteFont.MeasureString(Runs[i + RunsPerPage * (PastRunsPage - 1)].Score.ToString()) / 2, Color.Black);
                spriteBatch.DrawString(Fonts.PreviousRun.SpriteFont, Runs[i + RunsPerPage * (PastRunsPage - 1)].RunWon ? "Yes" : "No", new Vector2(20 + XSeperation * 11 / 2, 180 + i * 40) - Fonts.PreviousRun.SpriteFont.MeasureString(Runs[i + RunsPerPage * (PastRunsPage - 1)].RunWon ? "Yes" : "No") / 2, Color.Black);
            }
        }

        void DrawMiniMap(Rectangle MiniMapPosition)
        {
            // Loop through every x,y coordinate of the Minmap
            for (int y = 0; y < 10; y++)
            {
                for (int x = 0; x < 10; x++)
                {
                    // Set the colour of the pixel based on the type of room
                    Color color;
                    switch (Globals.Map[x, y].Type)
                    {
                        case RoomType.Fighting:
                            color = Color.Red;
                            break;
                        case RoomType.Chest:
                            color = Color.Blue;
                            break;
                        case RoomType.Mimic:
                            color = Color.DarkBlue;
                            break;
                        case RoomType.Shop:
                            color = Color.Goldenrod;
                            break;
                        case RoomType.Boss:
                            color = Color.Black;
                            break;
                        case RoomType.Start:
                            color = Color.Magenta;
                            break;
                        default:
                            color = Color.Transparent;
                            break;
                    }

                    // If debug mode is enabled then set the transparent parts to a checkerboard pattern
                    if (Globals.DebugModeEnabled)
                        if (color == Color.Transparent)
                            color = (x + y) % 2 == 0 ? new Color(0, 0, 0, 100) : new Color(255, 255, 255, 100);

                    if (GetNeighbourRooms(Globals.Map, new Point(x, y)).Any(z => z != null && z.Cleared) || new Point(x, y) == new Point(5, 5))
                    {
                        // Draw each pixel scaled 20x
                        spriteBatch.Draw(Globals.ColorTexture, new Rectangle(x * 20 + MiniMapPosition.X, y * 20 + MiniMapPosition.Y, MiniMapPosition.Width, MiniMapPosition.Height), color);

                        if (!Globals.Map[x, y].Cleared && Globals.Map[x, y].Type != RoomType.Empty && Globals.CurrentRoomPos != new Point(x, y))
                            spriteBatch.Draw(Globals.ColorTexture, new Rectangle(x * 20 + MiniMapPosition.X, y * 20 + MiniMapPosition.Y, MiniMapPosition.Width, MiniMapPosition.Height), new Color(0, 0, 0, 100));
                    }

                    // Draw the "selected" highlight over the current room
                    if (Globals.CurrentRoomPos == new Point(x, y))
                        spriteBatch.Draw(RoomHighlight, new Rectangle(x * 20 + MiniMapPosition.X, y * 20 + MiniMapPosition.Y, MiniMapPosition.Width, MiniMapPosition.Height), Color.White);
                }
            }
        }

        #endregion

        #region Navigate between GameState methods
        void StartGame()
        {
            // Set the Current GameState to the InGame state
            Globals.CurrentGameState = GameState.InGame;

            // Reset the player by overriding the old player then adds it to the Entities list to be updated
            Globals.player = new Player(PlayerTextures, 0.4f * Globals.WindowScale, CharacterType.Guy, new Vector2(200) * Globals.WindowScale);
            Globals.Entities.Add(Globals.player);

            MediaPlayer.Stop();
            MediaPlayer.Play(Sounds.MainTheme);

            LoadLobby();
        }

        void ExitGame(bool ShouldSaveGame = true)
        {
            // Sets the Current GameState to the Menu state
            Globals.CurrentGameState = GameState.Menu;

            // Resets the variables used when in game
            Globals.Reset();

            if (ShouldSaveGame) SaveGame(Globals.CurrentSaveGameID, Globals.RunNo, Globals.NoBossCoins, Upgrades.ExportUpgrades());

            MediaPlayer.Stop();
            MediaPlayer.Play(Sounds.MainMenuTheme);
        }

        void ExitToMenuWithoutReset()
        {
            // Sets the Current GameState to the Menu state
            Globals.CurrentGameState = GameState.Menu;

            Globals.Components.Clear();

            MediaPlayer.Stop();
            MediaPlayer.Play(Sounds.MainMenuTheme);
        }

        void PastRuns()
        {
            Globals.CurrentGameState = GameState.PastRuns;

            PastRunsPage = 1;
            Globals.Components.Add(new EntryBox(new Rectangle(210, 20, 180, 50), Fonts.PreviousRun, "Search SaveGameID", Color.White));

            ReloadPastRunPage();
        }

        void SelectSaveGame()
        {
            Globals.CurrentGameState = GameState.SaveGamesMenu;

            ReloadSelectSaveButtons();
        }

        void LoadLobby()
        {
            Globals.Map = new Room[10, 10];
            for (int x = 0; x < 10; x++)
                for (int y = 0; y < 10; y++)
                    Globals.Map[x, y] = new Room() { Type = RoomType.Empty };

            Globals.Map[5, 5] = new Room()
            {
                Type = RoomType.Lobby,
                Pos = new Point(5)
            };

            Globals.CurrentRoomPos = new Point(5, 5);
            SetDoors();

            Globals.Stage = 0;

            Globals.Entities.Add(new Portal(Globals.GameWindowRectangle.Center.ToVector2(), 0));
        }

        void Pause()
        {
            Globals.CurrentGameState = GameState.Paused;
        }

        void Unpause()
        {
            Globals.CurrentGameState = GameState.InGame;
        }

        #endregion

        #region Utility functions

        void ReloadSelectSaveButtons()
        {
            int[] SaveGameIDs = LoadAllSaveGameIDs();
            List<Button> SaveGameButtons = new List<Button>();

            SaveGameButtons.Add(new Button(() => { CreateNewSaveGame(); ReloadSelectSaveButtons(); }, new Rectangle(500, 20, 200, 40), Color.DimGray, "Create new savegame", Fonts.Debug));

            for (int i = 0; i < SaveGameIDs.Length; i++)
            {
                int ID = SaveGameIDs[i];
                SaveGameButtons.Add(new Button(() => { Globals.CurrentSaveGameID = ID; LoadSave(ID, out Globals.RunNo, out Globals.NoBossCoins, out int EncodedUpgrades); Upgrades.ImportUpgrades(EncodedUpgrades); StartGame(); }, new Rectangle(20, 20 + i * 50, 200, 40), Color.DimGray, $"SaveGameID: {ID}", Fonts.Debug));
                SaveGameButtons.Add(new Button(() => { DeleteSave(ID); ReloadSelectSaveButtons(); }, new Rectangle(230, 20 + i * 50, 200, 40), Color.DimGray, "DeleteSave", Fonts.Debug));
            }

            Globals.GameStateButtons[GameState.SaveGamesMenu] = SaveGameButtons.ToArray();
        }

        void ReloadPastRunPage()
        {
            if (int.TryParse((Globals.Components.First() as EntryBox).Text, out int SaveGameID))
                Runs = Sort(LoadAllRuns().Where(x => x.SaveGameID == SaveGameID).ToArray());
            else
                Runs = Sort(LoadAllRuns());

            TotalPastRunPages = (int)Math.Ceiling((float)Runs.Length / (float)RunsPerPage);

            List<Button> Buttons = new List<Button>();

            if (PastRunsPage > 1)
                Buttons.Add(new Button(() => { PastRunsPage--; ReloadPastRunPage(); }, new Rectangle(20, 20, 80, 50), Color.Gray, "Previous", Fonts.Debug));
            if (TotalPastRunPages > PastRunsPage)
                Buttons.Add(new Button(() => { PastRunsPage++; ReloadPastRunPage(); }, new Rectangle(110, 20, 80, 50), Color.Gray, "Next", Fonts.Debug));

            Globals.GameStateButtons[GameState.PastRuns] = Buttons.ToArray();
        }

        void MoveBetweenRooms(MoveDirection DirectionToMove)
        {
            Globals.Entities.Clear();

            // Loop through every direction that can be moved in and change the current room to the new room and move the player to the opposite side that they exited through to give the illusion of walking. through the door
            switch (DirectionToMove)
            {
                case MoveDirection.Up:
                    Globals.CurrentRoomPos.Y -= 1;
                    Globals.player.Centre = new Vector2(Globals.GameWindowRectangle.Center.X, Globals.GameWindowRectangle.Bottom - (Globals.player.HitBox.Height / 2 + Door.Texture.Height / 3));
                    break;

                case MoveDirection.Right:
                    Globals.CurrentRoomPos.X += 1;
                    Globals.player.Centre = new Vector2(Globals.GameWindowRectangle.Left + (Globals.player.HitBox.Width / 2 + Door.Texture.Width / 3), Globals.GameWindowRectangle.Center.Y);
                    break;

                case MoveDirection.Down:
                    Globals.CurrentRoomPos.Y += 1;
                    Globals.player.Centre = new Vector2(Globals.GameWindowRectangle.Center.X, Globals.GameWindowRectangle.Top + (Globals.player.HitBox.Height / 2 + Door.Texture.Width / 3));
                    break;

                case MoveDirection.Left:
                    Globals.CurrentRoomPos.X -= 1;
                    Globals.player.Centre = new Vector2(Globals.GameWindowRectangle.Right - (Globals.player.HitBox.Width / 2 + Door.Texture.Width / 3), Globals.GameWindowRectangle.Center.Y);
                    break;
            }

            // Code for spawning new enemies in the new room
            if (!Globals.CurrentRoom.Cleared)
            {
                if (Globals.CurrentRoom.Type == RoomType.Fighting)
                {
                    // Creates random enemies in a random position then spawns them
                    List<(Type, Vector2)> EnemiesToAdd = CreateRandomEnemies(Math.Max(1, (int)Math.Round(4 * Globals.DifficultyMultiplier + Globals.RandomNormal() * 2)));
                    SpawnEntities(EnemiesToAdd);
                }
                else if (Globals.CurrentRoom.Type == RoomType.Boss)
                    switch (Globals.Stage)
                    {
                        //case 1:
                        //  break;

                        //case 2:
                        //  break;

                        case 3:
                            Globals.Entities.Add(new NailedCloth(NailedClothTextures, Globals.GameWindowRectangle.Center.ToVector2()));
                            break;

                        default:
                            Globals.Entities.Add(new NailedCloth(NailedClothTextures, Globals.GameWindowRectangle.Center.ToVector2()));
                            break;
                    }
                else if (Globals.CurrentRoom.Type == RoomType.Chest)
                    Globals.Entities.Add(new Chest(ChestTextures, Globals.GameWindowRectangle.Center.ToVector2(), Pickup.GetRandomPickup(null)));
                else if (Globals.CurrentRoom.Type == RoomType.Mimic)
                    Globals.Entities.Add(new Mimic(MimicTextures, Globals.GameWindowRectangle.Center.ToVector2()));
            }
            else if (Globals.CurrentRoom.Type == RoomType.Chest)
                Globals.Entities.Add(new Chest(ChestTextures, Globals.GameWindowRectangle.Center.ToVector2()));


            // Make the doors of the room reflect the new room
            SetDoors();

            Globals.Entities.Add(Globals.player);
        }

        void LoadNewLevel()
        {
            // Regenerates the Map
            Globals.Map = GenerateRandomMap();

            // Sets the current room to the starting room
            Globals.CurrentRoomPos = new Point(5, 5);

            // Make the doors of the room reflect the new room
            SetDoors();

            Globals.Entities.Clear();
            Globals.Entities.Add(Globals.player);
            Globals.player.Centre = Globals.GameWindowRectangle.Center.ToVector2();
        }

        /// <summary>
        /// Loops through the list and spawns an enemy of the specified type at the specified position
        /// </summary>
        void SpawnEntities(List<(Type, Vector2)> EntitiesToAdd)
        {
            foreach ((Type, Vector2) EntityTuple in EntitiesToAdd)
            {
                if (EntityTuple.Item1 == typeof(SwarmerEnemy))
                    Globals.Entities.Add(new SwarmerEnemy(EnemyTextures, 0.3f * Globals.WindowScale, EntityTuple.Item2, (float)(1 + (Globals.random.NextDouble() * 2 - 1) * 0.3) * 0.7f * Globals.WindowScale));

                else if (EntityTuple.Item1 == typeof(RandomMoverShooterEnemy))
                    Globals.Entities.Add(new RandomMoverShooterEnemy(EnemyTextures, 0.3f * Globals.WindowScale, EntityTuple.Item2, 1f * Globals.WindowScale, 0.1f));

                else if (EntityTuple.Item1 == typeof(SpawnerEnemy))
                    Globals.Entities.Add(new SpawnerEnemy(SpawnerTextures, EnemyTextures, 1.5f * Globals.WindowScale, EntityTuple.Item2));

                else if (EntityTuple.Item1 == typeof(GuardEnemy))
                    Globals.Entities.Add(new GuardEnemy(GuardTextures, 0.4f * Globals.WindowScale, EntityTuple.Item2));
            }
        }

        List<(Type, Vector2)> CreateRandomEnemies(int Count)
        {
            List<(Type, Vector2)> EnemyList = new List<(Type, Vector2)>();

            int RandNum;
            double EnemySpawnChancesSum = EnemySpawnChances.Sum();
            if (EnemySpawnChancesSum != 0)
                for (int i = 0; i < EnemySpawnChances.Length; i++)
                    EnemySpawnChances[i] /= EnemySpawnChancesSum;

            for (int i = 0; i < Count; i++)
            {
                double rand = Globals.random.NextDouble();

                double Chance = 0;
                for (RandNum = 0; RandNum < EnemySpawnChances.Length; RandNum++)
                {
                    Chance += EnemySpawnChances[RandNum];
                    if (rand < Chance)
                        break;
                }

                switch (RandNum)
                {
                    case 0:
                        Vector2 SpawnPoint = Globals.RandomPointWithinRectangle(Globals.FloorRectangle);
                        EnemyList.Add((typeof(SpawnerEnemy), SpawnPoint));
                        EnemyList.Add((typeof(SwarmerEnemy), Globals.RandomPointWithinCircle(SpawnPoint, 10 * Globals.WindowScale)));
                        EnemyList.Add((typeof(SwarmerEnemy), Globals.RandomPointWithinCircle(SpawnPoint, 10 * Globals.WindowScale)));
                        break;
                    case 1:
                        EnemyList.Add((typeof(SwarmerEnemy), Globals.RandomPointWithinRectangle(Globals.FloorRectangle)));
                        break;
                    case 2:
                        EnemyList.Add((typeof(RandomMoverShooterEnemy), Globals.RandomPointWithinRectangle(Globals.FloorRectangle)));
                        break;
                    case 3:
                        EnemyList.Add((typeof(GuardEnemy), Globals.RandomPointWithinRectangle(Globals.FloorRectangle)));
                        break;
                    default:
                        throw new Exception("What?");
                        break;

                }
            }

            return EnemyList;
        }

        void SetDoors()
        {
            // Resets the currently active doors
            Globals.ActiveDoors.Clear();

            if (!Globals.CurrentRoom.Cleared)
            {
                // If there are enemies in the current room then the doors should be closed in all 4 directions
                for (int i = 0; i < 4; i++)
                    Globals.ActiveDoors.Add(new Door((DoorType)i));
            }

            // Duplicates doors on the doorways that lead to nowhere
            RoomType[] StartRoomNeighbours = GetNeighbours(Room.ToRoomTypeArr(Globals.Map), Globals.CurrentRoomPos);
            for (int i = 0; i < 4; i++)
                if (StartRoomNeighbours[i] == RoomType.Empty)
                    Globals.ActiveDoors.Add(new Door((DoorType)i));
        }

        void OnRoomClear()
        {
            Globals.ActiveDoors.RemoveRange(0, 4);
            Globals.CurrentRoom.Cleared = true;

            if (Globals.random.Next(3) == 0 && !(Globals.CurrentRoom.Type == RoomType.Start || Globals.CurrentRoom.Type == RoomType.Lobby || Globals.CurrentRoom.Type == RoomType.Shop || Globals.CurrentRoom.Type == RoomType.Chest || Globals.CurrentRoom.Type == RoomType.Mimic || Globals.CurrentRoom.Type == RoomType.Boss))
                Globals.CurrentRoom.Pickups.Add(Pickup.RandomSpecialPickup());
        }

        void ChangeCharacter(CharacterType characterType)
        {
            Weapon[] weapon = Globals.player.EquippedWeapons;

            Globals.Entities.RemoveAll(x => x is Player);
            Globals.player = new Player(PlayerTextures, 0.4f * Globals.WindowScale, characterType, Globals.player.Centre);
            Globals.Entities.Add(Globals.player);

            for (int i = 0; i < 2; i++)
                if (weapon[i] != null)
                    ChangeWeapon(weapon[i].WeaponType, 0);
        }

        /// <param name="Index">null is for one handed, zero for left hand, one for right hand</param>
        void ChangeWeapon(WeaponType weaponType, int Index = -1)
        {
            Globals.Entities.RemoveAll(x => x is Player);
            Weapon[] weapons = Globals.player.EquippedWeapons;
            Globals.player = new Player(PlayerTextures, 0.4f * Globals.WindowScale, Globals.player.characterType, Globals.player.Centre);
            weapons.ToList().ForEach(x => { if (x != null) x.player = Globals.player; });
            Globals.player.EquippedWeapons = weapons;

            switch (weaponType) 
            {
                case WeaponType.Pistol:
                    Globals.player.EquippedWeapons[Index] = new Pistol(Globals.player, Index == 0);
                    break;
                case WeaponType.Dagger:
                    Globals.player.EquippedWeapons[Index] = new Dagger(Globals.player, Index == 0);
                    break;
                case WeaponType.Whip:
                    throw new NotImplementedException();
                    break;
                case WeaponType.Bow:
                    Globals.player.EquippedWeapons[0] = new Bow(Globals.player);
                    Globals.player.EquippedWeapons[1] = null;
                    break;
                case WeaponType.Staff:
                    Globals.player.EquippedWeapons[0] = new Staff(Globals.player);
                    Globals.player.EquippedWeapons[1] = null;
                    break;
                case WeaponType.Axe:
                    Globals.player.EquippedWeapons[0] = new Axe(Globals.player);
                    Globals.player.EquippedWeapons[1] = null;
                    break;
                default:
                    throw new NotImplementedException();
                    break;
            }

            Globals.Entities.Add(Globals.player);
        }

        // A little bit of cheeky trigonometry to find the angle to the positive x or to create a normal vector with the specified angle to the positive x
        static float VectorToAngle(Vector2 vector) => (float)Math.Atan2(vector.Y, vector.X);
        static Vector2 AngleToVector(float angle) => Vector2.Normalize(new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)));

        #endregion

        #region Merge Sort
        Run[] Sort(Run[] Runs, bool Descending = true)
        {
            int[] Scores = Runs.Select(Run => Run.Score).ToArray();
            List<int> SortedScores = MergeSort(Scores).ToList();

            for (int i = 0; i < SortedScores.Count - 1; i++)
                if (SortedScores[i] == SortedScores[i + 1])
                    SortedScores.RemoveAt(i);

            List<Run> SortedRuns = new List<Run>();

            foreach (int Score in SortedScores)
                SortedRuns.AddRange(Runs.Where(x => x.Score == Score));

            if (Descending)
            {
                Stack<Run> temp = new Stack<Run>();
                SortedRuns.ForEach(x => temp.Push(x));
                SortedRuns.Clear();
                while (temp.Count > 0) SortedRuns.Add(temp.Pop());
            }

            return SortedRuns.ToArray();
        }

        static int[] MergeSort(int[] Array, int Start = -1, int End = -1)
        {
            if (Array.Length == 0) return Array;

            // First call - initialise variables
            if (Start == -1)
                Start = 0;
            if (End == -1)
                End = Array.Length - 1;

            if (Start < End)
            {
                // Calculate index of midpoint (round down if decimal)
                int Mid = (Start + End) / 2;

                // Recusively perform MergeSort on left and right sides of the array
                int[] Left = MergeSort(Array, Start, Mid);
                int[] Right = MergeSort(Array, Mid + 1, End);

                // Merge left and right lists in the correct order
                return Merge(Left, Right);
            }
            else
            {
                // Base case - If there is only one element left in the array then return that one item
                return new int[] { Array[Start] };
            }
        }
        static int[] Merge(int[] Left, int[] Right)
        {
            List<int> MergedList = new List<int>();

            // Repeat while the lengths of both the arrays are bigger than 0
            while (Left.Length > 0 && Right.Length > 0)
            {
                // Compare the first value in the left and right array and add the smaller of the two to the merged list and remove it from the original array
                if (Left[0] < Right[0])
                {
                    MergedList.Add(Left[0]);
                    RemoveFirstItem(ref Left);
                }
                else
                {
                    MergedList.Add(Right[0]);
                    RemoveFirstItem(ref Right);
                }
            }

            // Add the remaining values in the arrays to the merged list
            if (Left.Length == 0)
                MergedList.AddRange(Right);
            else if (Right.Length == 0)
                MergedList.AddRange(Left);

            // Return the new merged list
            return MergedList.ToArray();
        }

        static void RemoveFirstItem(ref int[] array)
        {
            List<int> list = array.ToList();

            if (array.Length == 0)
                throw new Exception("No items in the array");
            else
                list = list.GetRange(1, list.Count - 1);

            array = list.ToArray();
        }
        #endregion

        #region RoomMap functions
        Room[,] GenerateRandomMap()
        {
            // This function is much like a breadth first search of a tree but with constraints and some randomness

            // Initialise some variables
            Random random = new Random();
            RoomType[] Map;
            Queue<int> Queue;
            List<int> EndRooms;
            RoomType[] OutMap;

            int NoOfRooms;
            int RoomCount;

            // Repeats the loop if the Map generated by chance is not a valid map by out constraints
            do
            {
                Map = Enumerable.Repeat(RoomType.Empty, 100).ToArray(); // Populates the array with RoomType.Empty
                Queue = new Queue<int>(); // Clears Queue
                EndRooms = new List<int>(); // Clears EndRooms

                NoOfRooms = random.Next(4) + 6; // Randomises the amount of Rooms to generate in the map

                Queue.Enqueue(55); // Enqueue the start room
                Map[55] = RoomType.Start;

                RoomCount = 0;

                // Repeat while there are "active rooms"
                while (Queue.Count > 0)
                {
                    int room = Queue.Dequeue(); // Dequeue the Current Room
                    bool roomAdded = false;

                    // Loop through every neighbour of the current node
                    for (int i = 0; i < 4; i++)
                    {
                        int DirectionDisplacement = 0;

                        switch (i)
                        {
                            case 0:
                                DirectionDisplacement = -10;
                                break;
                            case 1:
                                DirectionDisplacement = 1;
                                break;
                            case 2:
                                DirectionDisplacement = 10;
                                break;
                            case 3:
                                DirectionDisplacement = -1;
                                break;
                        }

                        int neighbour = room + DirectionDisplacement;
                        if (neighbour < 0 || neighbour > 99 || neighbour % 10 == 0)
                            continue; // Move onto the next neighbout if it is outside the bounds of the map


                        // Calculate the number of neighbours that the neighbour has
                        int NoOfNeighboursNeighbours = 0;
                        for (int j = 0; j < 4; j++)
                        {
                            int NeighbourDirectionDisplacement = 0;

                            switch (j)
                            {
                                case 0:
                                    NeighbourDirectionDisplacement = -10;
                                    break;
                                case 1:
                                    NeighbourDirectionDisplacement = 1;
                                    break;
                                case 2:
                                    NeighbourDirectionDisplacement = 10;
                                    break;
                                case 3:
                                    NeighbourDirectionDisplacement = -1;
                                    break;
                            }
                            if (neighbour + NeighbourDirectionDisplacement < 0 || neighbour + NeighbourDirectionDisplacement > 99)
                                continue;
                            if (Map[neighbour + NeighbourDirectionDisplacement] != RoomType.Empty)
                                NoOfNeighboursNeighbours++;
                        }

                        // Apply constraints
                        if (RoomCount == NoOfRooms) break; // If the NoOfRooms we want has been met then break out of the loop
                        else if (NoOfNeighboursNeighbours > 2) continue; // If the neighbour has more than 2 neighbours of itself then move onto the next neighbour
                        else if (Map[neighbour] != RoomType.Empty) continue; // If the neighbour has already been given a room then move ont the next neighbour
                        else if (random.Next(2) == 0) continue; // 50% chance to reject this neighbour anyway
                        else // If passes the constraints
                        {
                            RoomCount++; // Increase the amount of rooms
                            Queue.Enqueue(neighbour); // Add the neighbour to be searched
                            roomAdded = true; // The current node has added a neighbour to the map so it isnt the end of a corridor
                            Map[neighbour] = RoomType.Temp; // Set the RoomType to be set later
                        }
                    }
                    if (!roomAdded) // The current node hasn't added a neighbour to the map so it is the end of a corridor adn will be used for a boss, chest, mimic, or shop room
                        EndRooms.Add(room);
                }

                // Add special rooms to the map
                Map[EndRooms[EndRooms.Count - 1]] = RoomType.Boss; // The last end room is always the boss room
                if (EndRooms.Count > 1)
                {
                    // Randomly assign the second to last end room
                    switch (random.Next(3))
                    {
                        case 0:
                            Map[EndRooms[random.Next(EndRooms.Count - 2)]] = RoomType.Chest;
                            break;
                        case 1:
                            Map[EndRooms[random.Next(EndRooms.Count - 2)]] = RoomType.Mimic;
                            break;
                        case 2:
                            Map[EndRooms[EndRooms.Count - 2]] = RoomType.Shop;
                            break;
                    }
                }

                // Any other unassigned room will be set to a fighting room unless it is empty
                OutMap = new RoomType[100];
                for (int i = 0; i < Map.Length; i++)
                {
                    if (Map[i] == RoomType.Empty) continue;
                    OutMap[i] = (Map[i] == RoomType.Temp) ? RoomType.Fighting : Map[i];
                }

            } while (!GoodMap(OutMap)); // Repeats the loop if the Map generated by chance is not a valid map by out constraints

            // Converts one dimensional array to a 2 dimensional array
            RoomType[,] RoomTypeMap = OneDToTwoDArray(OutMap);

            Room[,] RoomMap = new Room[10, 10];

            for (int x = 0; x < 10; x++)
                for (int y = 0; y < 10; y++)
                    RoomMap[x, y] = new Room()
                    {
                        Type = RoomTypeMap[x, y],
                        Cleared = false,
                        Pos = new Point(x, y),
                        BuyableItems = new BuyableItem[3] { new BuyableItem(0), new BuyableItem(1), new BuyableItem(2) }
                    };

            return RoomMap;
        }

        // The GenerateRandomMap function returns a one dimensional array so it needs to be converted to a 2d array
        RoomType[,] OneDToTwoDArray(RoomType[] Map)
        {
            RoomType[,] _2DMap = new RoomType[10, 10];

            for (int x = 0; x < 10; x++)
                for (int y = 0; y < 10; y++)
                    _2DMap[x, y] = Map[y * 10 + x];

            return _2DMap;
        }

        RoomType[] GetNeighbours(RoomType[,] Map, Point CurrentRoom)
        {
            // Gets the current nodes neighbours

            RoomType[] neighbours = new RoomType[4];
            if (CurrentRoom.Y - 1 >= 0) // Up
                neighbours[0] = Map[CurrentRoom.X, CurrentRoom.Y - 1];
            if (CurrentRoom.X + 1 < 10) // Right
                neighbours[1] = Map[CurrentRoom.X + 1, CurrentRoom.Y];
            if (CurrentRoom.Y + 1 < 10) // Down
                neighbours[2] = Map[CurrentRoom.X, CurrentRoom.Y + 1];
            if (CurrentRoom.X - 1 >= 0) // Left
                neighbours[3] = Map[CurrentRoom.X - 1, CurrentRoom.Y];

            return neighbours;
        }

        Room[] GetNeighbourRooms(Room[,] Map, Point CurrentRoom)
        {
            // Gets the current nodes neighbours

            Room[] neighbours = new Room[4];
            if (CurrentRoom.Y - 1 > 0) // Up
                neighbours[0] = Map[CurrentRoom.X, CurrentRoom.Y - 1];
            if (CurrentRoom.X + 1 < 10) // Right
                neighbours[1] = Map[CurrentRoom.X + 1, CurrentRoom.Y];
            if (CurrentRoom.Y + 1 < 10) // Down
                neighbours[2] = Map[CurrentRoom.X, CurrentRoom.Y + 1];
            if (CurrentRoom.X - 1 > 0) // Left
                neighbours[3] = Map[CurrentRoom.X - 1, CurrentRoom.Y];

            return neighbours;
        }

        // Reapplies the same constraints from inside the loop over the entire map to make sure no bad maps could by chance form
        bool GoodMap(RoomType[] Map)
        {
            bool goodMap = true;

            int RoomCount = 0;
            bool TooManyNeighbours = false;
            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < 10; y++)
                {
                    int i = y * 10 + x;

                    if (Map[i] != RoomType.Empty)
                        RoomCount++;

                    int NeighbourCount = 0;
                    foreach (RoomType neighbour in GetNeighbours(OneDToTwoDArray(Map), new Point(x, y)))
                        if (neighbour != RoomType.Empty)
                            NeighbourCount++;

                    if (NeighbourCount > 3)
                        TooManyNeighbours = true;
                }
            }

            if (GetNeighbours(OneDToTwoDArray(Map), new Point(5, 5)).Contains(RoomType.Boss)) goodMap = false;
            else if (!Map.Contains(RoomType.Boss)) goodMap = false;
            else if (!Map.Contains(RoomType.Start)) goodMap = false;
            else if (TooManyNeighbours) goodMap = false;
            else if (RoomCount < 7) goodMap = false;

            return goodMap;
        }

        #endregion

        #region SQL

        SQLiteConnection SQLiteCon;
        SQLiteCommand SQLiteCmd;

        void InitialiseSQL()
        {
            // Get SaveData.db file and create a new file if doesnt exist
            SQLiteCon = new SQLiteConnection("Data Source=SaveData.db; Version = 3; new = true; Compress = true; ");

            // Open Connection to file
            SQLiteCon.Open();

            // Create tables for save data if they dont exist
            SQLiteCmd = SQLiteCon.CreateCommand();
            SQLiteCmd.CommandText = "CREATE TABLE IF NOT EXISTS SaveGames(SaveGameID INT UNIQUE NOT NULL, RunNo INT, NoBossCoins INT, Upgrades INT, PRIMARY KEY (SaveGameID))";
            SQLiteCmd.ExecuteNonQuery();

            SQLiteCmd = SQLiteCon.CreateCommand();
            SQLiteCmd.CommandText = "CREATE TABLE IF NOT EXISTS PreviousRuns(RunID INT UNIQUE NOT NULL, SaveGameID INT, RunNo INT, RunTime TIME, NoBossCoinsCollected INT, Score INT, RunWon BIT, PRIMARY KEY (RunID), FOREIGN KEY (SaveGameID) REFERENCES SaveGames (SaveGameID))";
            SQLiteCmd.ExecuteNonQuery();
        }

        void DeleteSave(int SaveGameID)
        {
            SQLiteCmd = SQLiteCon.CreateCommand();
            SQLiteCmd.CommandText = "DELETE FROM SaveGames WHERE SaveGameID = @SaveGameID";
            SQLiteCmd.Parameters.AddWithValue("@SaveGameID", SaveGameID);
            SQLiteCmd.ExecuteNonQuery();

            SQLiteCmd = SQLiteCon.CreateCommand();
            SQLiteCmd.CommandText = "DELETE FROM PreviousRuns WHERE SaveGameID = @SaveGameID";
            SQLiteCmd.Parameters.AddWithValue("@SaveGameID", SaveGameID);
            SQLiteCmd.ExecuteNonQuery();
        }

        void SaveGame(int SaveGameID, int RunNo, int NoBossCoins, int Upgrades)
        {
            SQLiteCmd = SQLiteCon.CreateCommand();
            SQLiteCmd.CommandText = "UPDATE SaveGames SET RunNo = @RunNo, NoBossCoins = @NoBossCoins, Upgrades = @Upgrades WHERE SaveGameID = @SaveGameID";
            SQLiteCmd.Parameters.AddWithValue("@SaveGameID", SaveGameID);
            SQLiteCmd.Parameters.AddWithValue("@RunNo", RunNo);
            SQLiteCmd.Parameters.AddWithValue("@NoBossCoins", NoBossCoins);
            SQLiteCmd.Parameters.AddWithValue("@Upgrades", Upgrades);
            SQLiteCmd.ExecuteNonQuery();
        }

        void SaveNewRun(int SaveGameID, int RunNo, TimeSpan RunTime, int NoBossCoinsCollected, int Score, bool RunWon)
        {
            SQLiteCmd = SQLiteCon.CreateCommand();
            SQLiteCmd.CommandText = "INSERT INTO PreviousRuns(RunID, SaveGameID, RunNo, RunTime, NoBossCoinsCollected, Score, RunWon) VALUES(@RunID, @SaveGameID, @RunNo, @RunTime, @NoBossCoinsCollected, @Score, @RunWon)";
            SQLiteCmd.Parameters.AddWithValue("@RunID", int.Parse(SaveGameID.ToString("D8") + RunNo.ToString("D8")));
            SQLiteCmd.Parameters.AddWithValue("@SaveGameID", SaveGameID);
            SQLiteCmd.Parameters.AddWithValue("@RunNo", RunNo);
            SQLiteCmd.Parameters.AddWithValue("@RunTime", RunTime);
            SQLiteCmd.Parameters.AddWithValue("@NoBossCoinsCollected", NoBossCoinsCollected);
            SQLiteCmd.Parameters.AddWithValue("@Score", Score);
            SQLiteCmd.Parameters.AddWithValue("@RunWon", RunWon);
            SQLiteCmd.ExecuteNonQuery();
        }

        int CreateNewSaveGame()
        {
            int SaveGameID = 0;
            bool UniqueID;

            do
            {
                try
                {
                    SQLiteCmd = SQLiteCon.CreateCommand();
                    SQLiteCmd.CommandText = "INSERT INTO SaveGames(SaveGameID, RunNo, NoBossCoins, Upgrades) VALUES(@SaveGameID, @RunNo, @NoBossCoins, @Upgrades)";
                    SQLiteCmd.Parameters.AddWithValue("@SaveGameID", SaveGameID);
                    SQLiteCmd.Parameters.AddWithValue("@RunNo", 0);
                    SQLiteCmd.Parameters.AddWithValue("@NoBossCoins", 0);
                    SQLiteCmd.Parameters.AddWithValue("@Upgrades", Upgrades.DefaultUpgrades);
                    SQLiteCmd.ExecuteNonQuery();
                    UniqueID = true;
                }
                catch (SQLiteException)
                {
                    UniqueID = false;
                    SaveGameID++;
                }
            } while (!UniqueID);

            return SaveGameID;
        }

        int[] LoadAllSaveGameIDs()
        {
            List<int> IDList = new List<int>();

            SQLiteCmd = SQLiteCon.CreateCommand();
            SQLiteCmd.CommandText = "SELECT SaveGameID FROM SaveGames";
            using (SQLiteDataReader reader = SQLiteCmd.ExecuteReader())
            {
                while (reader.Read())
                    IDList.Add(reader.GetInt32(0));
                reader.Close();
            }

            return IDList.ToArray();
        }

        void LoadSave(int SaveGameID, out int RunNo, out int NoBossCoins, out int Upgrades)
        {
            SQLiteCmd = SQLiteCon.CreateCommand();
            SQLiteCmd.CommandText = "SELECT * FROM SaveGames WHERE SaveGameID = @SaveGameID";
            SQLiteCmd.Parameters.AddWithValue("@SaveGameID", SaveGameID);
            using (SQLiteDataReader reader = SQLiteCmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    RunNo = reader.GetInt32(1);
                    NoBossCoins = reader.GetInt32(2);
                    Upgrades = reader.GetInt32(3);
                }
                else
                {
                    throw new ArgumentOutOfRangeException("No savegame in Database with that parameter");
                }
                reader.Close();
            }
        }

        Run[] LoadRuns(int SaveGameID)
        {
            List<Run> RunList = new List<Run>();

            SQLiteCmd = SQLiteCon.CreateCommand();
            SQLiteCmd.CommandText = "SELECT * FROM PreviousRuns WHERE SaveGameID = @SaveGameID";
            SQLiteCmd.Parameters.AddWithValue("@SaveGameID", SaveGameID);
            using (SQLiteDataReader reader = SQLiteCmd.ExecuteReader())
            {
                while (reader.Read())
                    RunList.Add(new Run(reader.GetInt32(0), reader.GetInt32(1), reader.GetInt32(2), TimeSpan.Parse(reader.GetString(3)), reader.GetInt32(4), reader.GetInt32(5), reader.GetBoolean(6)));
                reader.Close();
            }

            return RunList.ToArray();
        }

        Run[] LoadAllRuns()
        {
            List<Run> RunList = new List<Run>();
            int[] SaveGameIDs = LoadAllSaveGameIDs();

            foreach (int SaveGameID in SaveGameIDs)
                RunList.AddRange(LoadRuns(SaveGameID));

            return RunList.ToArray();
        }

        #endregion

        #region Secret Room
        string[] StartRoomLines = new string[]
        {
            "You find yourself in a room.",
            "A rubber room.",
            "A rubber room with rats.",
            "The rats make you crazy.",
            "Crazy?"
        };
        string[] RepeatRoomLines = new string[]
        {
            "I was crazy once.",
            "They locked me in a room.",
            "A rubber room.",
            "A rubber room with rats.",
            "The rats made me crazy.",
            "Crazy?"
        };

        float FastTalkReloadTime = 0.06f, SlowTalkReloadTime = 0.1f, TalkReloadTimeRemaining = 0f;
        bool Repeating = false, ActuallyRepeating = false, Waiting = false;
        int LineIndex = 0, CharIndex = 0;
        string OutString = "";
        void SecretRoomUpdate(GameTime gameTime)
        {
            IsMouseVisible = false;

            if (Globals.CurrentKeyboardState.IsKeyDown(Keys.Space) && Globals.PreviousKeyboardState.IsKeyUp(Keys.Space) && Waiting)
            {
                if (Repeating && !ActuallyRepeating)
                {
                    RatIsHere.Play();
                    ActuallyRepeating = true;
                }

                Waiting = false;
                OutString = "";
            }

            if (TalkReloadTimeRemaining > 0)
                TalkReloadTimeRemaining -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            else if (!Waiting)
            {
                if (!Repeating && LineIndex == StartRoomLines.Length && CharIndex == StartRoomLines[LineIndex].Length)
                    TalkReloadTimeRemaining = SlowTalkReloadTime;
                else
                    TalkReloadTimeRemaining = FastTalkReloadTime;

                if (Repeating && CharIndex == RepeatRoomLines[LineIndex].Length || !Repeating && CharIndex == StartRoomLines[LineIndex].Length)
                {
                    CharIndex = 0;
                    LineIndex++;
                    Waiting = true;
                }
                if (!Repeating && LineIndex == StartRoomLines.Length)
                {
                    Repeating = true;
                    LineIndex = 0;
                }
                if (Repeating && LineIndex == RepeatRoomLines.Length)
                {
                    LineIndex = 0;
                }

                if (!Waiting)
                {
                    OutString += Repeating ? RepeatRoomLines[LineIndex][CharIndex] : StartRoomLines[LineIndex][CharIndex];
                    CharIndex++;
                }

                if (OutString.Length != 0 && OutString.Last() != ' ')
                    Talking.Play();
            }
        }

        void SecretRoomDraw()
        {
            spriteBatch.Draw(Globals.ColorTexture, Globals.ScreenRectangle, Color.Black);
            spriteBatch.Draw(RatBackground, Globals.GameWindowRectangle, Color.White);
            if (ActuallyRepeating)
                spriteBatch.Draw(Rat, Globals.GameWindowRectangle.Center.ToVector2() - new Vector2(Rat.Bounds.Size.ToVector2().X / 2, Rat.Bounds.Size.ToVector2().Y / 1.2f) * Globals.WindowScale * 2, null, Color.White, 0f, new Vector2(0), Globals.WindowScale * 2, SpriteEffects.None, 0f);

            spriteBatch.DrawString(Fonts.YoureCrazy.SpriteFont, "*", Globals.GameWindowRectangle.Center.ToVector2() + new Vector2(-635, 150 - Fonts.YoureCrazy.SpriteFont.MeasureString("*").Y * Globals.WindowScale / 2), Color.White, 0f, new Vector2(0), Globals.WindowScale, SpriteEffects.None, 0f);
            spriteBatch.DrawString(ActuallyRepeating ? Fonts.IWasCrazyOnce.SpriteFont : Fonts.YoureCrazy.SpriteFont, OutString, Globals.GameWindowRectangle.Center.ToVector2() + new Vector2(-585, 150 - (ActuallyRepeating ? Fonts.IWasCrazyOnce.SpriteFont : Fonts.YoureCrazy.SpriteFont).MeasureString(OutString).Y * Globals.WindowScale / 2), ActuallyRepeating ? Color.Red : Color.White, 0f, new Vector2(0), Globals.WindowScale, SpriteEffects.None, 0f);
        }
        #endregion
    }
}