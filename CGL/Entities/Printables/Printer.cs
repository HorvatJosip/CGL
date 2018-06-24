using System;
using System.Collections.Generic;
using System.Linq;

using CGL.Entities;

namespace CGL
{
    /// <summary>
    /// Used to print entities in a specified area
    /// </summary>
    public class Printer
    {
        #region Fields

        private Rectangle renderedArea;
        private IDictionary<Position, Entity> entities;

        #endregion

        #region Properties

        #region Indexers

        /// <summary>
        /// Gets the entity at the given position. If there isn't an entity there,
        /// null is returned
        /// </summary>
        /// <param name="x">X coordinate of the entity to find</param>
        /// <param name="y">Y coordinate of the entity to find</param>
        public Entity this[int x, int y] => this[new Position(x, y)];

        /// <summary>
        /// Gets the entity at the given position. If there isn't an entity there,
        /// null is returned
        /// </summary>
        /// <param name="position">Position of the entity to find</param>
        public Entity this[Position position]
        {
            get
            {
                if (position == null || entities == null)
                    return null;

                entities.TryGetValue(position, out Entity entity);

                return entity;
            }
        }

        #endregion

        /// <summary>
        /// Sets the entity collection that is used for drawing
        /// </summary>
        public IEnumerable<Entity> Entities
        {
            set { this.entities = value?.ToDictionary(entity => entity.Position); }
        }

        /// <summary>
        /// If there is an entity within the drawable area that isn't within the printable bounds,
        /// it will be drawn with this
        /// </summary>
        public Graphics EmptyCharGraphics { get; set; } = new Graphics(ConsoleColor.Red, 'X');

        /// <summary>
        /// Bounds of the area that can be printed
        /// </summary>
        public Bounds2D PrintableBounds { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Generates a <see cref="Printer"/> instance with the specified rendered area and printable bounds
        /// </summary>
        /// <param name="renderedArea">Area that is printed on the screen</param>
        /// <param name="printableBounds">Bounds of the area that can be printed</param>
        /// <exception cref="ArgumentNullException"/>
        public Printer(Rectangle renderedArea, Bounds2D printableBounds)
        {
            Utils.ThrowIfNull((renderedArea, nameof(renderedArea)), (printableBounds, nameof(printableBounds)));

            this.renderedArea = renderedArea;
            PrintableBounds = printableBounds;
        }

        /// <summary>
        /// Generates a <see cref="Printer"/> instance with the specified rendered area, printable bounds and entities
        /// </summary>
        /// <param name="renderedArea">Area that is printed on the screen</param>
        /// <param name="printableBounds">Bounds of the area that can be printed</param>
        /// <param name="entities">Entities that will be printed</param>
        /// <exception cref="ArgumentNullException"/>
        public Printer(Rectangle renderedArea, Bounds2D printableBounds, IEnumerable<Entity> entities)
            : this(renderedArea, printableBounds) => Entities = entities;

        /// <summary>
        /// Generates a <see cref="Printer"/> instance with the specified rendered area.
        /// Printable bounds are the same as the rendered area (no limits are set on printing)
        /// </summary>
        /// <param name="renderedArea">Area that is printed on the screen</param>
        /// <exception cref="ArgumentNullException"/>
        public Printer(Rectangle renderedArea)
            : this(renderedArea, new Bounds2D(renderedArea)) { }

        /// <summary>
        /// Generates a <see cref="Printer"/> instance with the specified rendered area and entities.
        /// Printable bounds are the same as the rendered area (no limits are set on printing)
        /// </summary>
        /// <param name="renderedArea">Area that is printed on the screen</param>
        /// <param name="entities">Entities that will be printed</param>
        /// <exception cref="ArgumentNullException"/>
        public Printer(Rectangle renderedArea, IEnumerable<Entity> entities)
            : this(renderedArea, new Bounds2D(renderedArea), entities) { }

        #endregion

        #region Methods

        /// <summary>
        /// Clears the drawable / rendered area
        /// </summary>
        public void ClearDrawableArea()
        {
            for (int y = renderedArea.Y; y <= renderedArea.Bottom; y++)
            {
                Console.SetCursorPosition(renderedArea.X, y);
                for (int x = renderedArea.X; x <= renderedArea.Right; x++)
                {
                    Console.Write(' ');
                }

                if (y != renderedArea.Bottom)
                    Console.WriteLine();
            }
        }

        #region Drawing

        /// <summary>
        /// Draws all the entities stored internally (either using <see cref="DrawEntities(IEnumerable{Entity}, bool?)"/>
        /// that has the option to set the collection or <see cref="Draw(Entity)"/> that adds the entity to that position
        /// or updates the existing one)
        /// </summary>
        public bool DrawAllStoredEntities()
        {
            if (entities == null)
                return false;

            return DrawEntities(entities.Values, false);
        }

        /// <summary>
        /// Draws the given entities on the screen (the ones that are within the drawable / rendered area).
        /// If an entity is in the printable bounds, its graphics will be used, 
        /// otherwise the <see cref="EmptyCharGraphics"/> will be used
        /// </summary>
        /// <param name="entities">Entities to draw</param>
        /// <param name="setEntitiesProp">Should the entities collection be set? if this is true,
        /// it will be set, if this is null, it will be set only if it is null, otherwise, it won't be set</param>
        /// <exception cref="ArgumentNullException"/>
        public bool DrawEntities(IEnumerable<Entity> entities, bool? setEntitiesProp)
        {
            entities.ThrowIfNull(nameof(entities));

            if (setEntitiesProp == true || (setEntitiesProp == null && entities == null))
                Entities = entities;

            for (int y = renderedArea.Y; y <= renderedArea.Bottom; y++)
            {
                Console.SetCursorPosition(renderedArea.X, y);

                for (int x = renderedArea.X; x <= renderedArea.Right; x++)
                {
                    var entity = entities.FirstOrDefault(e => e.Position == new Position(x, y));

                    if (entity != null)
                        if (!Draw(entity, false))
                            return false;
                }

                if (y != renderedArea.Bottom)
                    Console.WriteLine();
            }

            Console.ResetColor();
            return true;
        }

        /// <summary>
        /// Draws an entity on the screen if it is within the drawable / rendered area.
        /// If it is in the printable bounds, its graphics will be used, 
        /// otherwise the <see cref="EmptyCharGraphics"/> will be used
        /// </summary>
        /// <param name="entity">Entity to draw</param>
        /// <exception cref="ArgumentNullException"/>
        public bool Draw(Entity entity) => Draw(entity, true);

        //Helper
        private bool Draw(Entity entity, bool editCollection)
        {
            entity.ThrowIfNull(nameof(entity));

            if (editCollection)
            {

                if (entities == null)
                    entities = new Dictionary<Position, Entity>();

                if (entities.ContainsKey(entity.Position))
                    entities[entity.Position] = entity;
                else
                    entities.Add(entity.Position, entity);

            }

            Console.SetCursorPosition(entity.X, entity.Y);

            return entity.Position.InsideRectangle(renderedArea)
                ? Utils.Draw(entity.Position.InsideBounds(PrintableBounds)
                    ? entity.Graphics
                    : EmptyCharGraphics, false)
                : false;
        } 

        #endregion

        #endregion
    }
}
