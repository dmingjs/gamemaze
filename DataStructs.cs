using System;
using System.Collections.Generic;

namespace GameMazeCreator_01
{
	public class DataStructs
	{
		public DataStructs ()
		{
		}
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
			this.terrainMaze = new Cell[0, 0];
			this.spaces = new Space[0, 0];
			this.neighborMazes = new Dictionary<int, Maze> ();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="GameMazeCreator_01.Maze"/> struct.
		/// default is width == 9, height == 9;
		/// </summary>
		public Maze() : this (9, 9) {
		}
	}

	public struct LevelCell {
		public Point point;

		public MazeLevelCommon.CellType type;
		public int direction;
		public bool canStair;
		public bool isStair;
		public int level; // if isStair == false, use it as the level

		public Dictionary<int, List<DoorAdjvex>> neighbors;
		public Dictionary<int, LevelRange> levelRanges;

		public LevelCell() {
			this.point = new Point (-1, -1);

			this.type = MazeLevelCommon.CellType.UNDEFINE;
			this.direction = 0;
			this.canStair = false;
			this.isStair = false;
			this.level = -10;

			this.neighbors = new Dictionary<int, List<DoorAdjvex>> ();
			this.levelRanges = new Dictionary<int, LevelRange>();
		}
	}


		
	public struct Door {
		public Point coordinate;
		public Dictionary<int, List<DoorAdjvex>> neighbors;
		public Dictionary<int, LevelRange> directionLevelRange;
		public bool canStair;

		public Door(Point p) {
			this.coordinate = p;
			this.canStair = false;
			this.neighbors = new Dictionary<int, List<DoorAdjvex>> ();
			this.directionLevelRange = new Dictionary<int, LevelRange> ();
		}
	}
		
	public struct LevelRange {
		public int checkTimes;

		private int _maxLevel;
		public int maxLevel {
			get{ return _maxLevel; }
			set {
				if (value > 5) {
					_maxLevel = 5;
				} else {
					_maxLevel = value;
				}
			}
		}

		private int _minLevel;
		public int minLevel {
			get{ return _minLevel; }
			set {
				if (value < -5) {
					_minLevel = -5;
				} else {
					_minLevel = value;
				}
			}
		}

		public LevelRange (int min, int max) {
			this.checkTimes = 0;

			if (min < -5) min = -5;
			if (max > 5) max = 5;
			this._minLevel = min;
			this._maxLevel = max;
		}
	}

	public struct DoorAdjvex {
		public Point nextDoor;
		public bool isSpace;
		// the road should not store in the list, it should store in the queue.. 
		public Queue<Point> road;
		public int canStairCount; // just in road, not for the end point
		public DoorAdjvex () : this(new Point()){}

		public DoorAdjvex (Point point) {
			this.nextDoor = point;
			this.isSpace = false;
			this.canStairCount = 0;
			this.road = new Queue<Point> ();
		}
	}


	public struct Space {
		//public int x;
		//public int y;
		public Point anchor;
		public int height;
		public int width;
		public int level;
		public List<Point> addOn; //save the add on points of space area. the coodinate is the actual point's coodinate of (y, x);
		public List<Point> paths;
		public Space(int x, int y, int width, int height){
			this.anchor = new Point (x, y);
			this.width = width;
			this.height = height;
			this.level = 0;
			this.addOn = new List<Point> ();
			this.paths = new List<Point> ();
		}

		public Space(int x, int y, int width, int height, int level) : this (x, y, width, height) {
			this.level = level;
		}
	}

	public struct CellWithPoint {
		public Point point;
		public Cell cell;

		public CellWithPoint (Point p, Cell c) {
			this.cell = c;
			this.point = p;
		}
	}

}

