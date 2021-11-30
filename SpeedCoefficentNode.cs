using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class SpeedCoefficentNode
    {
        public SpeedCoefficentNode(float speed, float accruacy)
        {
            this.speed = speed;
            this.accuracy = accruacy; 
        }

        public float Speed
        {
            get
            {
                return speed;
            }
        }

        public float Accuracy
        {
            get
            {
                return accuracy;
            }
        }

        public static float InterpolateAccuracy(SpeedCoefficentNode highSpeed, SpeedCoefficentNode lowSpeed, float speed)
        {
            if (lowSpeed.accuracy < highSpeed.accuracy)
            {
                Debug.Log("lowSpeed accuracy less than high speed");
                return Interpolate(highSpeed, lowSpeed, speed);
           
            }
            else
            {
            Debug.Log("highSpeed accuracy less than low speed");
            return Interpolate(lowSpeed, highSpeed, speed);
            }
        }

        private static float Interpolate(SpeedCoefficentNode highSpeed, SpeedCoefficentNode lowSpeed, float speed)
        {
        Debug.Log("high speed = " + highSpeed.speed + " low speed = " + lowSpeed.speed + " speed = " + speed + " high s sccruacy = " + highSpeed.accuracy + " low speed accrcyt = " + lowSpeed.accuracy);
            return lowSpeed.accuracy + ((highSpeed.accuracy - lowSpeed.accuracy) / ((Math.Abs(highSpeed.speed - lowSpeed.speed)) / speed));
        }

        float speed;
        float accuracy;
    }

