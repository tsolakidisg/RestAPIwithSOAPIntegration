using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NLog;
using RestAPIwithSOAPIntegration.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RestAPIwithSOAPIntegration.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {      
        // GET api/<OrdersController>/5
        [HttpGet("{id}")]
        public async Task<string> Get(int id)
        {
            var guid = Guid.NewGuid();
            var coreApiUuid = Guid.NewGuid();
            var soapUuid = Guid.NewGuid();
            // Create a JSON string to transform the XML object into JSON
            string JSONString = string.Empty;

            LoggerHelper.HeadTableLogging(guid, "New Order", "New");
            LoggerHelper.DetailsTableLogging(guid, coreApiUuid, "New Order", "Orders/id", "Core API", "Initial", "{'Id':" + id + "}", new object() { });
            // Create service object to connect in the SOAP WS using WSDL
            LoggerHelper.DetailsTableLogging(guid, soapUuid, "New Order", "SoapDemo.asmx?op=GetOrderStatus", "SOAP_WS", "Sent", "{'Id':" + id + "}", new object() { });
            try
            {
                OrderService.SoapDemoSoapClient service = new OrderService.SoapDemoSoapClient(OrderService.SoapDemoSoapClient.EndpointConfiguration.SoapDemoSoap, "https://localhost/SoapDemo/SoapDemo.asmx?WSDL");
                //Create XML element to receive the response of the SOAP WS
                XmlElement element = await service.GetOrderStatusAsync(id);
                XmlNodeReader nodeReader = new XmlNodeReader(element);
                // Create a DataSet Object to store the result XML
                DataSet dsOrder = new DataSet();
                dsOrder.ReadXml(nodeReader, XmlReadMode.Auto);
                
                JSONString = JsonConvert.SerializeObject(dsOrder.Tables[0]);
                JSONString = JSONString[1..^1]; // Remove the table notation => [ ] (first and last character of the string)

                LoggerHelper.DetailsTableLogging(guid, soapUuid, "New Order", "SoapDemo.asmx?op=GetOrderStatus", "Core API", "Received", JSONString, new object() { });
                LoggerHelper.DetailsTableLogging(guid, coreApiUuid, "New Order", "Orders/id", "SAP", "Completed", JSONString, new object() { });
            }
            catch (Exception ex)
            {
                LoggerHelper.DetailsTableLogging(guid, soapUuid, "New Order", "SoapDemo.asmx?op=GetOrderStatus", "Core API", "Error", new object() { }, ex);
            }            

            return JSONString;
        }

        // PUT api/<OrdersController>/5
        [HttpPut("{id}")]
        public async Task<string> Put(int id, [FromBody] Order order)
        {
            // Create service object to connect in the SOAP WS using WSDL
            OrderService.SoapDemoSoapClient service = new OrderService.SoapDemoSoapClient(OrderService.SoapDemoSoapClient.EndpointConfiguration.SoapDemoSoap, "https://localhost:44349/SoapDemo.asmx?WSDL");

            // This method returns a result of type string
            string returnMessage = await service.UpdateDBRecordsAsync(id, order.CustomerName);

            // Create a new object of type Message to construct the final JSON string
            Message message = new Message();
            message.Response = returnMessage;
            string jsonReturnMessage = JsonConvert.SerializeObject(message);

            return jsonReturnMessage;
        }
    }
}
