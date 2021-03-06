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
        public int TotalQuestions { get; set; }
        public int MarkPerMcq { get; set; }
        public int NegativeMark { get; set; }
        public int TimeInMinutes { get; set; }
        //public List<repeaterField> subjects { get; set; }
        public long[] Subjects { get; set; }
        public int[] NoOfMcqs { get; set; }

    }

   
}