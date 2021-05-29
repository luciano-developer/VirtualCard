using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VirtualCard.Models;

namespace VirtualCard.Controllers
{
    [Route("v1/clients")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        //private DataContext _context;
        //public ClientController(DataContext context)
        //{
        //    _context = context;
        //}

        [HttpGet]
        [Route("")]
        public async Task<ActionResult<List<Client>>> Get([FromServices]DataContext context) {
            var clients = await context.Client.ToListAsync();
            return clients;
        }

        [HttpPost]
        [Route("")]
        public async Task<ActionResult<Client>> Post([FromServices]DataContext context,[FromBody]Client model)
        {
            if (ModelState.IsValid)
            {
                context.Client.Add(model);
                await context.SaveChangesAsync();
                return model;
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}