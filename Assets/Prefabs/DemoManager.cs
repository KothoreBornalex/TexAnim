using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;


public class DemoManager : MonoBehaviour
{

    [SerializeField] private GameObject _prefab;

    private int _totalCount;

    [SerializeField, Range(0, 250)] private int _rows;
    [SerializeField, Range(0, 250)] private int _columns;

    [SerializeField, Range(0, 10)] private int _rowsSpace;
    [SerializeField, Range(0, 10)] private int _columnsSpace;


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
        _totalCount = Rows * Columns;

        Vector3 position = transform.position;
        for (int i = 0; i < Rows; i++)
        {
            for (int y = 0; y < Columns; y++)
            {
                Instantiate(_prefab, new Vector3(position.x + -i * _rowsSpace, position.y, position.z + -y * _columnsSpace), Quaternion.identity);
            }
        }
    }
}
