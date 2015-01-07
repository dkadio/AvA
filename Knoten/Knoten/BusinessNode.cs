using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Knoten
{
    class BusinessNode : Knoten
    {
        String nodeTypeId;

        public BusinessNode(int id, String ip, int port, String nodeTypeId) : base(id, ip, port)
        {
            this.nodeTypeId = nodeTypeId;
        }

        public BusinessNode(int id, String nodeTypeId)
            : base(id)
        {
            this.nodeTypeId = nodeTypeId;
        }

        public override void printid()
        {
            Console.WriteLine("BusuinessKnioten");
        }
    }
}
