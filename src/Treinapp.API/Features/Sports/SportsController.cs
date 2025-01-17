﻿using MediatR;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Treinapp.Common;
using Treinapp.Commons.Domain;

namespace Treinapp.API.Features.Sports
{
    [ApiController]
    [Route(Constants.DefaultRoute)]
    public class SportsController : ControllerBase
    {
        private readonly ISender sender;

        private CancellationToken Token => HttpContext?.RequestAborted ?? default;

        public SportsController(ISender sender)
        {
            this.sender = sender ?? throw new System.ArgumentNullException(nameof(sender));
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Sport>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ListAll()
        {
            IEnumerable<Sport> result = await sender.Send(new ListSports(), Token);
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Sport), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateNew([FromBody] CreateSport command)
        {
            Sport result = await sender.Send(command, Token);
            return CreatedAtAction(nameof(ListAll), result);
        }
    }
}
