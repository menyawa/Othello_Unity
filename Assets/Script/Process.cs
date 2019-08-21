using System;
using UnityEngine;

public class Process : MonoBehaviour
{
    //二次元配列をコピーする関数
    public static int[,] copyArray(int[,] array) {
        int[,] copyArr = new int[GridManager.GRIDSIZE, GridManager.GRIDSIZE];
        for (int row = 0; row < GridManager.GRIDSIZE; row++) {
            for (int column = 0; column < GridManager.GRIDSIZE; column++)
                copyArr[row, column] = array[row, column];
        }

        return copyArr;
    }
}
