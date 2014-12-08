using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Knoten
{
    [Serializable]
    public class Message 
    {
        public int senderId;
        public String nachricht;
        public String typ;
        public Produkt product;
        public  const String CONTROLL_MSG = "ctrl";
        public  const String NORMAL_MSG = "msg";
        public  const String CAMPAIGN_MSG = "campaign";


        public Message(int senderId, String nachricht, String typ)
        {
            this.senderId = senderId;
            this.nachricht = nachricht;
            this.typ = typ;
        }

        public Message(int senderId, String nachricht, String typ, Produkt product)
        {
            this.senderId = senderId;
            this.nachricht = nachricht;
            this.typ = typ;
            this.product = product;
        }

        public Message()
        {
            
        }
    }
}
