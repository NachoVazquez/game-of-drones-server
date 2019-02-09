using AutoMapper;
using GameOfDrones.Core.Domain.Models;
using GameOfDrones.Models;

namespace GameOfDrones.Config
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {

            #region Game configs
            this.CreateMap<Game, GameIndexViewModel>().AfterMap((src, dest) => {
                dest.Player1Name = src.Player1.UserName;
                dest.Player2Name = src.Player2.UserName;
            }).ReverseMap();

            #endregion





        }
    }
}
