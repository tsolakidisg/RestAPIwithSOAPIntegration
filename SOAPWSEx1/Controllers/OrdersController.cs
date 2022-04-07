using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SOAPWSEx1.Helpers;
using SOAPWSEx1.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SOAPWSEx1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        // GET: api/<OrdersController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<OrdersController>/5
        [HttpGet("{id}")]
        public async Task<string> Get(int id)
        {
            OrderService.SoapDemoSoapClient service = new OrderService.SoapDemoSoapClient(OrderService.SoapDemoSoapClient.EndpointConfiguration.SoapDemoSoap, "https://localhost:44349/SoapDemo.asmx?WSDL");

            XmlElement element = await service.GetOrderStatusAsync(id);
            XmlNodeReader nodeReader = new XmlNodeReader(element);
            DataSet dsOrder = new DataSet();
            dsOrder.ReadXml(nodeReader, XmlReadMode.Auto);

            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(dsOrder.Tables[0]);
            JSONString = JSONString[1..^1];

            return JSONString;
        }

        // POST api/<OrdersController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<OrdersController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<OrdersController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
