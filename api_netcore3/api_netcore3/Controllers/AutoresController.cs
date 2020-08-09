using api_netcore3.Contexts;
using api_netcore3.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api_netcore3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AutoresController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly ILogger<AutoresController> logger;

        public AutoresController(ApplicationDbContext context,ILogger<AutoresController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        [HttpGet("/listado")]
        [HttpGet("listado")]
        public ActionResult<IEnumerable<Autor>> Get()
        {
            logger.LogInformation("Obteniendo los autores");
            return context.Autores.Include(x => x.Libros).ToList();
        }

        [HttpGet("PruebaCache")]
        [ResponseCache(Duration =15)]
        //[Authorize]
        public ActionResult<string> GetPruebaCache()
        {
            return DateTime.Now.Second.ToString();
        }

        //[HttpGet("{id}", Name = "ObtenerAutor")]

        // opcional param2
        //[HttpGet("{id}/{param2?}")]
        //Por defecto param2=Gonzalez
        [HttpGet("{id}/{param2=Gonzalez}")]

        // ActionResult Es recomendable
        public async Task<ActionResult<Autor>> Get(int id, [BindRequired] string param2)
        {
            var autor = await context.Autores.Include(x => x.Libros).FirstOrDefaultAsync(x => x.Id == id);

            if (autor == null)
            {
                logger.LogWarning($"El autor de Id {id} no ha sido encontrado");
                return NotFound();
            }
            return autor;
        }

        [HttpPost]
        public ActionResult Post([FromBody] Autor autor)
        {
            //Esto no es necesario en asp.net core 2.1 en adelante
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            context.Autores.Add(autor);
            context.SaveChanges();
            return new CreatedAtRouteResult("ObtenerAutor", new { id = autor.Id }, autor);
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Autor value)
        {
            //Esto no es necesario en asp.net core 2.1 en adelante
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            if (id != value.Id)
            {
                return BadRequest();
            }

            context.Entry(value).State = EntityState.Modified;
            context.SaveChanges();
            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult<Autor> Delete(int id)
        {
            var autor = context.Autores.FirstOrDefault(x => x.Id == id);

            if (autor == null)
            {
                return NotFound();
            }

            context.Autores.Remove(autor);
            context.SaveChanges();
            return autor;
        }
    }
}
