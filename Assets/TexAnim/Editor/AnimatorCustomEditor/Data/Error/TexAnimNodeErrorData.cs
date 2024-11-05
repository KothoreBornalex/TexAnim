using System.Collections;
using System.Collections.Generic;
using TexAnim.Elements;
using UnityEngine;


namespace TexAnim.Data.Error
{
    using Elements;
    public class TexAnimNodeErrorData
    {
        public TexAnimErrorData ErrorData { get; set; }
        public List<TexAnimNode> Nodes { get; set; }

        public TexAnimNodeErrorData()
        {
            ErrorData = new TexAnimErrorData();
            Nodes = new List<TexAnimNode>();
        }

    }
}
