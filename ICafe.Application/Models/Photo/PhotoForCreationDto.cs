﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace ICafe.Application.Models.Photo
{
    public class PhotoForCreationDto
    {
        public string Url { get; set; }

        public IFormFile File { get; set; }

        public string Description { get; set; }

        public string PublicId { get; set; }
    }
}
