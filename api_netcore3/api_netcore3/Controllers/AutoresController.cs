using api_netcore3.Contexts;
using api_netcore3.Entities;
using api_netcore3.Helpers;
using api_netcore3.Models;
using AutoMapper;
using AutoMapper.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
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
    //[Authorize]
    public class AutoresController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly ILogger<AutoresController> logger;
        private readonly IMapper mapper;
        private readonly IUrlHelper urlHelper;

        public AutoresController(ApplicationDbContext context, ILogger<AutoresController> logger, IMapper mapper, IConfiguration configuration
            , IUrlHelper urlHelper)
        {
            this.context = context;
            this.logger = logger;
            this.mapper = mapper;
            this.urlHelper = urlHelper;
        }

        [HttpGet(Name = "ObtenerAutores")]
        //[HttpGet("/listado")]
        //[HttpGet("listado")]
        //[HttpGet]
        //[ServiceFilter(typeof(MiFiltroDeAccion))]
        public ActionResult<IEnumerable<Autor>> Get()
        {
            //throw new NotImplementedException();
            logger.LogInformation("Obteniendo los autores");
            return context.Autores.Include(x => x.Libros).ToList();
        }



        [HttpGet("PruebaCache")]
        [ResponseCache(Duration = 15)]
        //[Authorize]
        public ActionResult<string> GetPruebaCache()
        {
            return DateTime.Now.Second.ToString();
        }

        //[HttpGet("{id}", Name = "ObtenerAutor")]

        // opcional param2
        //[HttpGet("{id}/{param2?}")]
        //Por defecto param2=Gonzalez
        [HttpGet("{id}/{param2=Gonzalez}", Name = "ObtenerAutor")]

        // ActionResult Es recomendable
        public async Task<ActionResult<AutorDTO>> Get(int id, [BindRequired] string param2)
        {
            var autor = await context.Autores.Include(x => x.Libros).FirstOrDefaultAsync(x => x.Id == id);

            if (autor == null)
            {
                logger.LogWarning($"El autor de Id {id} no ha sido encontrado");
                return NotFound();
            }
            //return autor;
            //return new AutorDTO()
            //{
            //    Id = autor.Id,
            //    Nombre = autor.Nombre
            //};

            var autorDTO = mapper.Map<AutorDTO>(autor);

            GenerarEnlaces(autorDTO);

            return autorDTO;
        }

        private void GenerarEnlaces(AutorDTO autor)
        {
            autor.Enlaces.Add(new Enlace(urlHelper.Link("ObtenerAutor", new { id = autor.Id }), rel: "self", metodo: "GET"));
            autor.Enlaces.Add(new Enlace(urlHelper.Link("ActualizarAutor", new { id = autor.Id }), rel: "update-author", metodo: "PUT"));
            autor.Enlaces.Add(new Enlace(urlHelper.Link("BorrarAutor", new { id = autor.Id }), rel: "delete-author", metodo: "DELETE"));
        }

        [HttpPost(Name = "CrearAutor")]
        public async Task<ActionResult> Post([FromBody] AutorCreacionDTO autorCreacion)
        {
            //Esto no es necesario en asp.net core 2.1 en adelante
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}
            var autor = mapper.Map<Autor>(autorCreacion);
            context.Autores.Add(autor);
            await context.SaveChangesAsync();
            var autorDTO = mapper.Map<AutorDTO>(autor);
            return new CreatedAtRouteResult("ObtenerAutor", new { id = autor.Id }, autorDTO);
        }

        [HttpPut("{id}", Name = "ActualizarAutor")]
        public async Task<ActionResult> Put(int id, [FromBody] AutorCreacionDTO autorActualizacion)
        {
            //Esto no es necesario en asp.net core 2.1 en adelante
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            //if (id != autor.Id)
            //{
            //    return BadRequest();
            //}
            var autor = mapper.Map<Autor>(autorActualizacion);
            autor.Id = id;
            context.Entry(autor).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult> Patch(int id, [FromBody] JsonPatchDocument<AutorCreacionDTO> patchDocument)
        {
            if (patchDocument == null)
            {
                return BadRequest();
            }

            var autorDeLaDB = await context.Autores.FirstOrDefaultAsync(x => x.Id == id);

            if (autorDeLaDB == null)
            {
                return NotFound();
            }

            var autorDTO = mapper.Map<AutorCreacionDTO>(autorDeLaDB);

            patchDocument.ApplyTo(autorDTO, ModelState);

            mapper.Map(autorDTO, autorDeLaDB);

            var isValid = TryValidateModel(autorDeLaDB);

            if (!isValid)
            {
                return BadRequest(ModelState);
            }

            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}", Name = "BorrarAutor")]
        public async Task<ActionResult<Autor>> Delete(int id)
        {
            var autorId = await context.Autores.Select(x => x.Id).FirstOrDefaultAsync(x => x == id);

            if (autorId == default(int))
            {
                return NotFound();
            }

            context.Remove(new Autor { Id = autorId });
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
