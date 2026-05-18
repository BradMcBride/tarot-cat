using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class IslandGenerator : Node
{
	private const int GridWidth      = 30;
	private const int GridHeight     = 30;
	private const int MinIslandSize  = 80;
	private const int MaxIslandSize  = 180;

	private static readonly Vector2I AtlasCoord = new Vector2I(1, 1);

	private static readonly Vector2I[] Directions =
	{
		new Vector2I( 0, -1),
		new Vector2I( 0,  1),
		new Vector2I(-1,  0),
		new Vector2I( 1,  0),
	};

	[Export] public TileMapLayer MainGround;

	private Random _rng = new Random();

	public override void _Ready()
	{
		var island = GenerateIsland();
		PaintTiles(island);
	}

	private HashSet<Vector2I> GenerateIsland()
	{
		int targetSize = _rng.Next(MinIslandSize, MaxIslandSize + 1);

		var start = new Vector2I(
			_rng.Next(GridWidth  / 4, GridWidth  * 3 / 4),
			_rng.Next(GridHeight / 4, GridHeight * 3 / 4)
		);

		var island   = new HashSet<Vector2I> { start };
		var frontier = new List<Vector2I>    { start };

		while (island.Count < targetSize && frontier.Count > 0)
		{
			int      fi       = _rng.Next(frontier.Count);
			Vector2I current  = frontier[fi];
			bool     expanded = false;

			foreach (var dir in Directions.OrderBy(_ => _rng.Next()))
			{
				Vector2I candidate = current + dir;

				if (IsInBounds(candidate) && !island.Contains(candidate))
				{
					island.Add(candidate);
					frontier.Add(candidate);
					expanded = true;
					break;
				}
			}

			if (!expanded)
				frontier.RemoveAt(fi);
		}

		return island;
	}

	private void PaintTiles(HashSet<Vector2I> tiles)
	{
		MainGround.Clear();
		foreach (var tile in tiles)
			MainGround.SetCell(tile, 0, AtlasCoord);
	}

	private static bool IsInBounds(Vector2I pos) =>
		pos.X >= 0 && pos.X < GridWidth &&
		pos.Y >= 0 && pos.Y < GridHeight;
}
