using System;
using System.Collections.Generic;
//using GameMazeCreator_01;


namespace GameMazeCreator_01
{
	public class BasicMazeCreator
	{
		int width, height;
		string mode;
		int seed;

		int[,] grid;

		#region construction
		public BasicMazeCreator () : this(20, 20, "random")
		{

		}

		public BasicMazeCreator (int width, int height, string mode)
		{
			this.width = width;
			this.height = height;
			this.mode = mode;

			//this.grid = new int[this.height , this.width];
		}

		public int GetWidth () {
			return this.width;
		}
		public int GetHeight () {
			return this.height;
		}
		#endregion
		/*
		//static int N = 1, S = 2, E = 4, W = 8;

		// arry sort by E W N S
		//
		//static int[] DR = new int[] {N, S, E, W };
		//static Dictionary<int, int> DX = new Dictionary<int, int> {{E, 1}, {W, -1}, {N, 0}, {S, 0}};
		//static Dictionary<int, int> DY = new Dictionary<int, int> {{E, 0}, {W, 0}, {N, -1}, {S, 1}};
		//static Dictionary<int, int> OPPOSITE = new Dictionary<int, int> {{E, W}, {W, E}, {N, S}, {S, N}};
		*/

		public int[,] GrowingTree_Maze_Old()
		{
			int height = this.height;
			int width = this.width;
			int[,] grid = new int[height, width];

			List<Point> Points = new List<Point> ();

			int x = MazeCommon.GetRandom().Next(width), y = MazeCommon.GetRandom().Next(height);
			Points.Add (new Point (x, y));

			while (Points.Count != 0)
			{
				int index = Points.Count;
				x = Points [index - 1].x;
				y = Points [index - 1].y;

				List<int> tempDR = new List<int> ();
				foreach (int t_i in MazeCommon.DR)
					tempDR.Add (t_i);

				while (tempDR.Count > 0)
				{
					int i = (new Random ()).Next (tempDR.Count);
					//System.Console.Write (i);
					System.Threading.Thread.Sleep (1);
					int nx = x + MazeCommon.DX[tempDR[i]], ny = y + MazeCommon.DY[tempDR[i]];
					if (nx >= 0 && ny >= 0 && nx < width && ny < height && grid [ny, nx] == 0) 
					{
						grid [y, x] |= tempDR[i];
						grid [ny, nx] |= MazeCommon.OPPOSITE [tempDR[i]];
						Points.Add (new Point (nx, ny));
						index = -1;

						break;
					}
					tempDR.RemoveAt (i);

				}

				if (index > 0) Points.RemoveAt (index - 1);

			}

			return grid;
		}
			

		public Maze GrowingTree_Maze() 
		{
			return GrowingTree_Maze(new Maze(this.width, this.height));
			/*

			int height = this.height;
			int width = this.width;
			Maze maze = new Maze (width, height);
			//int[,] grid = new int[height, width];

			List<Point> Points = new List<Point> ();

			int x = MazeCommon.GetRandom().Next(width), y = MazeCommon.GetRandom().Next(height);
			Points.Add (new Point (x, y));

			while (Points.Count != 0)
			{
				int index = Points.Count;
				x = Points [index - 1].x;
				y = Points [index - 1].y;

				List<int> tempDR = new List<int> ();
				foreach (int t_i in MazeCommon.DR)
					tempDR.Add (t_i);

				while (tempDR.Count > 0)
				{
					int i = (new Random ()).Next (tempDR.Count);
					//System.Console.Write (i);
					System.Threading.Thread.Sleep (1);
					int nx = x + MazeCommon.DX[tempDR[i]], ny = y + MazeCommon.DY[tempDR[i]];
					if (nx >= 0 && ny >= 0 && nx < width && ny < height && maze.maze [ny, nx].visited == false) 
					{
						maze.maze [y, x].visited = true;
						maze.maze [y, x].direction |= tempDR [i];
						maze.maze [ny, nx].visited = true;
						maze.maze [ny, nx].direction |= MazeCommon.OPPOSITE [tempDR[i]];
						Points.Add (new Point (nx, ny));
						index = -1;

						break;
					}
					tempDR.RemoveAt (i);

				}

				if (index > 0) Points.RemoveAt (index - 1);

			}

			return maze;*/
		}

