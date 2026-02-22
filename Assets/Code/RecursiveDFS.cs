using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecursiveDFS : MazeLogic
{
    public List<MapLocation> direction = new List<MapLocation>()
    {
        new MapLocation(1,0),
        new MapLocation(0,1),
        new MapLocation(-1,0),
        new MapLocation(0,-1)
    };

    public override void GenerateMaps()
    {
        Generate(5, 5);
    }

    void Generate(int x, int z)
    {
        if (CountSquareNeighbours(x, z) >= 2) return;
        map[x, z] = 0;

        // --- PERUBAHAN DI SINI ---
        // direction.Shuffle(); // <-- Ini yang bikin eror
        Shuffle(direction);     // <-- Ganti jadi begini
        // -------------------------

        Generate(x + direction[0].x, z + direction[0].z);
        Generate(x + direction[1].x, z + direction[1].z);
        Generate(x + direction[2].x, z + direction[2].z);
        Generate(x + direction[3].x, z + direction[3].z);
    }

    // --- TAMBAHKAN FUNGSI INI DI BAWAH ---
    void Shuffle<T>(List<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}