using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBlockGridGenerator : MonoBehaviour
{
    [SerializeField] private GameObject blockPrefab; 
    [SerializeField] private int gridWidth = 10;     
    [SerializeField] private int gridHeight = 10;    
    [SerializeField] private float blockSize = 1f;   
    
    void Start()
    {
        GenerateGrid(); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void GenerateGrid()
    {
        // Controllo che il prefab del blocco sia assegnato
        if (blockPrefab == null)
        {
            Debug.LogError("Il prefab del blocco non è assegnato!");
            return;
        }

        // Loop per generare una griglia di blocchi
        for (int x = 0; x < gridWidth; x++)
        {
            for (int z = 0; z < gridHeight; z++)
            {
                // Calcola la posizione del blocco in base alle sue coordinate e alla dimensione
                Vector3 blockPosition = new Vector3(x * blockSize, 0, z * blockSize);

                // Instanzia un nuovo blocco nella posizione calcolata
                Instantiate(blockPrefab, blockPosition, Quaternion.identity, transform);
            }
        }
    }
}
