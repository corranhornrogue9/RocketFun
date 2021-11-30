using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class RangeCoefficent
    {
        private List<RangeCoefficentNode> nodes;

        public RangeCoefficent()
        {
            nodes = new List<RangeCoefficentNode>();
        }

        public void AddNode(float speed, float distance, float accuracy)
        {
            if(nodes.Any(n => n.Disance == distance))
            {
                nodes.First(n => n.Disance == distance).SpeedCo.AddNode(speed, accuracy);
            }
            else
            {
                var rcn = new RangeCoefficentNode(distance, new SpeedCoefficent());
                nodes.Add(rcn);
                rcn.SpeedCo.AddNode(speed, accuracy);
                nodes = nodes.OrderBy(n => n.Disance).ToList();
            }
        }

        public float Accuracy(float distance, float speed, bool breaking)
        {
          

            float accuracy = 1;
            if (nodes.Count < 1)
            {
                accuracy = 1;
            }
            else if (nodes.Count == 1)
            {
                accuracy = nodes[0].GetSpeedAccuracy(speed, breaking);

        }
            else if (nodes[nodes.Count - 1].Disance > distance)
            {
                accuracy = nodes[nodes.Count - 1].GetSpeedAccuracy(speed, breaking);

            }
            else if (nodes[0].Disance < distance)
            {
                accuracy = nodes[0].GetSpeedAccuracy(speed, breaking);

        }
            else
            {
                for (int i = 0; i < nodes.Count; i++)
                {
                    if (nodes[i].Disance >= distance && nodes[i + 1].Disance <= distance)
                    {
                        accuracy = RangeCoefficentNode.InterpolateAccuracy(nodes[i], nodes[i + 1],distance, speed, breaking);
                    }
                }
            }

            return accuracy;
        }

    
}