		public Maze GrowingTree_Maze(Maze maze)
		{
			int height = maze.maze.GetLength (0);
			int width =  maze.maze.GetLength (1);

			List<Point> Points = new List<Point> ();

			int x = MazeCommon.GetRandom().Next(width), y = MazeCommon.GetRandom().Next(height);
			Points.Add (new Point (x, y));

			while (Points.Count != 0)
			{
				int index = Points.Count;
				x = Points [index - 1].x;
				y = Points [index - 1].y;

				List<int> tempDR = new List<int> ();
				foreach (int t_i in MazeCommon.DR)
					tempDR.Add (t_i);

				while (tempDR.Count > 0)
				{
					int i = (new Random ()).Next (tempDR.Count);
					//System.Console.Write (i);
					System.Threading.Thread.Sleep (1);
					int nx = x + MazeCommon.DX[tempDR[i]], ny = y + MazeCommon.DY[tempDR[i]];
					if (nx >= 0 && ny >= 0 && nx < width && ny < height && maze.maze [ny, nx].visited == false) 
					{
						maze.maze [y, x].visited = true;
						maze.maze [y, x].direction |= tempDR [i];
						maze.maze [ny, nx].visited = true;
						maze.maze [ny, nx].direction |= MazeCommon.OPPOSITE [tempDR[i]];
						Points.Add (new Point (nx, ny));
						index = -1;

						break;
					}
					tempDR.RemoveAt (i);

				}

				if (index > 0) Points.RemoveAt (index - 1);

			}

			return maze;
		}


		#region about blocks
		//先不处理，可能不用Random blocks都可以。
		int[,] InitGridWithBlocks(int width, int height, List<Block> blocks) {
			int[,] grid = new int[height, width];
			if (blocks.Count == 0)
				blocks = GetRandomBlocks (width, height, 6);
			return grid;
		}
		//先不处理，可能不用Random blocks都可以。
		List<Block> GetRandomBlocks(int width, int height, int split) {
			List<Block> result = new List<Block> ();

			if (split <= 3 || width < split || height < split)
				return result;

			int xNum = width / split;
			int yNum = height / split;

			for (int i = 0; i < yNum; i++) {
				for (int k = 0; k < xNum; k++) {
					Block block = new Block ();
					block.anchor.y = MazeCommon.GetRandom ().Next (i * split, (i + 1) * split - 1);
					block.anchor.x = MazeCommon.GetRandom ().Next (k * split, (k + 1) * split - 1);

					for (;;){
						block.height = MazeCommon.GetRandom ().Next (2, 4);
						if (block.anchor.y + block.height - 1 < split) {
							break;
						}
					}
					for (;;){
						block.width = MazeCommon.GetRandom ().Next (2, 4);
						if (block.anchor.x + block.width - 1 < split) {
							break;
						}
					}
					result.Add (block);

				}
			}

			return result;
		}
		#endregion
	}

	public struct Point
	{
		public int x, y;
		public Point(int x, int y)
		{
			this.x = x;
			this.y = y;
		}
	}

	public struct Block {
		public int level; // 1 is space, 2 is blocker, 3 is water
		public int[,] block;
		public int width;
		public int height;
		public Point anchor;
	}

	public struct Cell {
		//public Point point;
		public int direction;
		public bool visited;
		public int[,] block;
		public int level;
	}

	public struct Maze {
		public Cell[,] maze;
		public Cell[,] terrainMaze;
		public Space[,] spaces;
		public Dictionary<int, Maze> neighborMazes;
		public Maze(int width, int height) {
			this.maze = new Cell[height, width];
			for (int i = 0; i < height; i++) {
				for (int j = 0; j < width; j++) {
					this.maze [i, j].direction = 0;
					this.maze [i, j].visited = false;
					this.maze [i, j].level = 0;
				}
			}
			this.terrainMaze = new Cell[height * 2, width * 2];
			this.spaces = new Space[0, 0];
			this.neighborMazes = new Dictionary<int, Maze> ();
		}
	}
	
}

