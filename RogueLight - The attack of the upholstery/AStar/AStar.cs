using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace RogueLight___The_attack_of_the_upholstery
{
    static class AStar
    {
        public static int TileSize = 30;

        public static List<TilePath> Pathfinding(Point Start, Point Target, List<Rectangle> Boundaries, Rectangle Hitbox)
        {
            var start = new Tile();
            var finish = new Tile();

            start.Pos.Y = Start.Y / TileSize;
            start.Pos.X = Start.X / TileSize;

            finish.Pos.Y = Target.Y / TileSize;
            finish.Pos.X = Target.X / TileSize;

            start.SetDistance(finish.Pos.X, finish.Pos.Y);

            var activeTiles = new List<Tile>();
            activeTiles.Add(start);
            var visitedTiles = new List<Tile>();


            // runs this loop is anything is in the activetiles list
            while (activeTiles.Any())
            {
                var checkTile = activeTiles.OrderBy(x => x.fCost).ThenBy(x => x.hCost).First();

                if (checkTile.Pos == finish.Pos)
                {
                    List<TilePath> ShortestPath = new List<TilePath>();
                    var tile = checkTile;

                    while (tile != null)
                    {
                        ShortestPath.Add(new TilePath(tile.Pos.X * TileSize, tile.Pos.Y * TileSize));
                        tile = tile.Parent;
                    }
                    ShortestPath.Reverse();
                    return ShortestPath;
                }

                visitedTiles.Add(checkTile);
                activeTiles.Remove(checkTile);

                var walkableTiles = GetWalkableTiles(checkTile, finish, Boundaries, Hitbox);

                foreach (var walkableTile in walkableTiles)
                {
                    //We have already visited this tile so we don't need to do so again!
                    if (visitedTiles.Any(x => x.Pos == walkableTile.Pos))
                        continue;

                    //It's already in the active list, but that's OK, maybe this new tile has a better value (e.g. We might zigzag earlier but this is now straighter). 
                    if (activeTiles.Any(x => x.Pos == walkableTile.Pos))
                    {
                        var existingTile = activeTiles.First(x => x.Pos == walkableTile.Pos);
                        if (existingTile.fCost > walkableTile.fCost)
                        {
                            activeTiles.Remove(existingTile);
                            activeTiles.Add(walkableTile);
                        }
                    }
                    else
                    {
                        //We've never seen this tile before so add it to the list. 
                        activeTiles.Add(walkableTile);
                    }
                }
            }
            
            {
                List<TilePath> ShortestPath = new List<TilePath>(); 
                var tile = visitedTiles.OrderBy(x => x.gCost).First();

                while (tile != null)
                {
                    ShortestPath.Add(new TilePath(tile.Pos.X * TileSize, tile.Pos.Y * TileSize));
                    tile = tile.Parent;
                }
                ShortestPath.Reverse();
                return ShortestPath;
            }
        }

        private static List<Tile> GetWalkableTiles(Tile currentTile, Tile targetTile, List<Rectangle> Boundaries, Rectangle Hitbox)
        {
            var possibleTiles = new List<Tile>();

            bool IsTileLeft = false;
            bool IsTileRight = false;
            bool IsTileUp = false;
            bool IsTileDown = false;

            bool IsTileUpRight = false;
            bool IsTileUpLeft = false;
            bool IsTileDownRight = false;
            bool IsTileDownLeft = false;


            // do intersect check here instead so doesnt put clashing tile in 
            foreach (Rectangle Boundary in Boundaries)
            {
                Rectangle TileCheckDown = new Rectangle(currentTile.Pos.X * TileSize, currentTile.Pos.Y * TileSize - TileSize, Hitbox.Width, Hitbox.Height);
                Rectangle TileCheckUp = new Rectangle(currentTile.Pos.X * TileSize, currentTile.Pos.Y * TileSize + TileSize, Hitbox.Width, Hitbox.Height);
                Rectangle TileCheckLeft = new Rectangle(currentTile.Pos.X * TileSize - TileSize, currentTile.Pos.Y * TileSize, Hitbox.Width, Hitbox.Height);
                Rectangle TileCheckRight = new Rectangle(currentTile.Pos.X * TileSize + TileSize, currentTile.Pos.Y * TileSize, Hitbox.Width, Hitbox.Height);

                Rectangle TileCheckUpRight = new Rectangle(currentTile.Pos.X * TileSize + TileSize, currentTile.Pos.Y * TileSize + TileSize, Hitbox.Width, Hitbox.Height);
                Rectangle TileCheckUpLeft = new Rectangle(currentTile.Pos.X * TileSize - TileSize, currentTile.Pos.Y * TileSize + TileSize, Hitbox.Width, Hitbox.Height);
                Rectangle TileCheckDownLeft = new Rectangle(currentTile.Pos.X * TileSize - TileSize, currentTile.Pos.Y * TileSize - TileSize, Hitbox.Width, Hitbox.Height);
                Rectangle TileCheckDownRight = new Rectangle(currentTile.Pos.X * TileSize + TileSize, currentTile.Pos.Y * TileSize - TileSize, Hitbox.Width, Hitbox.Height);

                if (Boundary.Intersects(TileCheckDown))
                    IsTileDown = true;

                if (Boundary.Intersects(TileCheckUp))
                    IsTileUp = true;

                if (Boundary.Intersects(TileCheckLeft))
                    IsTileLeft = true;

                if (Boundary.Intersects(TileCheckRight))
                    IsTileRight = true;

                if (Boundary.Intersects(TileCheckUpLeft))
                    IsTileUpLeft = true;

                if (Boundary.Intersects(TileCheckUpRight))
                    IsTileUpRight = true;

                if (Boundary.Intersects(TileCheckDownLeft))
                    IsTileDownLeft = true;

                if (Boundary.Intersects(TileCheckDownRight))
                    IsTileDownRight = true;

            }
            if (!IsTileDown)
                possibleTiles.Add(new Tile { Pos = new Point(currentTile.Pos.X, currentTile.Pos.Y - 1), Parent = currentTile, gCost = currentTile.gCost + 10 });

            if (!IsTileUp)
                possibleTiles.Add(new Tile { Pos = new Point(currentTile.Pos.X, currentTile.Pos.Y + 1), Parent = currentTile, gCost = currentTile.gCost + 10 });

            if (!IsTileLeft)
                possibleTiles.Add(new Tile { Pos = new Point(currentTile.Pos.X - 1, currentTile.Pos.Y), Parent = currentTile, gCost = currentTile.gCost + 10 });

            if (!IsTileRight)
                possibleTiles.Add(new Tile { Pos = new Point(currentTile.Pos.X + 1, currentTile.Pos.Y), Parent = currentTile, gCost = currentTile.gCost + 10 });

            if (!IsTileUpLeft)
                possibleTiles.Add(new Tile { Pos = new Point(currentTile.Pos.X - 1, currentTile.Pos.Y + 1), Parent = currentTile, gCost = currentTile.gCost + 14 });

            if (!IsTileUpRight)
                possibleTiles.Add(new Tile { Pos = new Point(currentTile.Pos.X + 1, currentTile.Pos.Y + 1), Parent = currentTile, gCost = currentTile.gCost + 14 });

            if (!IsTileDownLeft)
                possibleTiles.Add(new Tile { Pos = new Point(currentTile.Pos.X - 1, currentTile.Pos.Y - 1), Parent = currentTile, gCost = currentTile.gCost + 14 });

            if (!IsTileDownRight)
                possibleTiles.Add(new Tile { Pos = new Point(currentTile.Pos.X + 1, currentTile.Pos.Y - 1), Parent = currentTile, gCost = currentTile.gCost + 14 });


            possibleTiles.ForEach(tile => tile.SetDistance(targetTile.Pos.X, targetTile.Pos.Y));

            return possibleTiles

            .Where(tile => tile.Pos.X >= 0 && tile.Pos.X <= Globals.ScreenRectangle.Width / TileSize)
            .Where(tile => tile.Pos.Y >= 0 && tile.Pos.Y <= Globals.ScreenRectangle.Height / TileSize)

            .ToList();
        }
    }
}
