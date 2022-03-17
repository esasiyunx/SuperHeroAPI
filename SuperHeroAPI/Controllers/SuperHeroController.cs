using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Linq;

namespace SuperHeroAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles ="User")]
    public class SuperHeroController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IMemoryCache _memoryCache;
        public SuperHeroController(DataContext context, IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
            _context = context;
        }

        [HttpGet("GetSuperHero")]
        public async Task<ActionResult<List<SuperHero>>> GetSuperHero()
        {
            var Getlist = await _memoryCache.GetOrCreateAsync("GetSuperHero", async cacheEntry =>
             {
                 return await _context.SuperHeroes.ToListAsync();
             });


            return Ok(Getlist);
        }

        [HttpGet("GetSuperHeroById/{id}")]
        public async Task<ActionResult<SuperHero>> GetSuperHeroById(int id)
        {
            var getHero = await _memoryCache.GetOrCreateAsync("GetSuperHeroById", async cacheEntry =>
            {
                return await _context.SuperHeroes.Where(x => x.Id == id).FirstOrDefaultAsync();
            });

            if (getHero == null)
                return BadRequest("Can't find Hero");

            return Ok(getHero);
        }

        [HttpGet("GetSuperHeroByIdList/{idList}")]
        public async Task<ActionResult<List<SuperHero>>> GetSuperHeroByIdList(string idList)
        {

            var getChars = idList.Split(',').ToList();

            var getHero = await _memoryCache.GetOrCreateAsync("GetSuperHeroByIdList", async cacheEntry =>
            {
                return await _context.SuperHeroes.Where(x => getChars.Contains(x.Id.ToString())).FirstOrDefaultAsync();
            });

            if (getHero == null)
                return BadRequest("Can't find Hero");

            return Ok(getHero);
        }

        [HttpPost("AddSuperHero")]
        public async Task<ActionResult<List<SuperHero>>> AddSuperHero(SuperHero hero)
        {
            _context.SuperHeroes.Add(hero);
            await _context.SaveChangesAsync();

            return Ok(await _context.SuperHeroes.ToListAsync());
        }

        [HttpPut("UpdateSuperHero")]
        public async Task<ActionResult<List<SuperHero>>> UpdateSuperHero(SuperHero hero)
        {
            var getHero = await _context.SuperHeroes.Where(x => x.Id == hero.Id).FirstOrDefaultAsync();

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

        [HttpDelete("DeleteSuperHero")]
        public async Task<ActionResult<List<SuperHero>>> DeleteSuperHero(int id)
        {
            var getHero = _context.SuperHeroes.Where(x => x.Id == id).FirstOrDefault();

            if (getHero == null)
                return BadRequest("Can't find Hero");

            _context.SuperHeroes.Remove(getHero);

            await _context.SaveChangesAsync();

            return Ok(await _context.SuperHeroes.ToListAsync(););
        }

    }
}
