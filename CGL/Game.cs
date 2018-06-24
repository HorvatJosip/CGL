using System;
using System.Collections.Generic;

using CGL.Board;
using CGL.Entities;
using CGL.Printables;

namespace CGL
{
    /// <summary>
    /// Manages all the components that a game will have
    /// </summary>
    public abstract class Game
    {
        #region Fields

        /// <summary>
        /// Game board / Terrain
        /// </summary>
        protected Board.Board board;

        /// <summary>
        /// Collection of actions that occurr on certain keypress combinations
        /// </summary>
        protected IDictionary<(ConsoleModifiers, ConsoleKey), Action> keyPressActions;

        #endregion

        #region Constructors

        /// <summary>
        /// Generates a <see cref="Game"/> instance with entities that will fill the board
        /// and a tile that will be used to replace tiles that are removed from the board
        /// </summary>
        /// <param name="gameRenderArea">Area that is printed on the screen</param>
        /// <param name="entities">Entities that will be placed onto the board</param>
        /// <param name="replacementTile">Tile used to replace the deleted tiles (can be null which means nothing
        /// will replace the removed tiles)</param>
        /// <exception cref="ArgumentNullException"/>
        public Game(Rectangle gameRenderArea, IEnumerable<Entity> entities, Tile replacementTile)
        {
            Utils.ThrowIfNull((gameRenderArea, nameof(gameRenderArea)), (entities, nameof(entities)));

            board = new Board.Board(gameRenderArea, entities) { ReplacementTile = replacementTile };

            GameLoop();
        }

        /// <summary>
        /// Generates a <see cref="Game"/> instance with tile chunk that will fill the board
        /// </summary>
        /// <param name="gameRenderArea">Area that is printed on the screen</param>
        /// <param name="initialBoardCharGraphics">Walkable tile graphics</param>
        /// <param name="initialBorderGraphics">Border tile graphics (if you don't want a border, leave this null)</param>
        /// <exception cref="ArgumentNullException"/>
        public Game(Rectangle gameRenderArea, Graphics initialBoardCharGraphics, Graphics initialBorderGraphics = null)
            : this(gameRenderArea, Utils.GenerateTileChunk(initialBoardCharGraphics, gameRenderArea, initialBorderGraphics),
                  new Tile(TileType.Walkable, initialBoardCharGraphics)) { }

        #endregion

        #region Methods

        private void GameLoop()
        {
            board.EntityAboutToMove += EntityAboutToMove;
            board.EntityMoved += EntityMoved;

            Initialize();

            while (true)
            {
                LoopStart();

                if (Console.KeyAvailable)
                {
                    var input = Console.ReadKey(true);
                    OnKeyPressed(input);
                    TriggerKeyPressActions(input);
                }

                LoopEnd();
            }
        }

        private void TriggerKeyPressActions(ConsoleKeyInfo input)
        {
            if (keyPressActions == null)
                return;

            foreach (var kvp in keyPressActions)
                if (input.Modifiers == kvp.Key.Item1 && input.Key == kvp.Key.Item2)
                    kvp.Value?.Invoke();
        }

        /// <summary>
        /// Used for displaying a message on game exit (waits for key press and then exits)
        /// </summary>
        /// <param name="exitMessage">Message displayed when the user wants to exit</param>
        /// <exception cref="ArgumentNullException"/>
        public void Exit(Text exitMessage)
        {
            exitMessage.ThrowIfNull(nameof(exitMessage));

            Console.Clear();

            int offset = 10;

            int x = Console.WindowWidth / 2 - offset;
            int y = Console.WindowHeight / 2 - offset;
            int width = offset * 2 + Console.WindowWidth % 2;
            int height = offset * 2 + Console.WindowHeight % 2;

            var rect = new Rectangle(x, y, width, height);
            var text = exitMessage.GetWrappedText(rect, true, out int wrapped);

            Utils.Draw(text, false);

            Console.SetCursorPosition(rect.X, rect.Y + wrapped + 2);
            Utils.Draw("Press any key to exit...", ConsoleColor.Red, true);

            Console.ReadKey(true);

            Console.Clear();
            Environment.Exit(0);
        }

        #region Events

        /// <summary>
        /// Occurs before the game starts
        /// <para>By default draws the board</para>
        /// </summary>
        protected virtual void Initialize() { board.Draw(); }

        /// <summary>
        /// Occurs when the user presses a key (or key combination)
        /// </summary>
        /// <param name="key">Information about the keypress</param>
        protected virtual void OnKeyPressed(ConsoleKeyInfo key) { }

        /// <summary>
        /// Occurs at the start of game loop
        /// </summary>
        protected virtual void LoopStart() { }

        /// <summary>
        /// Occurs at the end of game loop
        /// </summary>
        protected virtual void LoopEnd() { }

        /// <summary>
        /// Occurs when an entity successfully finishes the movement
        /// </summary>
        /// <param name="sender">Object that invoked the event</param>
        /// <param name="e">Arguments for the event</param>
        protected virtual void EntityMoved(object sender, EntityMovementEventArgs e) { }

        /// <summary>
        /// Occurs when an entity is about to move
        /// </summary>
        /// <param name="sender">Object that invoked the event</param>
        /// <param name="e">Arguments for the event</param>
        protected virtual void EntityAboutToMove(object sender, EntityMovementEventArgs e) { }

        #endregion

        #endregion
    }
}
