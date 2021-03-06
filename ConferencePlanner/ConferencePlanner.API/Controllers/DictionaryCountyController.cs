﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConferencePlanner.Abstraction.Model;
using ConferencePlanner.Abstraction.Repository;
using ConferencePlanner.Repository.Ef.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ConferencePlanner.Api.Controllers
{
    public class DictionaryCountyController : Controller
    {
        private readonly ILogger<DictionaryCountyController> _logger;
        private readonly IDictionaryCountyRepository _countyRepository;
        public DictionaryCountyController(ILogger<DictionaryCountyController> logger, IDictionaryCountyRepository countyRepository)
        {
            _logger = logger;
            _countyRepository = countyRepository;
        }

        [HttpGet]
        [Route("DictionaryCounty")]
        public IActionResult GetDictionaryCounty()
        {
            List<DictionaryCountyModel> county = _countyRepository.GetDictionaryCounty();
            return Ok(county);
        }

        [HttpPost]
        [Route("DictionaryCounty/AddCounty")]
        public IActionResult AddCounty([FromBody] string Code, [FromBody] string Name, [FromBody] string country)
        {
            _countyRepository.AddCounty(Code, Name, country);
            return Ok();
        }

        [HttpPut]
        [Route("DictionaryCounty/EditCounty")]
        public IActionResult EditCounty(string Code, string Name, int CountyId)
        {
            _countyRepository.EditCounty(Code, Name, CountyId);
            return Ok();
        }

        [HttpDelete]
        [Route("DictionaryCounty/DeleteCounty")]
        public IActionResult DeleteCounty(int CountyId)
        {
            bool x = _countyRepository.DeleteCounty(CountyId);
            if (x == true)
                return Ok();
            else return NoContent();
        }
    }
}
