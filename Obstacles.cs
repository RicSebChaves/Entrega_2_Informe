using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeometryFriendsAgents
{
    public class Obstacles
    {
        private List<Obstacle> _allObstacles;

        public class Obstacle
        {
            private float _x, _y, _height, _width;
            private int _id;

            public Obstacle(float x, float y, float height, float width, int id)
            {
                this._x = x;
                this._y = y;
                this._height = height;
                this._width = width;
                this._id = id;
            }

            public int getID()
            {
                return this._id;
            }

            public float maxHeight()
            {
                return this._y - this._height / 2;
            }

            public float minHeight()
            {
                return this._y + this._height / 2;
            }

            public float maxWidth()
            {
                return this._x + this._width / 2;
            }

            public float minWidth()
            {
                return this._x - this._width / 2;
            }

            public void setX(float x)
            {
                this._x = x;
            }

            public void setWidth(float width)
            {
                this._width = width;
            }

            public void setY(float y)
            {
                this._y = y;
            }

            public void setHeight(float height)
            {
                this._height = height;
            }
        }

        public Obstacles(int numbersInfo, float[] oI, int remainingInfo, float[] rI)
        {
            this._allObstacles = new List<Obstacle>();

            int temp = 1;
            if(numbersInfo > 0)
            {
                while(temp <= numbersInfo)
                {
                    this._allObstacles.Add(new Obstacle(oI[(temp * 4) - 4], oI[(temp * 4) - 3], oI[(temp * 4) - 2], oI[(temp * 4) - 1], temp));
                    temp++;
                }
            }
            else
            {
                this._allObstacles.Add(new Obstacle(oI[0], oI[1], oI[2], oI[3], temp));
            }

            int rtemp = 1;

            if (remainingInfo > 0)
            {
                while (rtemp <= remainingInfo)
                {
                    this._allObstacles.Add(new Obstacle(rI[(rtemp * 4) - 4], rI[(rtemp * 4) - 3], rI[(rtemp * 4) - 2], rI[(rtemp * 4) - 1], temp + rtemp));
                    rtemp++;
                }
            }
            else
            {
                if (!(rI[0] == 0 && rI[1] == 0 && rI[2] == 0 && rI[3] == 0))
                    this._allObstacles.Add(new Obstacle(rI[0], rI[1], rI[2], rI[3], temp + rtemp));
            }
            removeUnwantedObstacles();
        }

        public void removeUnwantedObstacles()
        {
            foreach (Obstacle obs in _allObstacles)
            {
                foreach (Obstacle o in _allObstacles)
                {
                    if (obs != o)
                    {
                        if (o.maxHeight() == obs.maxHeight() && o.minHeight() == obs.minHeight())
                        {
                            if (0 <= (o.minWidth() - obs.maxWidth()) && (o.minWidth() - obs.maxWidth()) <= 48)
                            {
                                float leftPoint = obs.minWidth();
                                float rightPoint = o.maxWidth();

                                obs.setWidth(rightPoint - leftPoint);
                                obs.setX(leftPoint + (rightPoint - leftPoint) / 2);
                                _allObstacles.Remove(o);
                                removeUnwantedObstacles();
                                return;
                            }
                        }
                    }
                }
            }
            foreach (Obstacle obs in _allObstacles)
            {
                foreach (Obstacle o in _allObstacles)
                {
                    if (obs != o)
                    {
                        if (o.maxWidth() == obs.maxWidth() && o.minWidth() == obs.minWidth())
                        {
                            if (0 <= (obs.maxHeight() - o.minHeight()) && (obs.maxHeight() - o.minHeight()) <= 48)
                            {
                                float botPoint = obs.minHeight();
                                float topPoint = o.maxHeight();

                                obs.setHeight(botPoint - topPoint);
                                obs.setY(botPoint - (botPoint - topPoint) / 2);
                                _allObstacles.Remove(o);
                                removeUnwantedObstacles();
                                return;
                            }
                        }
                    }
                }
            }
        }

        public Obstacle getNextPlatform(State s)
        {
            float closestY = 760;
            Obstacle nextObs = new Obstacle(600, 780, 40, 1200, 0);
            foreach (Obstacle obs in _allObstacles)
            {
                if (s.getPosY() < obs.maxHeight())
                {
                    if (obs.maxHeight() < closestY)
                        if (obs.maxWidth() > s.getPosX() && obs.minWidth() <= s.getPosX())
                        {
                            nextObs = obs;
                            closestY = obs.maxHeight();
                        }
                }
            }
            return nextObs;
        }

        public Obstacle getNextPlatformRange(State s, int range)
        {
            float closestY = 760;
            Obstacle nextObs = new Obstacle(600, 780, 40, 1200, 0);
            float begin, end;

            if (s.getVelocityX() == 0)
            {
                begin = s.getPosX() - range;
                if (begin < 0)
                    begin = 0;
                end = s.getPosX();
            }
            else
            {
                begin = s.getPosX();
                end = s.getPosX() + range;
                if (end > 1240)
                    end = 1240;
            }

            foreach (Obstacle obs in _allObstacles)
            {
                if (s.getPosY() + s.getSizeOfAgent() / 2 + 24 < obs.maxHeight())
                {
                    if (obs.maxHeight() < closestY)
                        if (obs.maxWidth() > begin && obs.minWidth() < end)
                        {
                            if (jumpableto(obs, begin, end, s.getVelocityX(), s.getSizeOfAgent()))
                            {
                                nextObs = obs;
                                closestY = obs.maxHeight();
                            }
                        }
                }
            }
            return nextObs;
        }

        public bool jumpableto(Obstacle obs, float begin, float end, float v, int height)
        {
            float newbegin, newend;
            int halfwidth = getWidth(height) / 2;
            if (v == 0)
            {
                newend = Math.Min(end + halfwidth, obs.maxWidth());
                newbegin = newend - 192;
            }
            else
            {
                newbegin = Math.Max(begin - halfwidth, obs.minWidth());
                newend = newbegin + 192;
            }

            bool jumpable = true;

            foreach (Obstacle o in _allObstacles)
            {
                if (o.maxWidth() > newbegin && o.minWidth() < newend && o.minHeight() <= obs.maxHeight())
                {
                    if (o.minHeight() > obs.maxHeight() - 100)
                        jumpable = false;
                }
            }

            return jumpable;
        }

        private int getWidth(int size)
        {
            return (96 * 96) / size;
        }

        // This function is meant to judge if the next platform (so it is only to be called near the end of the current platform) is at the same lvl and therefore exists a gap between them
        public float[] isThereAGap(State s, Obstacle o, float min, float max)
        {
            foreach (Obstacle obs in _allObstacles)
            {
                if (obs != o)
                {
                    if (obs.maxHeight() == o.maxHeight())
                    {
                        if ((obs.maxWidth() < o.minWidth() && (o.minWidth() - obs.maxWidth() > min && o.minWidth() - obs.maxWidth() < max)) && (s.getPosX() <= o.minWidth() && s.getPosX() > obs.maxWidth()))       // so that the gap is possible
                        {
                            float[] distance = new float[4];
                            distance[0] = s.getPosX() - obs.maxWidth();     // distance to next platform
                            distance[1] = obs.maxWidth() - obs.minWidth();  // platform width - use to measure speed and such
                            distance[2] = s.getPosX() - o.minWidth();		// actual maximum point by which we must jump
                            distance[3] = o.minWidth() - obs.maxWidth();
                            return distance;
                        }
                        else if ((obs.minWidth() > o.maxWidth() && (obs.minWidth() - o.maxWidth() > min && obs.minWidth() - o.maxWidth() < max)) && (s.getPosX() >= o.maxWidth() && s.getPosX() < obs.minWidth()))
                        {
                            float[] distance = new float[4];
                            distance[0] = obs.minWidth() - s.getPosX();
                            distance[1] = obs.maxWidth() - obs.minWidth();
                            distance[2] = o.maxWidth() - s.getPosX();
                            distance[3] = obs.minWidth() - o.maxWidth();
                            return distance;
                        }
                    }
                }
            }
            return null;
        }

        public ArrayList platformsAbove(State initS, State finalS)
        {
            float direction = initS.getVelocityX();
            float left = 0;
            float right = 0;
            float top = finalS.getPosY();
            float bot = initS.getPosY();

            if (direction == 0) // going left
            {
                left = finalS.getPosX();
                right = initS.getPosX();
            }
            else if (direction == 1) // going right
            {
                left = initS.getPosX();
                right = finalS.getPosX();
            }

            ArrayList l = new ArrayList();

            foreach (Obstacle obs in _allObstacles)
            {
                if (direction == 0)
                {
                    if (((left <= obs.maxWidth()) && (obs.maxWidth() <= right)) && ((bot >= obs.maxHeight()) && (obs.maxHeight() >= top)))
                    {
                        l.Add(obs);
                    }
                }
                else if (direction == 1)
                {
                    if ((left <= obs.minWidth() && obs.minWidth() <= right) && (bot >= obs.maxHeight() && obs.maxHeight() >= top))
                    {
                        l.Add(obs);
                    }
                }
            }

            ArrayList toBeRemoved = new ArrayList();

            foreach (Obstacle obs in l)
            {
                // check to see if obstacles are too close for the circle to fit
                foreach (Obstacle o in l)
                {
                    if (obs != o)
                    {
                        if (obs.maxHeight() > o.maxHeight())
                        {
                            if (obs.maxHeight() - o.minHeight() < finalS.getSizeOfAgent())
                            {
                                //l.Remove(obs);
                                toBeRemoved.Add(obs);
                                break;
                            }
                        }
                    }
                }
            }

            foreach (Obstacle obs in toBeRemoved)
            {
                l.Remove(obs);
            }

            return l;
        }

        public float rightEdge(Obstacle o)
        {
            return o.maxWidth();
        }
        public float leftEdge(Obstacle o)
        {
            return o.minWidth();
        }
        public float topEdge(Obstacle o)
        {
            return o.maxHeight();
        }
        public float bottomEdge(Obstacle o)
        {
            return o.minHeight();
        }

    }
}
