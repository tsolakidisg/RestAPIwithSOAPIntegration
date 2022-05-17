using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestAPIwithSOAPIntegration.Controllers
{
    public static class LoggerHelper
    {
        public static void HeadTableLogging(Guid guid, string order_type, string order_status)
        {
            var loggerH = LogManager.GetLogger("Head_Table");

            LogEventInfo headLogEvent = new LogEventInfo(LogLevel.Info, "Head_Table","");
            headLogEvent.Properties.Add("GUID", guid);
            headLogEvent.Properties.Add("OrderType", order_type);
            headLogEvent.Properties.Add("OrderStatus", order_status);

            loggerH.Info(headLogEvent);
        }

        public static void DetailsTableLogging(Guid guid, Guid uuid, string order_type, string service, string end_system, string state, object payload, object ex)
        {
            var loggerD = LogManager.GetLogger("Details_Table");

            if (state == "Error")
            {
                LogEventInfo detailsLogEvent = new LogEventInfo(LogLevel.Error, "Details_Table", null, "", new List<object>().ToArray(), (Exception)ex);
                detailsLogEvent.Properties.Add("GUID", guid);
                detailsLogEvent.Properties.Add("OrderType", order_type);
                detailsLogEvent.Properties.Add("UUID", uuid);
                detailsLogEvent.Properties.Add("Service", service);
                detailsLogEvent.Properties.Add("EndSystem", end_system);
                detailsLogEvent.Properties.Add("State", state);
                detailsLogEvent.Properties.Add("Payload", "");
                loggerD.Log(detailsLogEvent);
            }
            else 
            {
                LogEventInfo detailsLogEvent = new LogEventInfo(LogLevel.Info, "Details_Table", "");
                detailsLogEvent.Properties.Add("GUID", guid);
                detailsLogEvent.Properties.Add("OrderType", order_type);
                detailsLogEvent.Properties.Add("UUID", uuid);
                detailsLogEvent.Properties.Add("Service", service);
                detailsLogEvent.Properties.Add("EndSystem", end_system);
                detailsLogEvent.Properties.Add("State", state);
                detailsLogEvent.Properties.Add("Payload", payload);
                loggerD.Info(detailsLogEvent);
            }                   
        }
    }
}
