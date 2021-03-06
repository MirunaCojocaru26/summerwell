﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConferencePlanner.Abstraction.Model;
using ConferencePlanner.Abstraction.Repository;
using ConferencePlanner.Repository.Ef.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ConferencePlanner.Api.Controllers
{
    public class DictionaryCategoryController : Controller
    {
        private readonly ILogger<DictionaryCategoryController> _logger;
        private readonly IDictionaryConferenceCategoryRepository _conferenceCategoryRepository;
        public DictionaryCategoryController(ILogger<DictionaryCategoryController> logger, IDictionaryConferenceCategoryRepository conferenceCategoryRepository)
        {
            _logger = logger;
            _conferenceCategoryRepository = conferenceCategoryRepository;

        }
       
        [HttpGet]
        [Route("DictionaryConferenceCategory")]
        public IActionResult GetDictionaryCategory()
        {
            List<DictionaryConferenceCategoryModel> conferencesCategories= _conferenceCategoryRepository.GetDictionaryCategory();
            return Ok(conferencesCategories);
        }

        [HttpPost]
        [Route("DictionaryCategory/ConferenceId")]
        public IActionResult GetCategory([FromBody] int conferenceId)
        {
            DictionaryConferenceCategoryModel category = _conferenceCategoryRepository.GetDictionaryCategory(conferenceId);
            return Ok(category);
        }

        [HttpPost]
        [Route("AddCategory")]
        public IActionResult AddCategory([FromBody] string Name)
        {
            _conferenceCategoryRepository.AddCategory(Name);
            return Ok();
        }
    }
}
