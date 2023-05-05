// Author: Merle Roji
// Based on tutorial series: https://youtu.be/BHqfkMu1Syw

using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private const int SCREEN_WIDTH = 64;    // 1024 pixels
    private const int SCREEN_HEIGHT = 48;   // 768 pixels

    private Cell[,] m_grid = new Cell[SCREEN_WIDTH, SCREEN_HEIGHT]; // create grid
    private bool m_populationCanLoop;
    private Camera m_mainCam;
    private bool m_simulationEnabled = false;

    [SerializeField] private Cell cellPrefab;
    [SerializeField] private float timer;
    [SerializeField] private bool startSimulated = false;

    #region UNITY METHODS

    private void Start()
    {
        Init();
        DrawCellGrid();
    }

    private void Update()
    {
        CheckInput();

        if (!m_simulationEnabled) return; // stop if no simulation enabled

        if (m_populationCanLoop)
        {
            StopCoroutine(PopulationControlLoop());
            StartCoroutine(PopulationControlLoop());
        }
    }

    #endregion

    #region GAME OF LIFE METHODS

    private void Init()
    {
        m_populationCanLoop = true;
        m_simulationEnabled = false;
        m_mainCam = Camera.main;
    }

    private void CheckInput()
    {
        if (Input.GetMouseButtonDown(0)) // turn cell on or off
        {
            Vector2 mousePoint = m_mainCam.ScreenToWorldPoint(Input.mousePosition);
            int mouseX = Mathf.RoundToInt(mousePoint.x);
            int mouseY = Mathf.RoundToInt(mousePoint.y);

            // checking if in bounds
            if (mouseX >= 0
                && mouseX < SCREEN_WIDTH
                && mouseY >= 0
                && mouseY < SCREEN_HEIGHT)
            {
                m_grid[mouseX, mouseY].IsAlive = !m_grid[mouseX, mouseY].IsAlive;
            }
        }

        if (Input.GetKeyUp(KeyCode.P)) // pause
        {
            m_simulationEnabled = !m_simulationEnabled;
        }
    }

    private void DrawCellGrid()
    {
        // iterate over entire screen
        for (int x = 0; x < SCREEN_WIDTH; x++)
        {
            for (int y = 0; y < SCREEN_HEIGHT; y++)
            {
                Cell cell = Instantiate(cellPrefab, new Vector2(x, y), Quaternion.identity);
                m_grid[x, y] = cell;
                if (startSimulated) m_grid[x, y].IsAlive = RandomAliveCell();
                else m_grid[x, y].IsAlive = false;
            }
        }
    }

    private IEnumerator PopulationControlLoop()
    {
        m_populationCanLoop = false;

        CountNeighbors();
        PopulationControl();

        yield return new WaitForSeconds(timer);

        m_populationCanLoop = true;
    }

    private void CountNeighbors()
    {
        // iterate through grid
        for (int x = 0; x < SCREEN_WIDTH; x++)
        {
            for (int y = 0; y < SCREEN_HEIGHT; y++)
            {
                int numNeighbors = 0;

                // North
                if (y + 1 < SCREEN_HEIGHT)
                {
                    if (m_grid[x, y + 1].IsAlive)
                    {
                        numNeighbors++;
                    }
                }

                // North East
                if (x + 1 < SCREEN_WIDTH 
                    && y + 1 < SCREEN_HEIGHT)
                {
                    if (m_grid[x + 1, y + 1].IsAlive)
                    {
                        numNeighbors++;
                    }
                }

                // East
                if (x + 1 < SCREEN_WIDTH)
                {
                    if (m_grid[x + 1, y].IsAlive)
                    {
                        numNeighbors++;
                    }
                }

                // South East
                if (x + 1 < SCREEN_WIDTH
                    && y - 1 >= 0)
                {
                    if (m_grid[x + 1, y - 1].IsAlive)
                    {
                        numNeighbors++;
                    }
                }

                // South
                if (y - 1 >= 0)
                {
                    if (m_grid[x, y - 1].IsAlive)
                    {
                        numNeighbors++;
                    }
                }

                // South West
                if (x - 1 >= 0
                    && y -1 >= 0)
                {
                    if (m_grid[x - 1, y - 1].IsAlive)
                    {
                        numNeighbors++;
                    }
                }

                // West
                if (x - 1 >= 0)
                {
                    if (m_grid[x - 1, y].IsAlive)
                    {
                        numNeighbors++;
                    }
                }

                // North West
                if (x - 1 >= 0
                    && y + 1 < SCREEN_HEIGHT)
                {
                    if (m_grid[x - 1, y + 1].IsAlive)
                    {
                        numNeighbors++;
                    }
                }

                m_grid[x, y].NumNeighbors = numNeighbors; // tells how many neighbors each of our cells has

            }
        }
    }

    private void PopulationControl()
    {
        // iterate through grid
        for (int x = 0; x < SCREEN_WIDTH; x++)
        {
            for (int y = 0; y < SCREEN_HEIGHT; y++)
            {
                /* === RULES ===
                 * 1. Any live cell with 2 or 3 neighbors survives
                 * 2. Any dead cell with 3 live neighbors becomes a live cell
                 * 3. All other live cells die in the next generation and all other dead cells stay dead
                 */

                if (m_grid[x, y].IsAlive)
                {
                    if (m_grid[x, y].NumNeighbors != 2 
                        && m_grid[x, y].NumNeighbors != 3)
                    {
                        m_grid[x, y].IsAlive = false;
                    }
                }
                else // cell is dead
                {
                    if (m_grid[x, y].NumNeighbors == 3)
                    {
                        m_grid[x, y].IsAlive = true;
                    }
                }
            }
        }
    }

    private bool RandomAliveCell()
    {
        int rand = Random.Range(0, 100);

        if (rand > 75)
        {
            return true;
        }

        return false;
    }

    #endregion
}
