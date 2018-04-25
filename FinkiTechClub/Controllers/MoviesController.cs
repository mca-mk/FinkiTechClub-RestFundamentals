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
    [Route("api/Movies")]
    public class MoviesController : Controller
    {
        private readonly AppDbContext appDbContext;
        private readonly int PAGESIZE = 2;

        public MoviesController(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        [HttpGet]
        public IEnumerable<Movie> GetMovies(int? page = 1)
        {
            return appDbContext.Movie.Skip(PAGESIZE * (page.Value - 1)).Take(PAGESIZE);

        }

        [HttpPost]
        public Movie AddMovie([FromBody]Movie movie)
        {
            appDbContext.Movie.Add(movie);
            appDbContext.SaveChanges();
            return movie;
        }

        [HttpGet]
        [Route("{movieId:int}")]
        public dynamic GetMovie([FromRoute]int movieId)
        {
            var movie = from dbMovie in appDbContext.Movie
                        where dbMovie.Id == movieId
                        select new
                        {
                            Title = dbMovie.Title,
                            Year = dbMovie.Year,
                            Actors = from dbActorInMovie in appDbContext.ActorInMovie
                                     where dbActorInMovie.Movie.Id == movieId
                                     select dbActorInMovie.Actor
                        };

            return movie;
        }
    }
}