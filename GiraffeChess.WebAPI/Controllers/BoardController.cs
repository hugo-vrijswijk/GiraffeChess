﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GiraffeChess.DomainService.Command;
using GiraffeChess.DomainService.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GiraffeChess.WebAPI.Controllers
{
    [Produces("application/json")]
    [Route("[controller]")]
    public class BoardController : Controller
    {
        private IGameService GameService { get; }

        public BoardController(IGameService gameService)
        {
            GameService = gameService;
        }
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(GameService.NewGame());
        }

        [HttpPost]
        public IActionResult Move([FromBody] MoveCommand command)
        {
            return Ok(GameService.Move(command));
        }


    }
}