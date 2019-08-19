using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public const int GRIDSIZE = 8;
    public int[,] grids = new int[GRIDSIZE, GRIDSIZE];

    // Start is called before the first frame update
    void Start()
    {
        initGrid();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void initGrid() {
        for (int row = 0; row < GRIDSIZE; row++) {
            for (int column = 0; column < GRIDSIZE; column++)
                grids[row, column] = 0;
        }
        //-1しているのはインデックスが0から始まるため
        grids[GRIDSIZE / 2 - 1, GRIDSIZE / 2 - 1] = grids[GRIDSIZE / 2, GRIDSIZE / 2] = 2;
        grids[GRIDSIZE / 2 - 1, GRIDSIZE / 2] = grids[GRIDSIZE / 2, GRIDSIZE / 2 - 1] = 1;
    }
}
