using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class SpeedCoefficent
    {
        private List<SpeedCoefficentNode> nodes;

        public SpeedCoefficent()
        {
            nodes = new List<SpeedCoefficentNode>();
        }
        public void AddNode(float speed, float accuracy)
        {
            nodes.Add(new SpeedCoefficentNode(speed, accuracy));
            nodes = nodes.OrderByDescending(n => n.Speed).ToList();
        }

    public float Accuracy(float speed, bool breaking)
        {
            if(breaking)
            {
                speed = InvertSpeed(speed);
            }

            float accuracy = 1;
            if (nodes.Count < 1)
            {
                accuracy = 1;
                
            }
            else if (nodes.Count == 1)
            {
                accuracy = nodes[0].Accuracy;

            }
            else if (nodes[nodes.Count - 1].Speed > speed)
            {
                accuracy = nodes[nodes.Count - 1].Accuracy;

            }
            else if (nodes[0].Speed < speed)
            {
                accuracy = nodes[0].Accuracy;

        }
            else
            {
                for (int i = 0; i < nodes.Count; i++)
                {
                    if (nodes[i].Speed >= speed && nodes[i + 1].Speed <= speed)
                    {
                        accuracy = SpeedCoefficentNode.InterpolateAccuracy(nodes[i], nodes[i + 1], speed);
                    Debug.Log("accruacyItorplated from speed = " + accuracy);
                    }
                }
            }

            return accuracy;
        }

    private float InvertSpeed(float speed)
    {
        var highestSpeed = nodes.First().Speed;
        var lowestSpeed = nodes.First().Speed;

        if(highestSpeed < speed)
        {
            speed = lowestSpeed;
        }
        else if(lowestSpeed > speed)
        {
            speed = highestSpeed;
        }
        else
        {
            speed = lowestSpeed + (highestSpeed - speed); 
        }

        if(speed < 0)
        {
            Debug.Log("Speed les than 0 " + speed);
        }

        return speed;
    }
}

