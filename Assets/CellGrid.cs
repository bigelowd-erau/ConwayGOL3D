using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellGrid : MonoBehaviour
{
    public int dims = 2;
    private GameObject[][][] cells;
    private float gameObjTransScalar;
    public GameObject Cell;
    public float simulationFrequency = 1.0f;
    private int cloneCount = 0;
    private float elapsedTime = 0;
    public float livingThreshold = 0.5f;
    public int minPopulation = 5;
    public int maxPopulation = 9;
    public int repopPopulation = 9;

    // Start is called before the first frame update
    void Start()
    {
        gameObjTransScalar = Cell.transform.localScale.x;
        cells = new GameObject[dims][][];
        for (int i = 0; i < dims; ++i)
        {
            cells[i] = new GameObject[dims][];
            for (int c = 0; c < dims; ++c)
                cells[i][c] = new GameObject[dims];
        }
        FillCellGrid();
    }

    private void FillCellGrid()
    {
        for (int row = 0; row < dims; ++row)
        {
            for (int col = 0; col < dims; ++col)
            {
                for (int dep = 0; dep < dims; ++dep)
                {
                    GameObject newCell = Instantiate(Cell);
                    newCell.transform.position = new Vector3(row, col, dep) * gameObjTransScalar;
                    newCell.name += cloneCount;
                    cells[row][col][dep] = newCell;
                    if (Random.value < livingThreshold)
                        newCell.SetActive(false);
                    ++cloneCount;
                }
            }
        }
    }

    void FixedUpdate()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= simulationFrequency)
        {
            Simulate();
            elapsedTime = elapsedTime % simulationFrequency;
        }
    }

    void Simulate()
    {
        //For every cell
        for (int row = 0; row < dims; ++row)
        {
            for (int col = 0; col < dims; ++col)
            {
                for (int dep = 0; dep < dims; ++dep)
                {
                    //Debug.Log("r:" + row + ", c:" + col + ", dep:" + dep);
                    int neighbors = 0;
                    //check every neighbor
                    for (int x = row > 0 ? row - 1 : row; x <= (row < dims - 1 ? row + 1 : row); ++x)
                    {
                        for (int y = col > 0 ? col - 1 : col; y <= (col < dims - 1 ? col + 1 : col); ++y)
                        {
                            for (int z = dep > 0 ? dep - 1 : dep; z <= (dep < dims - 1 ? dep + 1 : dep); ++z)
                            {
                                /*if (!(x == row && y == col && z == dep))
                                    Debug.Log(x + ", " + y + ", " + z);*/
                                if (cells[x][y][z].activeSelf/* && !(x == row && y == col && z == dep)*/)
                                {
                                    //Debug.Log("Found Neighbor");
                                    ++neighbors;
                                }
                            }
                        }
                    }

                    if (neighbors < minPopulation || neighbors > maxPopulation)
                        cells[row][col][dep].SetActive(false);
                    else if (neighbors == repopPopulation)
                        cells[row][col][dep].SetActive(true);
                    /*if (neighbors != 0)
                        Debug.Log("Neighbors:" + neighbors);*/

                }
            }
        }
    }
    
}
