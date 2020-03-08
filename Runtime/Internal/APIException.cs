/******************************************/
/*                                        */
/*     Copyright (c) 2018 monitor1394     */
/*     https://github.com/monitor1394     */
/*                                        */
/******************************************/


namespace XCharts
{
    public class APIException : System.Exception
    {
        public APIException() { }
        public APIException(string message) : base(message) { }
        public APIException(string message, System.Exception inner) : base(message, inner) { }
        protected APIException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}