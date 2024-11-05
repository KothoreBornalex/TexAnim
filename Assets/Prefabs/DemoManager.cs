using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;


public class DemoManager : MonoBehaviour
{

    [SerializeField] private GameObject _prefab;
    [SerializeField] private int _rows;
    [SerializeField] private int _columns;


    // Start is called before the first frame update
    void Start()
    {
        if (_prefab != null)
        {
            InitializeDemo(_rows, _columns);
        }
    }

    // Update is called once per frame
    void Update()
    {
    
    }

    void InitializeDemo(int Rows, int Columns)
    {
        Vector3 position = transform.position;
        for (int i = 0; i < Rows; i++)
        {
            for (int y = 0; y < Columns; y++)
            {
                Instantiate(_prefab, new Vector3(position.x + Rows, position.y, position.z + y), Quaternion.identity);
            }
        }
    }
}
