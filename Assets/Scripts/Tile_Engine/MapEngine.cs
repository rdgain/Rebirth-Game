using System;
using UnityEngine;
using Random = UnityEngine.Random;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Runtime.ConstrainedExecution;
using System.Text;

public class MapEngine : MonoBehaviour
{
	private class Objects
	{
		public Vector2 Position;
		public char Prefab;
		public string Name;

		public Objects(string s)
		{
			string[] line = s.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
			Prefab = char.Parse(line[0]);
			Position = new Vector2(int.Parse(line[1]), int.Parse(line[2]));
			Name = line[3];
		}
	}

	private class Doors
	{
		public Vector2 Position;
		public char Direction;

		public Doors(string s)
		{
			string[] line = s.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
			Position = new Vector2(float.Parse(line[0]), float.Parse(line[1]));
			Direction = char.Parse(line[2]);
		}
	}

	private class Room
	{
		private TextAsset roomData;

		private int width;
		private int height;
	    private int transposed;
	    private int flipHorizontal;
        private int flipVertical;
        private readonly int size; 
		public readonly Dictionary<char, string> Legend;
		public readonly string Name;
		private readonly char[,] background;
		public readonly List<Objects> Objects;
		public readonly List<Doors> Entries;
		public readonly List<Doors> Exits;
		public readonly List<string> Rules;


		public Room(string path)
		{
		    transposed = 0;
		    flipHorizontal = 0;
		    flipVertical = 0;

			roomData = (TextAsset)Resources.Load(path, typeof(TextAsset));

            string[] data = roomData.text.Replace("\r", "").Split(new string[] { "\n- " }, StringSplitOptions.RemoveEmptyEntries);

            Legend = new Dictionary<char, string>();
			List<string>  background = new List<string>();
			Objects = new List<Objects>();
			Exits = new List<Doors>();
			Entries = new List<Doors>();

			Rules = new List<string>();

			Name = data[0];

			for ( int i = 1; i < data.Length; i++ )
			{
				if (data[i].StartsWith("Legend"))
				{
					string[] legendData = data[i].Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);

					for (int j = 1; j < legendData.Length; j++)
					{
						string[] line = legendData[j].Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
						Legend.Add(char.Parse(line[0]), line[1]);
					}
				}
				if (data[i].StartsWith("Entries"))
				{
					string[] doorsData = data[i].Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);

					for (int j = 1; j < doorsData.Length; j++)
					{
						string[] line = doorsData[j].Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
						Entries.Add(new Doors(doorsData[j]));
					}
				}
				else if (data[i].StartsWith("Exits"))
				{
					string[] doorsData = data[i].Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
					
					for (int j = 1; j < doorsData.Length; j++)
					{
						Exits.Add(new Doors(doorsData[j]));
					}
				}
				else if (data[i].StartsWith("Background"))
				{
					string[] backgroundData = data[i].Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);

					for (int j = 1; j < backgroundData.Length; j++)
					{
						background.Add(backgroundData[j]);
					}
				}
				else if (data[i].StartsWith("Objects"))
				{
					string[] objectsData = data[i].Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);

