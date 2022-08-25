using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Viki
{
    internal class Performance
    {

        public class Rootobject
        {
            public Message message { get; set; }
            public string webview { get; set; }
        }

        public class Message
        {
            public string method { get; set; }
            public Params _params { get; set; }
        }

        public class Params
        {
            public string documentURL { get; set; }
            public string frameId { get; set; }
            public bool hasUserGesture { get; set; }
            public Initiator initiator { get; set; }
            public string loaderId { get; set; }
            public bool redirectHasExtraInfo { get; set; }
            public Request request { get; set; }
            public string requestId { get; set; }
            public float timestamp { get; set; }
            public string type { get; set; }
            public float wallTime { get; set; }
        }

        public class Initiator
        {
            public Stack stack { get; set; }
            public string type { get; set; }
        }

        public class Stack
        {
            public Callframe[] callFrames { get; set; }
        }

        public class Callframe
        {
            public int columnNumber { get; set; }
            public string functionName { get; set; }
            public int lineNumber { get; set; }
            public string scriptId { get; set; }
            public string url { get; set; }
        }

        public class Request
        {
            public bool hasPostData { get; set; }
            public Headers headers { get; set; }
            public string initialPriority { get; set; }
            public bool isSameSite { get; set; }
            public string method { get; set; }
            public string mixedContentType { get; set; }
            public string postData { get; set; }
            public Postdataentry[] postDataEntries { get; set; }
            public string referrerPolicy { get; set; }
            public string url { get; set; }
        }

        public class Headers
        {
            public string Referer { get; set; }
            public string UserAgent { get; set; }
            public string secchua { get; set; }
            public string secchuamobile { get; set; }
            public string secchuaplatform { get; set; }
        }

        public class Postdataentry
        {
            public string bytes { get; set; }
        }
    }
}
