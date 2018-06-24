using System;
using System.Collections.Generic;
using System.Linq;

using CGL.Entities;

namespace CGL.Board
{
    /// <summary>
    /// Represents a terrain on which the game is played
    /// </summary>
    public class Board
    {
        #region Fields

        private List<Entity> entities;
        private Printer printer;

        #endregion

        #region Properties

        #region Indexers

        /// <summary>
        /// Gets an entity from the board at the specified position
        /// </summary>
        /// <param name="x">X coordinate of the entity</param>
        /// <param name="y">Y coordinate of the entity</param>
        /// <returns></returns>
        public Entity this[int x, int y] => this[new Position(x, y)];

        /// <summary>
        /// Gets an entity from the board at the specified position
        /// </summary>
        /// <param name="position">Position of the entity</param>
        public Entity this[Position position]
        {
            get
            {
                if (entities == null || position == null)
                    return null;

                return entities.FirstOrDefault(entity => entity.Position == position);
            }
        }

        /// <summary>
        /// Gets the first entity from the board with the specified graphics
        /// </summary>
        /// <param name="graphics">Graphics of the entity to find</param>
        public Entity this[Graphics graphics]
        {
            get
            {
                if (entities == null || graphics == null)
                    return null;

                return entities.FirstOrDefault(entity => entity.Graphics == graphics);
            }
        }

        #endregion

        /// <summary>
        /// Represents how far does the map stretch on the X and Y axis
        /// </summary>
        public Bounds2D TerrainBounds { get; } = new Bounds2D(
            new Bounds(int.MaxValue, int.MinValue),
            new Bounds(int.MaxValue, int.MinValue)
        );

        /// <summary>
        /// Area that is printed on the screen
        /// </summary>
        public Rectangle DrawableArea { get; }

        /// <summary>
        /// Tile that is used to replace a tile that gets removed (can be left null so that
        /// nothing replaces the removed tile)
        /// </summary>
        public Tile ReplacementTile { get; set; }

        #endregion

        #region Events

        /// <summary>
        /// Triggered when an entity is about to move
        /// </summary>
        public event EventHandler<EntityMovementEventArgs> EntityAboutToMove;

        /// <summary>
        /// Triggered when an entity successfully finishes the movement
        /// </summary>
        public event EventHandler<EntityMovementEventArgs> EntityMoved;

        #endregion

        #region Constructors

        /// <summary>
        /// Generates a <see cref="Board"/> instance with the given drawable area.
        /// </summary>
        /// <param name="drawableArea">Area that is printed on the screen</param>
        /// <exception cref="ArgumentNullException"/>
        public Board(Rectangle drawableArea)
        {
            drawableArea.ThrowIfNull(nameof(drawableArea));

            DrawableArea = drawableArea;

            printer = new Printer(drawableArea,
                new Bounds2D(drawableArea.X, drawableArea.Right, drawableArea.Y, drawableArea.Bottom));
        }

        /// <summary>
        /// Generates a <see cref="Board"/> instance with the given drawable area and entities
        /// that will be placed onto the board (and drawn if they are inside the drawable area).
        /// </summary>
        /// <param name="drawableArea">Area that is printed on the screen</param>
        /// <param name="entities">Entities that will be placed onto the board (and drawn if they are inside the drawable area)</param>
        /// <exception cref="ArgumentNullException"/>
        public Board(Rectangle drawableArea, IEnumerable<Entity> entities)
            : this(drawableArea)
        {
            entities.ThrowIfNull(nameof(entities));

            this.entities = entities.ToList();
            printer.Entities = entities;

            int minX = entities.Min(tile => tile?.Position?.X ?? int.MaxValue);
            int maxX = entities.Max(tile => tile?.Position?.X ?? int.MinValue);
            int minY = entities.Min(tile => tile?.Position?.Y ?? int.MaxValue);
            int maxY = entities.Max(tile => tile?.Position?.Y ?? int.MinValue);

            TerrainBounds.X = new Bounds(minX, maxX);
            TerrainBounds.Y = new Bounds(minY, maxY);
        }

        #endregion

        #region Methods

        #region Drawing

        /// <summary>
        /// Clears the drawable area and draws all the entities
        /// </summary>
        public bool Draw()
        {
            printer.ClearDrawableArea();

            return printer.DrawEntities(entities, true);
        }

        /// <summary>
        /// Draws a part of the board that resideds in the specified bounds
        /// </summary>
        /// <param name="startX">X coordinate where the drawing should start</param>
        /// <param name="endX">X coordinate where the drawing should end</param>
        /// <param name="startY">Y coordinate where the drawing should start</param>
        /// <param name="endY">Y coordinate where the drawing should end</param>
        /// <returns></returns>
        public bool DrawPart(int startX, int endX, int startY, int endY)
            => DrawPart(new Bounds2D(startX, endX, startY, endY));

