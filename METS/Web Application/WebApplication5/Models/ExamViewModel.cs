﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication5.Models
{
    public class ExamViewModel
    {
        public string Name { get; set; }
        public HttpPostedFileBase Image { get; set; }
        public string Cover { get; set; }

    }
}