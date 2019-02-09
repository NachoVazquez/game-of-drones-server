using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GameOfDrones.Core.Abstractions.Business;
using GameOfDrones.Core.Abstractions.DataAccess;
using GameOfDrones.Core.Domain.Base;
using GameOfDrones.DataAccess.UnitOfWork;
using Microsoft.AspNetCore.Mvc;

namespace GameOfDrones.Controllers
{
    [Produces("application/json")]
    [Route("api/v1/[controller]/[action]")]
    public class BaseApiController<TApplicationService,
        TEntity,
        TKey,
        TBaseViewModel,
        TCreateViewModel,
        TEditViewModel,
        TIndexViewModel> : Controller
        where TApplicationService : IApplicationService<TEntity, TKey>
        where TEntity : Entity<TKey>
    {
        /// <summary>
        ///     The appService for the current controller.
        /// </summary>
        protected TApplicationService ApplicationService { get; set; }

        /// <summary>
        ///     An <see cref="AutoMapper"/> instance.
        /// </summary>
        protected IMapper Mapper { get; set; }

        /// <summary>
        ///  An <see cref="IUnitOfWork"/> instance.
        /// </summary>
        protected IUnitOfWork UnitOfWork { get; set; }

        public BaseApiController(TApplicationService appService, IMapper mapper, IUnitOfWork unitOfWork)
        {
            ApplicationService = appService;
            Mapper = mapper;
            UnitOfWork = unitOfWork;
        }


        /// <summary>
        ///     Gets all entities.
        /// </summary>
        /// <returns>All entities</returns>
        /// <response code="200"></response>
        [HttpGet]
        public virtual async Task<IActionResult> GetAll()
        {
            var result = (await ApplicationService.ReadAllAsync(_ => true)).ToList();

            var resultViewModel = Mapper.Map<List<TIndexViewModel>>(result);

            return Ok(resultViewModel);
        }


        /// <summary>
        ///     Gets an entity by its ID
        /// </summary>
        /// <param name="objectId">The id of the entity to return</param>
        /// <returns>The entity with ID=<paramref name="id"/>, null if not found</returns>
        /// <response code="200">When the entity is found by its id</response>
        /// <response code="404">When the entity couldn't be found</response>
        [HttpGet("{id}")]
        public virtual async Task<IActionResult> Get(TKey objectId)
        {
            var result = await ApplicationService.SingleOrDefaultAsync(objectId);

            if (result == null)
                return NotFound();
            var resultViewModel = Mapper.Map<TIndexViewModel>(result);

            return Ok(resultViewModel);
        }

        /// <summary>
        /// Creates a new entity
        /// </summary>
        /// <param name="newObject">The new entity</param>
        /// <returns>The created entity</returns>
        /// <response code="201">When the entity was successfully created</response>
        /// <response code="400">When the entity model was not in a correct state and validation failed</response>
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public virtual async Task<IActionResult> Post([FromBody, Required] TCreateViewModel newObject)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            TEntity newEntity;
            try
            {
                newEntity = Mapper.Map<TCreateViewModel, TEntity>(newObject);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }

            newEntity = await ApplicationService.AddAsync(newEntity);
            await ApplicationService.SaveChangesAsync();

            return CreatedAtAction("Post", new {id = newEntity.Id}, Mapper.Map<TEntity, TIndexViewModel>(newEntity));
        }

        /// <summary>
        /// Updates an entity
        /// </summary>
        /// <param name="id">The id of the entity to update</param>
        /// <param name="value">The updated entity</param>
        /// <returns>The updated entity</returns>
        /// <response code="200">When the entity was successfully updated</response>
        /// <response code="400">When the entity model was not in a correct state and validation failed</response>
        /// <response code="404">When the entity to update was not found</response>
        [HttpPut("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public virtual async Task<IActionResult> Put(TKey id, [FromBody] TEditViewModel value)
        {
            var originalEntity = await ApplicationService.SingleOrDefaultAsync(id);

            if (originalEntity == null)
                return NotFound(id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var entity = Mapper.Map(value, originalEntity);

            entity = await ApplicationService.UpdateAsync(entity);
            await ApplicationService.SaveChangesAsync();

            return Ok(Mapper.Map<TEntity, TIndexViewModel>(entity));
        }

        /// <summary>
        /// Deletes an entity
        /// </summary>
        /// <param name="id">The ID of the entity to delete</param>
        /// <returns>The deleted entity</returns>
        /// <response code="200">When the entity was successfully deleted</response>
        /// <response code="404">When the entity to delete was not found</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public virtual async Task<IActionResult> Delete(TKey id)
        {
            var entity = await ApplicationService.SingleOrDefaultAsync(id);
            if (entity != null)
            {
                await ApplicationService.RemoveAsync(entity);
                await ApplicationService.SaveChangesAsync();

                return Ok();
            }
            else return NotFound();
        }
    }
}