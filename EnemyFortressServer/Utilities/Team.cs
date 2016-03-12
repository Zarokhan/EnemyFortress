using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnemyFortressServer.Utilities
{
    class Team
    {
        public int teamID;
        public int members;
        public string teamName;
        public List<Point> spawns;

        public Team()
        {
            spawns = new List<Point>();
        }
    }
}
