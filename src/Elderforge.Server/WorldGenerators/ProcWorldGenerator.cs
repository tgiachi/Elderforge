using Elderforge.Core.Extensions;
using Elderforge.Core.Server.Interfaces.World;
using Elderforge.Core.Server.Noise;
using Elderforge.Core.Server.Noise.AccidentalNoise.Enums;
using Elderforge.Core.Server.Noise.AccidentalNoise.Implicit;
using Elderforge.Core.Server.Types;
using Elderforge.Server.Data.Configs;
using Elderforge.Shared.Chunks;

namespace Elderforge.Server.WorldGenerators;

public class ProcWorldGenerator : ITerrainGenerator
{
    public int Width { get; private set; }

    public int Height { get; private set; }

    public int TerrainOctaves { get; private set; }

    public float TerrainFrequency { get; private set; }


    private readonly MapData _heightMapData;
    private readonly MapData _heatMapData;
    private readonly MapData _moistureMapData;

    private readonly ImplicitCombiner _heightMap;
    private readonly ImplicitGradient _gradient;
    private readonly ImplicitFractal _fractal;


    private readonly Tile[,] _tiles;

    private readonly List<TileGroup> _waters = new();
    private readonly List<TileGroup> _lands = new();


    private readonly float ColdestValue = 0.05f;
    private readonly float ColderValue = 0.18f;
    private readonly float ColdValue = 0.4f;
    private readonly float WarmValue = 0.6f;
    private readonly float WarmerValue = 0.8f;
    private readonly float DryerValue = 0.27f;
    private readonly float DryValue = 0.4f;
    private readonly float WetValue = 0.6f;
    private readonly float WetterValue = 0.8f;
    private readonly float WettestValue = 0.9f;


    public ProcWorldGenerator(ProcWorldConfig config)
    {
        _tiles = new Tile[Width, Height];
        Width = config.Width;
        Height = config.Height;
        TerrainOctaves = config.TerrainOctaves;
        TerrainFrequency = config.TerrainFrequency;

        _heightMapData = new MapData(Width, Height);
        _heatMapData = new MapData(Width, Height);
        _moistureMapData = new MapData(Width, Height);

        _fractal = new ImplicitFractal(
            FractalType.Multi,
            BasisType.Simplex,
            InterpolationType.Quintic,
            TerrainOctaves,
            TerrainFrequency,
            1337
        );

        _gradient = new ImplicitGradient(1, 1, 1, 0, 1, 1, 1, 1, 1, 1, 1, 1);

        _heightMap = new ImplicitCombiner(CombinerType.Multiply);

        _heightMap.AddSource(_gradient);
        _heightMap.AddSource(_fractal);

        GetData(_heightMap, ref _heightMapData);

        LoadTiles();

        UpdateNeighbors();

        UpdateBitmasks();

        FloodFill();
    }


    private void UpdateNeighbors()
    {
        for (var x = 0; x < Width; x++)
        {
            for (var y = 0; y < Height; y++)
            {
                var t = _tiles[x, y];

                t.Top = GetTop(t);
                t.Bottom = GetBottom(t);
                t.Left = GetLeft(t);
                t.Right = GetRight(t);
            }
        }
    }

    private void UpdateBitmasks()
    {
        for (var x = 0; x < Width; x++)
        {
            for (var y = 0; y < Height; y++)
            {
                _tiles[x, y].UpdateBitmask();
            }
        }
    }


    private void GetData(ImplicitModuleBase module, ref MapData mapData)
    {
        mapData = new MapData(Width, Height);


        for (var x = 0; x < Width; x++)
        {
            for (var y = 0; y < Height; y++)
            {
                float x1 = 0, x2 = 1;
                float y1 = 0, y2 = 1;
                float dx = x2 - x1;
                float dy = y2 - y1;

                float s = x / (float)Width;
                float t = y / (float)Height;

                // Calculate our 4D coordinates
                float nx = x1 + MathF.Cos(s * 2 * MathF.PI) * dx / (2 * MathF.PI);
                float ny = y1 + MathF.Cos(t * 2 * MathF.PI) * dy / (2 * MathF.PI);
                float nz = x1 + MathF.Sin(s * 2 * MathF.PI) * dx / (2 * MathF.PI);
                float nw = y1 + MathF.Sin(t * 2 * MathF.PI) * dy / (2 * MathF.PI);

                float heightValue = (float)_heightMap.Get(nx, ny, nz, nw);


                if (heightValue > mapData.Max)
                {
                    mapData.Max = heightValue;
                }

                if (heightValue < mapData.Min)
                {
                    mapData.Min = heightValue;
                }

                mapData.Data[x, y] = heightValue;
            }
        }
    }