        /// <summary>
        /// Draws a part of the board that resideds in the specified bounds
        /// </summary>
        /// <param name="bounds">Area that needs to be printed</param>
        /// <exception cref="ArgumentNullException"/>
        public bool DrawPart(Bounds2D bounds)
        {
            bounds.ThrowIfNull(nameof(bounds));

            var initialBounds = printer.PrintableBounds;

            printer.PrintableBounds = bounds;

            var drawn = Draw();

            printer.PrintableBounds = initialBounds;

            return drawn;
        }

        #endregion

        #region Moving

        /// <summary>
        /// Moves the board in a given direction
        /// </summary>
        /// <param name="direction">Direction to move the board in</param>
        /// <param name="count">How much should the board be moved in the given direction</param>
        /// <param name="drawAfter">Should the board be redrawn after it has been moved</param>
        /// <exception cref="UnknownEnumValueException"/>
        public bool Move(Direction direction, int count, bool drawAfter)
        {
            if (count < 1)
                return false;

            switch (direction)
            {
                case Direction.Up:
                    printer.PrintableBounds.Y -= count;
                    break;
                case Direction.Down:
                    printer.PrintableBounds.Y += count;
                    break;
                case Direction.Left:
                    printer.PrintableBounds.X -= count;
                    break;
                case Direction.Right:
                    printer.PrintableBounds.X += count;
                    break;
                default:
                    throw new UnknownEnumValueException();
            }

            if (drawAfter)
                return Draw();

            return true;
        }

        /// <summary>
        /// Moves an entity in a certain direction
        /// </summary>
        /// <param name="entity">Entity to move</param>
        /// <param name="direction">Direction to move it in</param>
        /// <param name="amount">How many tiles should it be moved in that direction</param>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="UnknownEnumValueException"/>
        public bool MoveEntity(Entity entity, Direction direction, int amount)
            => MoveEntity(entity, entity.Position.GetRelativePosition(direction, amount));

        /// <summary>
        /// Moves an entity to a certain position
        /// </summary>
        /// <param name="entity">Entity to move</param>
        /// <param name="position">Position to move the entity to</param>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="UnknownEnumValueException"/>
        public bool MoveEntity(Entity entity, Position position)
        {
            entity.ThrowIfNull(nameof(entity));

            EntityAboutToMove?.Invoke(this, new EntityMovementEventArgs(entity, null));

            var targetTile = printer[position] as Tile;

            if (targetTile == null || !(
                targetTile.Type.HasFlag(TileType.Walkable) &&
                position.InsideBounds(printer.PrintableBounds)))
                return false;

            EditEntities(entity, EditOperation.Delete);
            entity.Position = position;
            EditEntities(entity, EditOperation.UpdateOrCreate);

            EntityMoved?.Invoke(this, new EntityMovementEventArgs(entity, targetTile));

            return true;
        }

        /// <summary>
        /// Moves an entity in a random direction
        /// </summary>
        /// <param name="entity">Entity to move</param>
        /// <param name="amount">How many tiles should it be moved in that direction</param>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="UnknownEnumValueException"/>
        public bool MoveEntityInRandomDirection(Entity entity, int amount)
            => MoveEntity(entity, Utils.GetRandomEnumValue<Direction>(), amount);

        #endregion

        /// <summary>
        /// Performs an operation on the entities:
        /// <para><see cref="EditOperation.Create"/>: adds a new entity</para>
        /// <para><see cref="EditOperation.Update"/>: updates an existing entity (based on <see cref="Entity.Position"/>)</para>
        /// <para><see cref="EditOperation.Delete"/>: deletes an entity</para>
        /// <para><see cref="EditOperation.UpdateOrCreate"/>: if the entity already exists, it updates it, 
        /// otherwise, it creates a new one</para>
        /// </summary>
        /// <param name="entity">Entity used in the edit operation</param>
        /// <param name="operation">Edit operation to perform</param>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="UnknownEnumValueException"/>
        public bool EditEntities(Entity entity, EditOperation operation)
        {
            entity.ThrowIfNull(nameof(entity));

            bool DrawEntityAfterOperationSucceeds(Func<bool> operationToExecute)
            {
                if (operationToExecute?.Invoke() == true)
                    return printer.Draw(entity);

                return false;
            }

            switch (operation)
            {
                case EditOperation.Create:
                    return DrawEntityAfterOperationSucceeds(() => AddEntityIfItDoesntExist(entity));
                case EditOperation.Update:
                    return DrawEntityAfterOperationSucceeds(() => UpdateEntity(entity));
                case EditOperation.Delete:
                    if (!entities.Remove(entity))
                        return false;

                    bool emptyTile = ReplacementTile == null;

                    var tile = emptyTile
                        ? new Tile(TileType.None, new Graphics(ConsoleColor.Black, ' '), entity.Position)
                        : new Tile(ReplacementTile, entity.Position);

                    //if (!emptyTile)
                    entities.Add(tile);

                    return printer.Draw(tile);
                case EditOperation.UpdateOrCreate:
                    return DrawEntityAfterOperationSucceeds(() => AddEntityIfItDoesntExist(entity))
                    ? true
                    : DrawEntityAfterOperationSucceeds(() => UpdateEntity(entity));
                default:
                    throw new UnknownEnumValueException();
            }
        }

