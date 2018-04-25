using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinkiTechClub.Db;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FinkiTechClub.Controllers
{
    [Produces("application/json")]
    [Route("api/Actors")]
    public class ActorsController : Controller
    {
        private readonly AppDbContext appDbContext;

        public ActorsController(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        [HttpGet]
        public List<Actor> GetActors()
        {
            return appDbContext.Actor.ToList();
        }

        [HttpPost]
        public Actor AddActor([FromBody]Actor actor)
        {
            appDbContext.Actor.Add(actor);
            appDbContext.SaveChanges();
            return actor;
        }

        [HttpGet]
        [Route("{actorId:int}")]
        public Actor GetActor(int actorId)
        {
            return appDbContext.Actor
                .FirstOrDefault(c => c.Id == actorId);
        }

        [HttpDelete]
        [Route("{actorId:int}")]
        public ActionResult DeleteActor(int actorId)
        {
            var actorToDelete = appDbContext.Actor.FirstOrDefault(c => c.Id == actorId);

            if (actorToDelete == null)
            {
                return NotFound();
            }
            else
            {
                appDbContext.Actor.Remove(actorToDelete);
                appDbContext.SaveChanges();
                return Ok();
            }
        }
    }
}