using AutoMapper;
using GameOfDrones.Core.Abstractions.Business;
using GameOfDrones.Core.Abstractions.DataAccess;
using GameOfDrones.Core.Domain.Models;
using GameOfDrones.Models;
using GameOfDrones.Models.Game;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GameOfDrones.Controllers
{
    public class GamesController : BaseApiController<IGameService, Game, int, GameIndexViewModel, GameCreateViewModel, GameIndexViewModel, GameIndexViewModel>
    {

        public GamesController(IGameService appService, IMapper mapper, IUnitOfWork unitOfWork) : base(appService, mapper, unitOfWork)
        {

        }

        [HttpPost]
        public async Task<IActionResult> CreateByPlayersName(GameCreateViewModel data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var created = await this.ApplicationService.CreateGameFromPlayerNamesAsync(data.Player1Name, data.Player2Name);

            if (created == null)
                return StatusCode(500);

            return CreatedAtAction("Post", new { Id = created.Id }, Mapper.Map<Game, GameIndexViewModel>(created));

        }
    }
}
