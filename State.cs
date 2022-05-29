using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeometryFriendsAgents
{
    public class State
    {
        private float _xSpeed, _ySpeed, _xPos, _yPos;
        private int _error, _action, _size;
        private ArrayList _collectibles, _collected, _reachObstacles;
        private Obstacles.Obstacle _currentPlatform;
        private Point _point;
        private bool[] _visitedP;
        private bool _specialRight, _specialLeft;

        public bool[] VisitedP
        {
            get { return _visitedP; }
            set { _visitedP = value; }
        }

        public ArrayList ReachObstacles
        {
            get { return _reachObstacles; }
            set { _reachObstacles = value; }
        }

        public bool SpecialRight
        {
            get { return _specialRight; }
            set { _specialRight = value; }
        }

        public bool SpecialLeft
        {
            get { return _specialLeft; }
            set { _specialLeft = value; }
        }

        public Point point
        {
            get { return _point; }
            set { _point = value; }
        }

        public Obstacles.Obstacle CurrentP
        {
            get { return _currentPlatform; }
            set
            {
                this.point.Platform = value;
                _currentPlatform = value;
            }
        }

        public State (float velX, float velY, float posX, float posY, int action, ArrayList collectibles, ArrayList collectedPreviously)
        {
            this._xSpeed = velX;
            this._ySpeed = velY;
            this._xPos = posX;
            this._yPos = posY;
            this.setError(5);
            this._action = action;
            this._collectibles = collectibles;
            this._collected = collectedPreviously;
            this.point = new Point(posX, posY);
            this.SpecialLeft = false;
            this.SpecialRight = false;
            this.ReachObstacles = null;
        }

        public void setCollectibles(ArrayList col)
        {
            this._collectibles = col;
        }

        public void Collected(float posx, float posy)
        {
            this._collected.Add(posx);
            this._collected.Add(posy);
        }

        public ArrayList getAllCollected()
        {
            return (ArrayList) this._collected.Clone();
        }

        public float getCollected(int i)
        {
            return (float)this._collected[i];
        }

		public int sizeOfCaughtCollectible()
		{
			return this._collected.Count / 2;
		}

		public int getSizeOfAgent()
		{
			return _size;
		}

		public void setSizeOfAgent(int s)
		{
			this.point.Size = s;
			this._size = s;
		}

		public ArrayList getCollectibles()
		{
			ArrayList giveawayList = (ArrayList)this._collectibles.Clone();
			return giveawayList;
		}

		public int numberOfCollectibles()
		{
			return this._collectibles.Count / 2;
		}

		public void setAction(int a)
		{
			this._action = a;
		}

		public int getAction()
		{
			return this._action;
		}

		public void setError(int x)
		{
			this._error = x;
		}

		public int getError()
		{
			return this._error;
		}

		public float getVelocityX()
		{
			return this._xSpeed;
		}

		public void setVelocityX(float velocityX)
		{
			this._xSpeed = velocityX;
		}

		public float getVelocityY()
		{
			return this._ySpeed;
		}

		public float getPosX()
		{
			return this._xPos;
		}

		public void setPosX(float x)
		{
			this._xPos = x;
		}

		public float getPosY()
		{
			return this._yPos;
		}

		public void setPosY(float y)
		{
			this._yPos = y;
		}

		// equals -> checks position (with a error parameter)& velocity (for now only checks is the velocities are in the same direction
		public override bool Equals(object obj)
		{
			if (obj == null)
				return false;

			State s = obj as State;
			if ((System.Object)s == null)
				return false;

			//	if( this.isPositive(this._velocityX) == s.isPositive(s.getVelocityX()))

			if ((this.getPosX() + this.getError()) > s.getPosX() && (this.getPosX() - this.getError() < s.getPosX()))
			{
				if (this.getVelocityX() == s.getVelocityX())
				{
					if (this.getAction() == s.getAction())
					//if (this.getAction() != 2 || (this.getAction() == 2 && s.getAction() != 2) || (this.getAction() == 2 && s.getAction() != 2))
					{
						if (this.CurrentP.getID() == s.CurrentP.getID())
						{
							if (this.sizeOfCaughtCollectible() == s.sizeOfCaughtCollectible())
							{
								int a = 0;
								foreach (float i in this._collected)
								{
									if (!i.Equals(s._collected[a]))
										return false;
									a++;
								}
								return true;
							}
							else
								return false;
						}
						else
							return false;
					}
					else
						return false;
				}
				else
					return false;
			}
			else
			{
				return false;
			}
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		private bool isPositive(float x)
		{
			if (x > 0)
				return true;
			else
				return false;
		}

	}
}
