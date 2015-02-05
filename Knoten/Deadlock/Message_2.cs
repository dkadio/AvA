using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deadlock
{
    [Serializable]
    public class Message : IEquatable<Message>
    {
        public int senderId;
        public String nachricht;
        public String typ;
        public int prozessId;
        public const String CONTROLL_MSG = "ctrl";
        public const String NORMAL_MSG = "msg";
        public const String CAMPAIGN_MSG = "campaign";
        public const String BUY_MSG = "buy";
        public const String EXPLORER_MSG = "explorer";
        public const String ECHO_MSG = "echo";
        public const String REQUEST_FILE = "request.file"; //request for file
        public const String GRANTED_FILE = "granted.file"; // answ for request
        public const String RELEASE_FILE = "release.file"; //releas file
        public const String RELEASED_FILE = "released.file"; //answ for realease
        public const String RENOUNCE_FILE = "renounce.file"; // auf schreibrecht verzichten
        public const String RENOUNCE_FILE_OK = "renounce.file.ok"; //bestätigung auf verzicht 
        public const String REFUSAL_FILE = "REFUSAL.file"; //absage eines rechtes


        public Message(int senderId, String nachricht, String typ)
        {
            this.senderId = senderId;
            this.nachricht = nachricht;
            this.typ = typ;
        }

        public int extractIdFromName()
        {
            String number = nachricht.Substring(nachricht.Length - 1);
            return Convert.ToInt32(number);
        }


        public Message()
        {

        }

        public bool Equals(Message other)
        {
            return this.senderId == other.senderId;
        }
    }
}
