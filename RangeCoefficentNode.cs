using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
class RangeCoefficentNode
    {
        private SpeedCoefficent sppedCo;
        private float disance;

        public RangeCoefficentNode(float distance, SpeedCoefficent speedCo)
        {
            this.sppedCo = speedCo;
            this.disance = distance;
        }

        public float GetSpeedAccuracy(float speed, bool breaking)
        {
            return sppedCo.Accuracy(speed, breaking);
        }

        public float Disance
        {
            get
            {
                return disance;
            }
        }

        public SpeedCoefficent SpeedCo
        {
            get
            {
                return sppedCo;
            }
        }

        public static float InterpolateAccuracy(RangeCoefficentNode highrange, RangeCoefficentNode lowrange, float range, float speed, bool breaking)
        {
            if (lowrange.GetSpeedAccuracy(speed, breaking) < highrange.GetSpeedAccuracy(speed, breaking))
            {
                return Interpolate(highrange, lowrange, speed, range, breaking);
            }
            else
            {
                return Interpolate(lowrange, highrange, speed, range, breaking);
            }
        }

        private static float Interpolate(RangeCoefficentNode lowRange, RangeCoefficentNode lowSpeed, float speed, float distance, bool breaking)
        {
            return lowSpeed.GetSpeedAccuracy(speed, breaking) + ((lowRange.GetSpeedAccuracy(speed, breaking) - lowSpeed.GetSpeedAccuracy(speed, breaking)) / ((lowRange.Disance - lowSpeed.Disance) / distance));
        }

    }

