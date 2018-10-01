namespace ToDoApi.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.Annotations;
    using models = Infrastructure.Models;

    [Route("[controller]")]
    [ApiController]
    public class ToDoController : ControllerBase
    {
        /// <summary>
        /// returns all of the available lists
        /// </summary>
        /// <param name="searchString">pass an optional search string for looking up a list</param>
        /// <param name="skip">number of records to skip for pagination</param>
        /// <param name="limit">maximum number of records to return</param>
        /// <returns></returns>
        /// <response code="200">search results matching criteria</response>
        /// <response code="400">bad input parameter</response>
        [Route("lists")]
        [HttpGet]
        [SwaggerResponse(200, "search results matching criteria", typeof(List<models.List>))]
        [SwaggerResponse(400, "bad input parameter")]
        public async Task<IActionResult> GetLists([FromQuery]string searchString, [FromQuery]int skip, [FromQuery]int limit)
        {
            return this.Ok();
        }

        /// <summary>
        /// creates a new list
        /// </summary>
        /// <param name="todoList">ToDo list to add</param>
        /// <returns></returns>
        /// <response code="201">item created</response>
        /// <response code="400">invalid input, object invalid</response>
        /// <response code="409">an existing item already exists</response>
        [Route("lists")]
        [HttpPost]
        [SwaggerResponse(201, "item created")]
        [SwaggerResponse(400, "invalid input, object invalid")]
        [SwaggerResponse(409, "an existing item already exists")]
        public async Task<IActionResult> CreateList([FromBody]models.List todoList)
        {
            return this.Ok();
        }

        /// <summary>
        /// return the specified todo list
        /// </summary>
        /// <param name="id">The unique identifier of the list</param>
        /// <returns></returns>
        /// <response code="200">successful operation</response>
        /// <response code="400">Invalid id supplied</response>
        /// <response code="409">List not found</response>
        [Route("list/{id}")]
        [HttpGet]
        [SwaggerResponse(200, "successful operation", typeof(models.List))]
        [SwaggerResponse(400, "Invalid id supplied")]
        [SwaggerResponse(404, "List not found")]
        public async Task<IActionResult> GetList([FromQuery]string id)
        {
            return this.Ok();
        }

        /// <summary>
        /// add a new task to the todo list
        /// </summary>
        /// <param name="id">Unique identifier of the list to add the task for</param>
        /// <param name="task">task to add</param>
        /// <returns></returns>
        /// <response code="201">item created</response>
        /// <response code="400">invalid input, object invalid</response>
        /// <response code="409">an existing item already exists</response>
        [Route("lists/{id}/tasks")]
        [HttpPost]
        [SwaggerResponse(201, "item created")]
        [SwaggerResponse(400, "invalid input, object invalid")]
        [SwaggerResponse(409, "an existing item already exists")]
        public async Task<IActionResult> CreateTask([FromQuery]string id, [FromBody]models.Task task)
        {
            return this.Ok();
        }

        /// <summary>
        /// updates the completed state of a task
        /// </summary>
        /// <param name="id">Unique identifier of the list to add the task for</param>
        /// <param name="taskId">Unique identifier task to complete</param>
        /// <returns></returns>
        /// <response code="201">item updated</response>
        /// <response code="400">invalid input, object invalid</response>
        /// <response code="404">item not found</response>
        /// <summary>The original swagger had a different task object here, which is unnecessary</summary>
        [Route("lists/{id}/task/{taskId}/complete")]
        [HttpPost]
        [SwaggerResponse(201, "item updated")]
        [SwaggerResponse(400, "invalid input, object invalid")]
        [SwaggerResponse(404, "item not found")]
        public async Task<IActionResult> CompleteTask([FromQuery]string id, [FromQuery]string taskId)
        {
            return this.Ok();
        }
    }
}