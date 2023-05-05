// Author: Merle Roji
// Based on tutorial series: https://youtu.be/BHqfkMu1Syw

using Unity.Collections;
using UnityEngine;

public class Cell : MonoBehaviour
{
    private bool m_isAlive = false;
    public bool IsAlive
    {
        get => m_isAlive;
        set
        {
            m_isAlive = value;

            if (value) // if value is true
            {
                GetComponent<SpriteRenderer>().enabled = true;
            }
            else // if value is false
            {
                GetComponent<SpriteRenderer>().enabled = false;
            }
        }
    }

    private int m_numNeighbors = 0;
    public int NumNeighbors
    {
        get => m_numNeighbors;
        set => m_numNeighbors = value;
    }
}
