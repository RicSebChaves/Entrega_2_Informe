using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeometryFriendsAgents
{    
    public class Point
    {
        private float _x, _y;
        private bool _toPlatform, _gap, _turningP, _gapStart, _fall, _fallGap, _diamondAbove, _morph, _tilt;
        private int _size, distanceMomentum, distancePlatform;
        public int amountCollectibles;
        private Obstacles.Obstacle _platform;

        public Point(float x, float y)
        {
            this._x = x;
            this._y = y;
            TurningP = false;
            Gap = false;
            ToPlatform = false;
            Fall = false;
            GapStart = false;
            DiamodAbove = false;
            Tilt = false;
            Morph = false;
            FallGap = false;
            amountCollectibles = 0;

        }

        public bool DiamodAbove
        {
            get { return _diamondAbove; }
            set { _diamondAbove = value; }
        }

        public bool GapStart
        {
            get { return _gapStart; }
            set { _gapStart = value; }
        }

        public bool Fall
        {
            get { return _fall; }
            set { _fall = value; }
        }

        public bool FallGap
        {
            get { return _fallGap; }
            set { _fallGap = value; }
        }

        public float X
        {
            get { return _x; }
            set { _x = value; }
        }

        public float Y
        {
            get { return _y; }
            set { _y = value; }
        }

        public bool TurningP
        {
            get { return _turningP; }
            set { _turningP = value; }
        }

        public bool Gap
        {
            get { return _gap; }
            set { _gap = value; }
        }

        public bool ToPlatform
        {
            get { return _toPlatform; }
            set { _toPlatform = value; }
        }

        public bool Tilt
        {
            get { return _tilt; }
            set { _tilt = value; }
        }

        public bool Morph
        {
            get { return _morph; }
            set { _morph = value; }
        }

        public int Size
        {
            get { return _size; }
            set { _size = value; }
        }

        public Obstacles.Obstacle Platform
        {
            get { return _platform; }
            set { _platform = value; }
        }

        public int DistancePlatform
        {
            get { return distancePlatform; }
            set { distancePlatform = value; }
        }

        public int DistanceMomentum
        {
            get { return distanceMomentum; }
            set { distanceMomentum = value; }
        }

        public bool flagsTrue()
        {
            return ToPlatform || Gap || TurningP || GapStart || Fall || DiamodAbove || Tilt || Morph || FallGap;
        }
    }
}
