using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TexAnim.Data.Error
{
    public class TexAnimErrorData
    {
        public Color color { get; set; }

        public TexAnimErrorData()
        {
            GenerateRandomColor();
        }


        private void GenerateRandomColor()
        {
            //color = new Color32((byte)Random.Range(180, 255), (byte)Random.Range(50, 176), (byte)Random.Range(50, 176), 255);
            color = new Color32((byte)255, 0, 0, 255);

        }
    }
}
