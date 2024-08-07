﻿using CafeApp_api.DTO;
using CafeApp_api.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CafeApp_api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CafeController : ControllerBase
	{
        private readonly ICafeService _cafeService;
        public CafeController(ICafeService cafeService)
        {
              _cafeService = cafeService; 
        }

        [HttpGet("GetUsers")]
        public async Task<List<Users>> GetUsersAsync()
        {
            var users = await _cafeService.GetAllUsers();
            return users;
        }
    }
}
