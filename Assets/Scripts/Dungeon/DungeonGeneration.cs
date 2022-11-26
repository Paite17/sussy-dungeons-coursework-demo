using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

public class DungeonGeneration : MonoBehaviour
{

    [SerializeField] public int numberOfRooms;
    [SerializeField] private TileBase obstacleTile;
    [SerializeField] private int numberOfObstacles;
    [SerializeField] private Vector2Int[] possibleObstacleSizes;
    [SerializeField] private GameObject goalPrefab;
    [SerializeField] private GameObject chestPrefab;
    // why yes i serialize public variables how could you tell?
    [SerializeField] public int dungeonLevel;
    [SerializeField] public int dungeonFloor;

    private Room[,] rooms;

    private Room currentRoom;

    private static DungeonGeneration instance = null;

    public AudioSource dungeonMusic;

    [SerializeField] public bool musicAlreadyPlaying = false;


    private bool fileExists = false;

    private Scene currentScene;

    private string sceneName;


    // prevent dungeon being overwritten on restart
    private void Awake()
    {
        currentScene = SceneManager.GetActiveScene();
        sceneName = currentScene.name;

        if (instance == null && sceneName == "MainScene")
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
            this.currentRoom = GenerateDungeon();
        }
        else
        {
            string roomPrefabName = instance.currentRoom.PrefabName();
            GameObject roomObject = (GameObject)Instantiate(Resources.Load(roomPrefabName));
            Tilemap tilemap = roomObject.GetComponentInChildren<Tilemap>();
            instance.currentRoom.AddPopulationToTilemap(tilemap, instance.obstacleTile);
            
            Destroy(this.gameObject);
        }
    }
    
    // temp place for saving location please lewis make a menu for this!
    private void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.Escape))
        {
            SaveDungeon();
            Debug.Log("Saved Dungeon");
        } */
    }

    // Start is called before the first frame update
    void Start()
    {
        // i hate making saving systems its so much easier in clickteam
        SaveSystem.CreateDungeonFile(this);
        fileExists = SaveSystem.DoesDungeonFileExist(fileExists);
        Debug.Log(fileExists);
        if (fileExists == true)
        {
            LoadDungeon();
        }
        else
        {
            SaveDungeon();
        }

        // initialise the dungeon
        this.currentRoom = GenerateDungeon();
        string roomPrefabName = this.currentRoom.PrefabName();
        GameObject roomObject = (GameObject)Instantiate(Resources.Load(roomPrefabName));
        Tilemap tilemap = roomObject.GetComponentInChildren<Tilemap>();
        this.currentRoom.AddPopulationToTilemap(tilemap, this.obstacleTile);
        //FindObjectOfType<AudioManager>().Play("Dungeon Music 1");
    }

    // saving dungeon and floor level to file
    private void SaveDungeon()
    {
        Debug.Log("SaveDungeon() was called in DungeonGeneration");
        SaveSystem.SaveDungeon(this);
    }

    // load dungeon - call this on start please mr lewis fart poops
    private void LoadDungeon()
    {
        DungeonData dungeon = SaveSystem.LoadDungeon();

        dungeonFloor = dungeon.dungeonFloor;
        dungeonLevel = dungeon.dungeonLevel;
        numberOfRooms = dungeon.amountOfRooms;
    }
    private Room GenerateDungeon()
    {
        Room chestRooms = null;
        int gridSize = 3 * numberOfRooms;
        rooms = new Room[gridSize, gridSize];

        Vector2Int initialRoomCoordinate = new Vector2Int((gridSize / 2) - 1, (gridSize / 2) - 1);

        Queue<Room> roomsToCreate = new Queue<Room>();
        roomsToCreate.Enqueue(new Room(initialRoomCoordinate.x, initialRoomCoordinate.y));
        List<Room> createdRooms = new List<Room>();

        while (roomsToCreate.Count > 0 && createdRooms.Count < numberOfRooms)
        {
            Room currentRoom = roomsToCreate.Dequeue();
            this.rooms[currentRoom.roomCoordinate.x, currentRoom.roomCoordinate.y] = currentRoom;
            createdRooms.Add(currentRoom);
            AddNeighbours(currentRoom, roomsToCreate);
        }

        int maximumDistanceToInitialRoom = 0;
        Room finalRoom = null;
        GameObject[] chestPrefabs = { this.chestPrefab };
        // these two chest variables need to be defined so that the game doesn't spawn chests everywhere
        int maxChestLimit = 1;
        int currentNumberOfChests = 0;
        System.Random random = new System.Random();

        // connect each room
        foreach (Room room in createdRooms)
        {
            List<Vector2Int> neighbourCoordinates = room.NeighbourCoordinates();
            foreach (Vector2Int coordinate in neighbourCoordinates)
            {
                Room neighbour = this.rooms[coordinate.x, coordinate.y];
                if (neighbour != null)
                {
                    room.Connect(neighbour);
                }
            }
            room.PopulateObstacles(this.numberOfObstacles, this.possibleObstacleSizes);

            // checks if chest can be placed in each room
            var roomCheck = random.Next(2) == 1;
            if (roomCheck == true && currentNumberOfChests < maxChestLimit)
            {
                chestRooms = room;
                chestRooms.PopulatePrefabs(1, chestPrefabs);
                currentNumberOfChests = currentNumberOfChests + 1;
            }
            else
            {
                chestRooms = null;
            }

            // looks for furthest room to place goal prefab
            int distanceToInitialRoom = Mathf.Abs(room.roomCoordinate.x - initialRoomCoordinate.x) + Mathf.Abs(room.roomCoordinate.y - initialRoomCoordinate.y);
            if (distanceToInitialRoom > maximumDistanceToInitialRoom)
            {
                maximumDistanceToInitialRoom = distanceToInitialRoom;
                finalRoom = room;
            }

        }

        // TODO: make chestAmount a random range that differs based on floorNumber

       
        
        GameObject[] goalPrefabs = { this.goalPrefab };
        finalRoom.PopulatePrefabs(1, goalPrefabs);

        return this.rooms[initialRoomCoordinate.x, initialRoomCoordinate.y];
    }


    // adds neighbouring rooms to the dungeon
    private void AddNeighbours(Room currentRoom, Queue<Room> roomsToCreate)
    {
        List<Vector2Int> neighbourCoordinates = currentRoom.NeighbourCoordinates();
        List<Vector2Int> availableNeighbours = new List<Vector2Int>();

        foreach (Vector2Int coordinate in neighbourCoordinates)
        {
            if (this.rooms[coordinate.x, coordinate.y] == null)
            {
                availableNeighbours.Add(coordinate);
            }
        }

        int numberOfNeighbours = (int)UnityEngine.Random.Range(1, availableNeighbours.Count);

        for (int neighbourIndex = 0; neighbourIndex < numberOfNeighbours; neighbourIndex++)
        {
            float randomNumber = UnityEngine.Random.value;
            float roomFrac = 1f / (float)availableNeighbours.Count;
            Vector2Int chosenNeighbour = new Vector2Int(0, 0);

            foreach (Vector2Int coordinate in availableNeighbours)
            {
                if (randomNumber < roomFrac)
                {
                    chosenNeighbour = coordinate;
                    break;
                }
                else
                {
                    roomFrac += 1f / (float)availableNeighbours.Count;
                }
            }
            roomsToCreate.Enqueue(new Room(chosenNeighbour));
            availableNeighbours.Remove(chosenNeighbour);
        }
    }

    // debug method for testing if dungeon generates correctly
    private void PrintGrid()
    {
        for (int rowIndex = 0; rowIndex < this.rooms.GetLength(1); rowIndex++)
        {
            string row = "";
            for (int columnIndex = 0; columnIndex < this.rooms.GetLength(0); columnIndex++)
            {
                if (this.rooms[columnIndex, rowIndex] == null)
                {
                    row += "X";
                }
                else
                {
                    row += "R";
                }
            }
            Debug.Log(row);
        }
    }

    public Room CurrentRoom()
    {
        return this.currentRoom;
    }

    public void MoveToRoom(Room room)
    {
        this.currentRoom = room;
    }

    // re-generates dungeon when called
    public void ResetDungeon()
    {
        SaveDungeon();

        // I'm pretty sure this only gets called when the goal is reached on the current floor so I could just add to the floor and/or level values here :3
        dungeonFloor++;
        // also making the room get bigger the further in you go but only to a certain degree
        // if i come back to this game and add more dungeons i might increase the cap
        if (numberOfRooms <= 15)
        {
            numberOfRooms++;
        }

        this.currentRoom = GenerateDungeon();
        
    }
}