    private void FloodFill()
    {
        // Use a stack instead of recursion
        Stack<Tile> stack = new Stack<Tile>();

        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                Tile t = _tiles[x, y];

                //Tile already flood filled, skip
                if (t.FloodFilled) continue;

                // Land
                if (t.Collidable)
                {
                    TileGroup group = new TileGroup(TileGroupType.Land);
                    stack.Push(t);

                    while (stack.Count > 0)
                    {
                        FloodFill(stack.Pop(), ref group, ref stack);
                    }

                    if (group.Tiles.Count > 0)
                        _lands.Add(group);
                }
                // Water
                else
                {
                    TileGroup group = new TileGroup(TileGroupType.Water);
                    stack.Push(t);

                    while (stack.Count > 0)
                    {
                        FloodFill(stack.Pop(), ref group, ref stack);
                    }

                    if (group.Tiles.Count > 0)
                        _waters.Add(group);
                }
            }
        }
    }


    private void FloodFill(Tile tile, ref TileGroup tiles, ref Stack<Tile> stack)
    {
        // Validate
        if (tile.FloodFilled)
        {
            return;
        }

        if (tiles.Type == TileGroupType.Land && !tile.Collidable)
        {
            return;
        }

        if (tiles.Type == TileGroupType.Water && tile.Collidable)
        {
            return;
        }

        // Add to TileGroup
        tiles.Tiles.Add(tile);
        tile.FloodFilled = true;

        // floodfill into neighbors
        var t = GetTop(tile);
        if (!t.FloodFilled && tile.Collidable == t.Collidable)
        {
            stack.Push(t);
        }

        t = GetBottom(tile);
        if (!t.FloodFilled && tile.Collidable == t.Collidable)
        {
            stack.Push(t);
        }

        t = GetLeft(tile);
        if (!t.FloodFilled && tile.Collidable == t.Collidable)
        {
            stack.Push(t);
        }

        t = GetRight(tile);
        if (!t.FloodFilled && tile.Collidable == t.Collidable)
        {
            stack.Push(t);
        }
    }


    private void LoadTiles()
    {
        for (var x = 0; x < Width; x++)
        {
            for (var y = 0; y < Height; y++)
            {
                var value = _heightMapData.Data[x, y];


                value = (value - _heightMapData.Min) / (_heightMapData.Max - _heightMapData.Min);

                var t = new Tile(x, y, value);

                _tiles[x, y] = t;


                t.HeatType = GetHeatType(value);

                t.MoistureType = GetMoistureType(value);

                if (t.HeightType == TileHeightType.Grass)
                {
                    _heightMapData.Data[t.X, t.Y] -= 0.1f * t.HeightValue;
                }
                else if (t.HeightType == TileHeightType.Forest)
                {
                    _heightMapData.Data[t.X, t.Y] -= 0.2f * t.HeightValue;
                }
                else if (t.HeightType == TileHeightType.Rock)
                {
                    _heightMapData.Data[t.X, t.Y] -= 0.3f * t.HeightValue;
                }
                else if (t.HeightType == TileHeightType.Snow)
                {
                    _heightMapData.Data[t.X, t.Y] -= 0.4f * t.HeightValue;
                }
            }
        }
    }

    private Tile GetTop(Tile t)
    {
        return _tiles[t.X, MathExtension.Mod(t.Y - 1, Height)];
    }

    private Tile GetBottom(Tile t)
    {
        return _tiles[t.X, MathExtension.Mod(t.Y + 1, Height)];
    }

    private Tile GetLeft(Tile t)
    {
        return _tiles[MathExtension.Mod(t.X - 1, Width), t.Y];
    }

    private Tile GetRight(Tile t)
    {
        return _tiles[MathExtension.Mod(t.X + 1, Width), t.Y];
    }

    private HeatType GetHeatType(float value)
    {
        if (value < ColdestValue)
        {
            return HeatType.Coldest;
        }

        if (value < ColderValue)
        {
            return HeatType.Colder;
        }

        if (value < ColdValue)
        {
            return HeatType.Cold;
        }

        if (value < WarmValue)
        {
            return HeatType.Warm;
        }

        if (value < WarmerValue)
        {
            return HeatType.Warmer;
        }

        return HeatType.Warmest;
    }

    private MoistureType GetMoistureType(float value)
    {
        if (value < DryerValue)
        {
            return MoistureType.Dryest;
        }

        if (value < DryValue)
        {
            return MoistureType.Dryer;
        }

        if (value < WetValue)
        {
            return MoistureType.Dry;
        }

        if (value < WetterValue)
        {
            return MoistureType.Wet;
        }

        if (value < WettestValue)
        {
            return MoistureType.Wetter;
        }

        return MoistureType.Wettest;
    }

    public void GenerateChunk(ChunkEntity chunk, int seed)
    {
        _heightMap.Seed = seed;
    }
}
