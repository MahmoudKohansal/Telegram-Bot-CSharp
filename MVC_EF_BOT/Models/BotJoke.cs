using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVC_EF_BOT.Models
{
    public class BotJoke
    {
        public long ID { get; set; }
        public long teleID { get; set; }
        public string joke { get; set; }
        
    }
}