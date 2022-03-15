using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace SuperHeroAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuperHeroController : ControllerBase
    {
        private static List<SuperHero> heroes = new List<SuperHero> { new SuperHero
        {
            Id = 1,
            Name = "Spider Man",
            FirstName  = "Peter",
            LastName = "Parker",
            Place = "London",
        } };

        private readonly DataContext _context;
        public SuperHeroController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<SuperHero>>> GetSuperHero()
        {
            var getList = _context.SuperHeroes.ToListAsync();

            return Ok(await getList);
        }

        [HttpGet("GetSuperHeroById/{id}")]
        public async Task<ActionResult<SuperHero>> GetSuperHeroById(int id)
        {
            var getHero = heroes.Find(x => x.Id == id);

            if (getHero == null)
                return BadRequest("Can't find Hero");

            return Ok(getHero);
        }

        [HttpGet("GetSuperHeroByIdList/{idList}")]
        public async Task<ActionResult<List<SuperHero>>> GetSuperHeroByIdList(string idList)
        {

            var getChars = idList.Split(',').ToList();

            var getHero = heroes.FindAll(x => getChars.Contains(x.Id.ToString()));

            if (getHero == null)
                return BadRequest("Can't find Hero");

            return Ok(getHero);
        }

        [HttpPost]
        public async Task<ActionResult<List<SuperHero>>> AddSuperHero(SuperHero hero)
        {
            heroes.Add(hero);

            return Ok(heroes);
        }

        [HttpPut]
        public async Task<ActionResult<List<SuperHero>>> UpdateSuperHero(SuperHero hero)
        {
            var getHero = heroes.Find(x => x.Id == hero.Id);

            if (getHero == null)
                return BadRequest("Can't find Hero");

            getHero.Id = hero.Id;
            getHero.Name = hero.Name;
            getHero.FirstName = hero.FirstName;
            getHero.LastName = hero.LastName;
            getHero.Place = hero.Place;

            return Ok(heroes);
        }

        [HttpDelete]
        public async Task<ActionResult<List<SuperHero>>> DeleteSuperHero(int id)
        {
            var getHero = heroes.Find(x => x.Id == id);

            if (getHero == null)
                return BadRequest("Can't find Hero");

            heroes.Remove(getHero);

            return Ok(heroes);
        }

    }
}