					for (int j = 1; j < objectsData.Length; j++)
					{
						Objects.Add(new Objects(objectsData[j]));
					}
				}
				else if (data[i].StartsWith("Rules"))
				{
					string[] rulesData = data[i].Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);

					for (int j = 1; j < rulesData.Length; j++)
					{
						Rules.Add(rulesData[j]);
					}
				}
			}

			height = background.Count;
			width = 0;
			foreach (var line in background)
			{
				if (line.Length > width)
				{
					width = line.Length;
				}
			}

			size = Math.Max(width, height);
			this.background = new char[size, size];
			for (int x = 0; x < size; x++)
			{
				for (int y = 0; y < size; y++)
				{
					if (x < width && y < height)
					{
						this.background[x, y] = background[height-y-1][x];
					}
					else
					{
						this.background[x, y] = 'b';
					}
			}
			}
		}

		public int GetWidth()
		{
			return width;
		}

		public int GetHeight()
		{
			return height;
		}

		public char GetBackground(int x, int y)
		{
			return background[x, y];
		}

		public void Transpose()
		{
		    transposed = 1-transposed;

			for (int x = 0; x < size; x++)
			{
				for (int y = x; y < size; y++)
				{ 
					char c = background[x, y];
					background[x, y] = background[y, x];
					background[y, x] = c;
				}
			}

			for (int i = 0; i < Entries.Count; i++)
			{
				Entries[i].Position.Set(Entries[i].Position.y, Entries[i].Position.x);

				if (Entries[i].Direction == 'n')
				{
					Entries[i].Direction = 'w';
				}
				else if (Entries[i].Direction == 'w')
				{
					Entries[i].Direction = 'n';
				}
				else if (Entries[i].Direction == 's')
				{
					Entries[i].Direction = 'e';
				}
				else if (Entries[i].Direction == 'e')
				{
					Entries[i].Direction = 's';
				}
			}

			for (int i = 0; i < Exits.Count; i++)
			{
				Exits[i].Position.Set(Exits[i].Position.y, Exits[i].Position.x);

				if (Exits[i].Direction == 'n')
				{
					Exits[i].Direction = 'w';
				}
				else if (Exits[i].Direction == 'w')
				{
					Exits[i].Direction = 'n';
				}
				else if (Exits[i].Direction == 's')
				{
					Exits[i].Direction = 'e';
				}
				else if (Exits[i].Direction == 'e')
				{
					Exits[i].Direction = 's';
				}
			}

			for (int i = 0; i < Objects.Count; i++)
			{
				Objects[i].Position.Set(Objects[i].Position.y, Objects[i].Position.x);
			}

			int w = width;
			width = height;
			height = w;
		}

		public void FlipHorizontal()
		{
		    flipHorizontal = 1-flipHorizontal;

			for (int x = 0; x < width; x++)
			{
				for (int y = 0; y < height/2; y++)
				{
					char temp = background[x,y];
					background[x,y] = background[x,height - y - 1];
					background[x,height - y - 1] = temp;
				}
			}

			for (int i = 0; i < Entries.Count; i++)
			{
				Entries[i].Position.Set(Entries[i].Position.x, height - Entries[i].Position.y - 1);
				if (Entries[i].Direction == 'n')
				{
					Entries[i].Direction = 's';
				}
				else if (Entries[i].Direction == 's')
				{
					Entries[i].Direction = 'n';
				}
			}

			for (int i = 0; i < Exits.Count; i++)
			{
				Exits[i].Position.Set(Exits[i].Position.x, height - Exits[i].Position.y - 1);
				if (Entries[i].Direction == 'n')
				{
					Entries[i].Direction = 's';
				}
				else if (Entries[i].Direction == 's')
				{
					Entries[i].Direction = 'n';
				}
			}

			for (int i = 0; i < Objects.Count; i++)
			{
				Objects[i].Position.Set(Objects[i].Position.x, height - Objects[i].Position.y - 1);
			}
		}

		public void FlipVertical()
		{
		    flipVertical = 1-flipVertical;

			for (int x = 0; x < width/2; x++)
			{
				for (int y = 0; y < height; y++)
				{
					char temp = background[x,y];
					background[x,y] = background[width - x - 1,y];
					background[width - x - 1,y] = temp;
				}
			}

			for (int i = 0; i < Entries.Count; i++)
			{
				Entries[i].Position.Set(width - Entries[i].Position.x - 1, Entries[i].Position.y);
				if (Entries[i].Direction == 'w')
				{
					Entries[i].Direction = 'e';
				}
				else if (Entries[i].Direction == 'e')
				{
					Entries[i].Direction = 'w';
				}
			}

			for (int i = 0; i < Exits.Count; i++)
			{
				Exits[i].Position.Set(width - Exits[i].Position.x - 1, Exits[i].Position.y);
				if (Entries[i].Direction == 'w')
				{
					Entries[i].Direction = 'e';
				}
				else if (Entries[i].Direction == 'e')
				{
					Entries[i].Direction = 'w';
				}
			}

			for (int i = 0; i < Objects.Count; i++)
			{
				Objects[i].Position.Set(width - Objects[i].Position.x - 1, Objects[i].Position.y);
			}
		}

	    public int Orientation()
	    {
	        return flipHorizontal * 4 + flipVertical * 2 + transposed;
	    }
	}

	public TextAsset LevelData;
	public Player Player;

	private int Width;
	private int Height;
	private Dictionary<char, string> Legend;
	private string Name;
	private char[][] background;

	private List<string> roomNames;
	private List<string> usedRooms; 
	private Dictionary<string, Room> rooms;
	private Dictionary<Vector4, string> roomPositions;
	private Dictionary<string, Vector4> roomLocations;

	private Dictionary<string, string> levelInfo; 

	public void Awake()
	{
		LoadLevel();
		CreateMap();
	}

	public void LoadLevel()
	{
		rooms = new Dictionary<string, Room>();
		rooms.Add("Outside", new Room("Levels/Outside"));
		roomNames = new List<string>();
		roomPositions = new Dictionary<Vector4, string>();
		roomLocations = new Dictionary<string, Vector4>();
		levelInfo = new Dictionary<string, string>();
		usedRooms = new List<string>();

		Legend = new Dictionary<char, string>();

		string[] data = LevelData.text.Split(new string[] { "\n- " }, StringSplitOptions.RemoveEmptyEntries);

		Name = data[0];

		for (int i = 1; i < data.Length; i++)
		{
			if (data[i].StartsWith("Legend"))
			{
				string[] legendData = data[i].Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);

				for (int j = 1; j<legendData.Length; j++)
				{
					Legend.Add(legendData[j][0], legendData[j].Split(' ')[1]);
				}
			}
			else if (data[i].StartsWith("Rooms"))
			{
				string[] roomData = data[i].Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);

				for (int j = 1; j<roomData.Length; j++)
				{
					Room room = new Room(roomData[j]);
					roomNames.Add(room.Name);
					rooms.Add(room.Name, room);
				}
			}
			else if (data[i].StartsWith("Level"))
			{
				string[] levelData = data[i].Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);

				for (int j = 1; j < levelData.Length; j++)
				{
					levelInfo.Add(levelData[j].Split(' ')[0], levelData[j].Split(' ')[1]);
				}
			}
		}

		Height = int.Parse(levelInfo["Height"]);
		Width = int.Parse(levelInfo["Width"]);

		background = new char[Width][];
		for (int i = 0; i < Width; i++)
		{
			background[i] = new char[Height];
			for (int j = 0; j < Height; j++)
			{
				background[i][j] = ' ';
			}
		}
	}

	public void CreateMap()
	{
		int roomNumber = int.Parse (levelInfo["RoomNumber"]);

		usedRooms.Add(levelInfo["StartRoom"]);

		while (usedRooms.Count < roomNumber) {
			int random = Random.Range(0, roomNames.Count);
			if (!usedRooms.Contains(roomNames[random])) {
				usedRooms.Add(roomNames[random]);
			}
		}

	    Queue<Doors> exits = new Queue<Doors>();

		CreateRoom(int.Parse(levelInfo["StartRoomX"]), int.Parse(levelInfo["StartRoomX"]), usedRooms[0]);

		foreach (var exit in rooms[usedRooms[0]].Exits)
		{
			exits.Enqueue(exit);
		}

		for (int i = 1; i < roomNumber; i++)
		{
			Room room = rooms[usedRooms[i]];
			int door = Random.Range(0, room.Entries.Count);
			Doors entry = room.Entries[door];
			Doors exit = exits.Dequeue();

			switch (exit.Direction)
			{
				case 'n':
					switch (entry.Direction)
					{
						case 'n':
							room.FlipHorizontal();
							break;
						case 'w':
							room.Transpose();
							break;
						case 's':
							break;
						case 'e':
							room.Transpose();
							room.FlipHorizontal();
							break;
					}
					if (Random.value < 0.5)
					{
						room.FlipVertical();
					}
					break;
				case 'w':
					switch (entry.Direction)
					{
						case 'n':
							room.Transpose();
							break;
						case 'w':
							room.FlipVertical();
							break;
						case 's':
							room.Transpose();
							room.FlipVertical();
							break;
						case 'e':
							break;
					}
					if (Random.value < 0.5)
					{
						room.FlipHorizontal();
					}
					break;
				case 's':
					switch (entry.Direction)
					{
						case 'n':
							break;
						case 'w':
							room.Transpose();
							room.FlipHorizontal();
							break;
						case 's':
							room.FlipHorizontal();
							break;
						case 'e':
							room.Transpose();
							break;
					}
					if (Random.value < 0.5)
					{
						room.FlipVertical();
					}
					break;
				case 'e':
					switch (entry.Direction)
					{
						case 'n':
							room.Transpose();
							room.FlipVertical();
							break;
						case 'w':
							break;
						case 's':
							room.Transpose();
							break;
						case 'e':
							room.FlipVertical();
							break;
					}
					if (Random.value < 0.5)
					{
						room.FlipHorizontal();
					}
					break;
			}

			int x = 0;
			int y = 0;

		    int adjustment = 0;
		    int hack = 0;

            //primitive
		    do
		    {
		        //Debug.Log("Adjustment: " + adjustment);

                if (exit.Direction == 'n')
                {
                    x = (int)exit.Position.x - (int)entry.Position.x;
                    y = (int)exit.Position.y + int.Parse(levelInfo["RoomDistance"]) + adjustment - (int)entry.Position.y + 1;
                }
                else if (exit.Direction == 'w')
                {
                    x = (int)exit.Position.x - int.Parse(levelInfo["RoomDistance"]) - adjustment - (int)entry.Position.x - 1;
                    y = (int)exit.Position.y - (int)entry.Position.y;
                }
                else if (exit.Direction == 's')
                {
                    x = (int)exit.Position.x - (int)entry.Position.x;
                    y = (int)exit.Position.y - int.Parse(levelInfo["RoomDistance"]) - adjustment - (int)entry.Position.y - 1;
                }
                else if (exit.Direction == 'e')
                {
                    x = (int)exit.Position.x + int.Parse(levelInfo["RoomDistance"]) + adjustment - (int)entry.Position.x + 1;
                    y = (int)exit.Position.y - (int)entry.Position.y;
                }

		        adjustment++;
		        hack++;

		    } while (hack < 10 && !CheckEmplacement(x, y, usedRooms[i]));

            CreateRoom(x, y, usedRooms[i]);
            CreateAdjustment(exit.Position, entry.Position + new Vector2(x, y));
           
            foreach (var newExit in rooms[usedRooms[i]].Exits)
            {
                exits.Enqueue(newExit);
            }

        }

		CreateBackground();

		foreach (var room in usedRooms)
		{
			LoadRoom(room);
		}

	}

    private void CreateAdjustment(Vector2 start, Vector2 end)
    {
        if ((int)start.x == (int)end.x)
        {
            int x = (int)start.x;
            for (int y = (int)Math.Min(start.y, end.y) + 1; y < Math.Max(start.y, end.y); y++)
            {
                background[x+1][y] = 'w';
                background[x][y] = 'f';
                background[x-1][y] = 'w';
            }
        }
        else
        {
            int y = (int)start.y;
            for (int x = (int)Math.Min(start.x, end.x) + 1; x < Math.Max(start.x, end.x); x++)
            {
                background[x][y+1] = 'w';
                background[x][y] = 'f';
                background[x][y-1] = 'w';
            }
        }
    }

	private void CreateBackground()
	{
		Transform bgTransform;

		if (!transform.FindChild("Background"))
		{
		    GameObject bg = new GameObject {name = "Background"};
		    bg.transform.parent = transform;
			bgTransform = bg.transform;
		}
		else
		{
			bgTransform = transform.FindChild("Background");
		}

		for (int y = 0; y < Height; y++)
		{
			for (int x = 0; x < Width; x++)
			{
			    char c = background[x][y];

			    if (c == ' ') continue;
			    var tile = Load(c, rooms[RoomAt(x, y)]);
			    if (tile == null) continue;
			    Vector3 pos = new Vector3(x, y, 0.3f);
			    GameObject test = (GameObject) Instantiate(tile, pos, bgTransform.rotation);
			    test.transform.parent = bgTransform;
			}
		}
	}

	private GameObject Load(char c, Room room)
	{
		if (c == 0)
		{
			return null;
		}

		if (room.Legend.ContainsKey(c))
		{
			return (GameObject)Resources.Load(room.Legend[c]);
		}
		if (Legend.ContainsKey(c))
		{
			return (GameObject)Resources.Load(Legend[c]);
		}

		Debug.Log(c+" Not Found in: " + room.Name);
		return null;
	}

    private bool CheckEmplacement(int x, int y, string roomName)
    {
        Vector4 target = new Vector4(x, y, rooms[roomName].GetWidth() + x, rooms[roomName].GetHeight() + y);

        //Debug.Log("Target: " + target.ToString());

        foreach (var room in roomPositions.Keys)
        {
            if (!(target.x >= room.z || target.z <= room.x || target.y >= room.w || target.w <= room.y ))
            {
                //Debug.Log("Room: " + room.ToString());
                return false;
            }
        }

        return true;
    }

	public void CreateRoom(int x, int y, string roomName)
	{
		Room room = rooms[roomName];

		
		for (int i = x; i < room.GetWidth() + x; i++)
		{
			for (int j = y; j < room.GetHeight() + y; j++)
			{
				if (room.GetBackground(i - x, j - y) != ' ')
				{
					background[i][j] = room.GetBackground(i - x, j - y);
				}
			}
		}

	    Vector4 position = new Vector4(x, y, room.GetWidth() + x, room.GetHeight() + y);
        //Debug.Log(roomPositions.Count + position.ToString());

        roomPositions.Add(position, room.Name);
		roomLocations.Add(room.Name, position);

		//Debug.Log("Created: "+room.Name+" at: "+ x+ ":"+ y+"::" +(room.GetWidth() + x )+ ":" + (room.GetHeight() + y));

	    for (int i = 0; i < room.Exits.Count; i++)
	    {
	        room.Exits[i].Position.Set(room.Exits[i].Position.x + x, room.Exits[i].Position.y + y);
	    }
	}

	public void LoadRoom(string roomName)
	{
		int x = (int)roomLocations [roomName].x;
		int y = (int)roomLocations [roomName].y;

		Room room = rooms[roomName];

        if (!transform.FindChild(roomName))
        {
            GameObject roomObject = new GameObject();
            roomObject.name = roomName;
            roomObject.transform.parent = transform;
        }

        for (int i = 0; i < room.Objects.Count; i++)
	    {
	        Vector3 pos = new Vector3(room.Objects[i].Position.x + x, room.Objects[i].Position.y + y, 0);
	        GameObject ob = Load(room.Objects[i].Prefab, room);

	        if (ob != null)
	        {
	            GameObject ob2 = (GameObject) Instantiate(ob, pos, transform.rotation);
	            ob2.transform.name = room.Objects[i].Name;
	            ob2.transform.parent = transform.FindChild(roomName);
	        }
	    }
	}

	public void ReloadRoom()
	{
		string name = ActiveRoom();
		GameObject room = GameObject.Find(name);
		int x = room.transform.childCount;
		for (int i = 0; i< x; i++) {
			Destroy (room.transform.GetChild(i).gameObject);
		}

		LoadRoom (name);
	}

	public string ActiveRoom()
	{
		Vector3 pos = Player.GetPosition();

		return RoomAt(pos.x, pos.y);
	}

	public string RoomAt(float x, float y)
	{
	    if (roomPositions != null)
	    {
	        foreach (var room in roomPositions.Keys)
	        {
	            if (room.x <= x && x < room.z)
	            {
	                if (room.y <= y && y < room.w)
	                    return roomPositions[room];
	            }
	        }
	    }

	    return "Outside";
	}

    public List<String> RoomRules(string name)
    {
        return rooms[name].Rules;
    }

    public int RoomOrientation(string name)
    {
        return rooms[name].Orientation();
    }
}
