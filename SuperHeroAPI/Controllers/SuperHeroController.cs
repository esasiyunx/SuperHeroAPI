using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace SuperHeroAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuperHeroController : ControllerBase
    {
        private readonly DataContext _context;
        public SuperHeroController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<SuperHero>>> GetSuperHero()
        {
            var getList = await _context.SuperHeroes.ToListAsync();

            return Ok(getList);
        }

        [HttpGet("GetSuperHeroById/{id}")]
        public async Task<ActionResult<SuperHero>> GetSuperHeroById(int id)
        {
            var getHero = _context.SuperHeroes.Where(x=> x.Id == id).FirstOrDefault();

            if (getHero == null)
                return BadRequest("Can't find Hero");

            return Ok(getHero);
        }

        [HttpGet("GetSuperHeroByIdList/{idList}")]
        public async Task<ActionResult<List<SuperHero>>> GetSuperHeroByIdList(string idList)
        {

            var getChars = idList.Split(',').ToList();

            var getHero = await _context.SuperHeroes.Where(x => getChars.Contains(x.Id.ToString())).ToListAsync();

            if (getHero == null)
                return BadRequest("Can't find Hero");

            return Ok(getHero);
        }

        [HttpPost]
        public async Task<ActionResult<List<SuperHero>>> AddSuperHero(SuperHero hero)
        {
            _context.SuperHeroes.Add(hero);
            await _context.SaveChangesAsync();

            return Ok(_context.SuperHeroes.ToListAsync());
        }

        [HttpPut]
        public async Task<ActionResult<List<SuperHero>>> UpdateSuperHero(SuperHero hero)
        {
            var getHero = _context.SuperHeroes.Where(x => x.Id == hero.Id).FirstOrDefault();

            if (getHero == null)
                return BadRequest("Can't find Hero");

            getHero.Id = hero.Id;
            getHero.Name = hero.Name;
            getHero.FirstName = hero.FirstName;
            getHero.LastName = hero.LastName;
            getHero.Place = hero.Place;

            await _context.SaveChangesAsync();

            return Ok(await _context.SuperHeroes.ToListAsync());
        }

        [HttpDelete]
        public async Task<ActionResult<List<SuperHero>>> DeleteSuperHero(int id)
        {
            var getHero =  _context.SuperHeroes.Where(x=> x.Id == id).FirstOrDefault();

            if (getHero == null)
                return BadRequest("Can't find Hero");

            _context.SuperHeroes.Remove(getHero);

            await _context.SaveChangesAsync();

            return Ok(await _context.SuperHeroes.ToListAsync());
        }

    }
}
