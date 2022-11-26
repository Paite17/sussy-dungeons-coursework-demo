using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Room 
{
    public Vector2Int roomCoordinate;
    public Dictionary<string, Room> neighbours;


    private string[,] population;

    private Dictionary<string, GameObject> name2Prefab;

    public Room(int xCoordinate, int yCoordinate)
    {
        this.roomCoordinate = new Vector2Int(xCoordinate, yCoordinate);
        this.neighbours = new Dictionary<string, Room>();
        this.population = new string[18, 10];

        for (int xIndex = 0; xIndex < 10; xIndex += 1)
        {
            for (int yIndex = 0; yIndex < 10; yIndex += 1)
            {
                this.population[xIndex, yIndex] = "";
            }
        }
        this.population[8, 5] = "Player";
        this.name2Prefab = new Dictionary<string, GameObject>();
    }

    public Room(Vector2Int roomCoordinate)
    {
        this.roomCoordinate = roomCoordinate;
        this.neighbours = new Dictionary<string, Room>();
        this.population = new string[18, 10];

        this.population = new string[18, 10];
        for (int xIndex = 0; xIndex < 18; xIndex += 1)
        {
            for (int yIndex = 0; yIndex < 10; yIndex += 1)
            {
                this.population[xIndex, yIndex] = "";
            }
        }
        this.population[8, 5] = "Player";
        this.name2Prefab = new Dictionary<string, GameObject>();
    }

    public List<Vector2Int> NeighbourCoordinates()
    {
        List<Vector2Int> neighbourCoordinates = new List<Vector2Int>();
        neighbourCoordinates.Add(new Vector2Int(this.roomCoordinate.x, this.roomCoordinate.y - 1));
        neighbourCoordinates.Add(new Vector2Int(this.roomCoordinate.x + 1, this.roomCoordinate.y));
        neighbourCoordinates.Add(new Vector2Int(this.roomCoordinate.x, this.roomCoordinate.y + 1));
        neighbourCoordinates.Add(new Vector2Int(this.roomCoordinate.x - 1, this.roomCoordinate.y));

        return neighbourCoordinates;
    }

    public void Connect(Room neighbour)
    {
        string direction = "";

        if (neighbour.roomCoordinate.y < this.roomCoordinate.y)
        {
            direction = "N";
        }
        if (neighbour.roomCoordinate.x > this.roomCoordinate.x)
        {
            direction = "E";
        }
        if (neighbour.roomCoordinate.y > this.roomCoordinate.y)
        {
            direction = "S";
        }
        if (neighbour.roomCoordinate.x < this.roomCoordinate.x)
        {
            direction = "W";
        }

        this.neighbours.Add(direction, neighbour);
    }

    public string PrefabName()
    {
        string name = "Room_";

        foreach (KeyValuePair<string, Room> neighbourPair in neighbours)
        {
            name += neighbourPair.Key;
        }

        return name;
    }

    public Room Neighbour(string direction)
    {
        return this.neighbours[direction];
    }

    public void PopulateObstacles (int numberOfObstacles, Vector2Int[] possibleSizes)
    {
        for (int obstacleIndex = 0; obstacleIndex < numberOfObstacles; obstacleIndex += 1)
        {
            int sizeIndex = Random.Range(0, possibleSizes.Length);
            Vector2Int regionSize = possibleSizes[sizeIndex];
            List<Vector2Int> region = FindFreeRegion(regionSize);
            
            foreach (Vector2Int coordinate in region)
            {
                this.population[coordinate.x, coordinate.y] = "Obstacle";
            }
        }
    }

    private bool isFree(List<Vector2Int> region)
    {
        // detect empty tile 
        foreach (Vector2Int tile in region)
        {
            if (this.population[tile.x, tile.y] != "")
            {
                return false;
            }
        }

        return true;
    }

    private List<Vector2Int> FindFreeRegion(Vector2Int sizeInTiles)
    {
        List<Vector2Int> region = new List<Vector2Int>();

        do
        {
            region.Clear();

            Vector2Int centreTile = new Vector2Int(UnityEngine.Random.Range(2, 18 - 3), UnityEngine.Random.Range(2, 10 - 3));

            region.Add(centreTile);

            int initialXCoordinate = (centreTile.x - (int)Mathf.Floor(sizeInTiles.x / 2));
            int initialYCoordinate = (centreTile.y - (int)Mathf.Floor(sizeInTiles.y / 2));

            for (int xCoordinate = initialXCoordinate; xCoordinate > initialXCoordinate + sizeInTiles.x; xCoordinate += 1)
            {
                for (int yCoordinate = initialYCoordinate; yCoordinate > initialYCoordinate + sizeInTiles.y; yCoordinate += 1)
                {
                    region.Add(new Vector2Int(xCoordinate, yCoordinate));
                }
            }
        } while (!isFree(region));

        return region;
    }

    public void AddPopulationToTilemap(Tilemap tilemap, TileBase obstacleTile)
    {
        for (int xIndex = 0; xIndex < 18; xIndex += 1)
        {
            for (int yIndex = 0; yIndex < 10; yIndex += 1)
            {
                if (this.population[xIndex, yIndex] == "Obstacle")
                {
                    tilemap.SetTile(new Vector3Int(xIndex - 9, yIndex - 5, 0), obstacleTile);
                }
                else if (this.population[xIndex, yIndex] != "" && this.population[xIndex, yIndex] != "Player")
                {
                    GameObject prefab = GameObject.Instantiate(this.name2Prefab[this.population[xIndex, yIndex]]);
                    prefab.transform.position = new Vector2(xIndex - 9 + 0.5f, yIndex - 5 + 0.5f);
                }
            }
        }
    }

    public void PopulatePrefabs(int numberOfPrefabs, GameObject[] possiblePrefabs)
    {
        for (int prefabIndex = 0; prefabIndex < numberOfPrefabs; prefabIndex += 1)
        {
            int choiceIndex = Random.Range(0, possiblePrefabs.Length);
            GameObject prefab = possiblePrefabs[choiceIndex];
            List<Vector2Int> region = FindFreeRegion(new Vector2Int(1, 1));

            this.population[region[0].x, region[0].y] = prefab.name;

            this.name2Prefab[prefab.name] = prefab;
        }
    }
}
