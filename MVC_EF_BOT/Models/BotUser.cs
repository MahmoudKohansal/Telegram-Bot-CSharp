using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVC_EF_BOT.Models
{
    public class BotUser
    {
        public long ID { get; set; }

        [Index(IsUnique = true)]
        public long teleID { get; set; }
        public bool getNews { get; set; }
    }
}