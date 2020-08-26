﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ConferencePlanner.Abstraction.Model
{
    public class AddEventDetailModel
    {
        public int ConferenceTypeId { get; set; }
        public string ConferenceTypeName { get; set; }
        public int DictionaryCountryId { get; set; }
        public string DictionaryCountryCity { get; set; }
        public string SpeakerName { get; set; }
        public string SpeakerRating { get; set; }
        public int DictionaryCountyId { get; set; }
        public string DictionaryCountyName { get; set; }
        public int DictionaryConferenceCategoryId { get; set; }
        public string DictionaryConferenceCategoryName { get; set; }

    }
}