        /// <summary>
        /// Generates a spawn point at random based on the given parameters
        /// </summary>
        /// <param name="walkabilityMinimumPercentage">Minimum percentage (from 0 to 100) that needs
        /// to be walkable (<see cref="TileType.Walkable"/>) around the position that was randomly picked</param>
        /// <param name="walkabilityCheckRadius">Radius of tiles checked for walkability (at least 1).
        /// Example: if the given radius is 1, only the 8 neighbour tiles will be checked for walkability</param>
        /// <param name="useEmptySpotsIntoAccount">If a tile doesn't exist, should it be taken into account?
        /// Example: imagine being in the corner of the map and the specified radius is 1. If this is set to true,
        /// there will be 8 tiles to check, otherwise just 3</param>
        /// <param name="tileHasToBeWalkable">Should the position returned be walkable or not</param>
        /// <param name="triesAllowed">Number of tries (at least 1) allowed until the position with the given
        /// arguments is found (if the try count reaches this number, null is returned)</param>
        public Position GetRandomSpawnPoint(
            int walkabilityMinimumPercentage = 25,
            int walkabilityCheckRadius = 3,
            bool useEmptySpotsIntoAccount = true,
            bool tileHasToBeWalkable = true,
            int triesAllowed = 10000
        )
        {
            if (!walkabilityMinimumPercentage.Between(0, 100) ||
                walkabilityCheckRadius < 1 ||
                triesAllowed < 1)
                return null;

            while (true)
            {
                var randomEntity = entities.ElementAt(Utils.Rng.Next(entities.Count));

                if (!(
                    randomEntity is Tile startTile &&
                    startTile.Position.InsideRectangle(DrawableArea) &&
                    (!tileHasToBeWalkable || startTile.Type.HasFlag(TileType.Walkable))
                ))
                    continue;

                int walkable = 0;
                int checkCount = 0;

                int xMin = startTile.X - walkabilityCheckRadius;
                int xMax = startTile.X + walkabilityCheckRadius;
                int yMin = startTile.Y - walkabilityCheckRadius;
                int yMax = startTile.Y + walkabilityCheckRadius;

                for (int x = xMin; x <= xMax; x++)
                    for (int y = yMin; y <= yMax; y++)
                    {
                        var entity = printer[x, y];

                        if (entity is Tile tile && tile != null)
                        {
                            if (tile.Type.HasFlag(TileType.Walkable))
                                walkable++;

                            checkCount++;
                        }
                        else if (useEmptySpotsIntoAccount)
                            checkCount++;
                    }

                if (((walkable / (double)checkCount) * 100) >= walkabilityMinimumPercentage)
                    return startTile.Position;

                if (--triesAllowed == 0)
                    return null;
            }
        }

        #region Helpers

        private bool AddEntityIfItDoesntExist(Entity entity)
        {
            if (this[entity.Position] != null)
                return false;

            entities.Add(entity);

            #region Update bounds if needed

            if (entity.X < TerrainBounds.X.Min)
                TerrainBounds.X.Min = entity.X;
            if (entity.Y < TerrainBounds.Y.Min)
                TerrainBounds.Y.Min = entity.Y;
            if (entity.X > TerrainBounds.X.Max)
                TerrainBounds.X.Max = entity.X;
            if (entity.Y > TerrainBounds.Y.Max)
                TerrainBounds.Y.Max = entity.Y;

            #endregion

            return true;
        }

        private bool UpdateEntity(Entity entity)
        {
            var entityToRemove = this[entity.Position];

            if (entityToRemove == null)
                return false;

            entities.Remove(entityToRemove);

            entities.Add(entity);
            return true;
        }

        #endregion

        #endregion Methods
    }
}
