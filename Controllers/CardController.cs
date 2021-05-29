using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection.Emit;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VirtualCard.Models;

namespace VirtualCard.Controllers
{
    [Route("v1/cards")]
    [ApiController]
    public class CardController : ControllerBase
    {
        [HttpGet]
        [Route("")]
        public async Task<ActionResult<List<Card>>> Get([FromServices]DataContext context)
        {
            var cards = await context.Card.ToListAsync();
            return cards;
        }
        //[HttpGet]
        //[Route("clients")]
        //public HttpResponseMessage Get([FromServices]DataContext context, [FromQuery]string email)
        //{
        //    return this.Request.CreateResponse(
        //        HttpStatusCode.OK,
        //        new { Message = "Hello", Value = 123 });
        //}

        [HttpGet]
        [Route("clients")]
        public async Task<ActionResult<List<CardByClient>>> GetCardByEmail([FromServices]DataContext context, [FromQuery]string email)
        {
            if (ModelState.IsValid)
            {
                var cards = await context.Card
                   .Include(x => x.Client)
                   .Where(x => x.ClientEmail == email)
                   .ToListAsync();

                if (cards == null)
                {
                    return NotFound(new { message = "Nao existe cartao cadastrar para este usuario!" });
                }

                List<string> numbers = new List<string>();
                cards.ForEach(x => numbers.Add(x.Number));

                var query = from c in cards orderby c.DateOrder group c by c.ClientEmail;
                List<CardByClient> list = new List<CardByClient>();
                foreach (var group in query)
                {
                    CardByClient cc = new CardByClient();
                    cc.Client = group.Key;
                    foreach (var card in group)
                    {
                        cc.Numbers.Add(card.Number);
                    }                   
                    list.Add(cc);
                }
                return list;
            }
            else
            {
                return BadRequest(ModelState);
            }
            /*if (ModelState.IsValid)
            {
             var cards = await context.Card
                .Include(x => x.Client)
                .Where(x => x.ClientEmail == email)
                .ToListAsync();

            if (cards==null)
            {
                    return NotFound(new { message="Nao existe cartao cadastrar para este usuario!"});
            }

                List<string> numbers = new List<string>();
                cards.ForEach(x=>numbers.Add(x.Number));
                return numbers;
            }
            else
            {
                return BadRequest(ModelState);
            }*/

        }

        /*[HttpPost]
        [Route("")]
        public async Task<ActionResult<Card>> Post([FromServices]DataContext context, [FromBody]Card model)
        {
            if (ModelState.IsValid)
            {
                model.Number = CardFactory.getNewCardNumber();
                context.Card.Add(model);
                await context.SaveChangesAsync();
                return model;
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
        */
        [HttpPost]
        [Route("")]
        public async Task<ActionResult<dynamic>> Post([FromServices]DataContext context, [FromQuery]string email)
        {
            if (ModelState.IsValid)
            {
                var client = context.Client.Where(x => x.Email.Contains(email)).FirstOrDefault();

                if (client == null)
                {
                    return NotFound(new { message="Email nao existe"});
                }
                else
                {
                    Card model = new Card();                
                    model.Number = CardFactory.getNewCardNumber();
                    model.Client = client;
                    model.DateOrder = DateTime.UtcNow;
                    context.Card.Add(model);
                    await context.SaveChangesAsync();
                    return new { model.Number };
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}