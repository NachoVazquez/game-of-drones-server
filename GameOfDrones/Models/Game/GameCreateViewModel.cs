using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GameOfDrones.Models.Game
{
    public class GameCreateViewModel
    {
        [Required]
        public string Player1Name { get; set; }
        [Required]
        public string Player2Name { get; set; }
    }
}
