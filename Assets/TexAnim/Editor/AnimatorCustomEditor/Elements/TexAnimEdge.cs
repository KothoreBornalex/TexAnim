using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace TexAnim
{
    public class TexAnimEdge : Edge
    {
        
        public string targetNode;
        public string parentNode;
        public List<TransitionCondition> conditions;
        public TexAnimEdge() : base()
        {
            this.name = "TexAnim Edge";
            // Any additional initialization code for TexAnimEdge
        }

    }
}
