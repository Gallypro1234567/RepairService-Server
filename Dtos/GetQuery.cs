using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace WorkAppReactAPI.Dtos
{
    public class Query
    {
        public string Code { get; set; }
        public int Start { get; set; }
        public int Lenght { get; set; }
        public int Order { get; set; }
        public int status { get; set; }
    }

}