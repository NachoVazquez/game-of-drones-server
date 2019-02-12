using AutoMapper;
using GameOfDrones.Core.Domain.Models;
using GameOfDrones.Models.Game;
using GameOfDrones.Models.Round;

namespace GameOfDrones.Config
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {

            #region Game configs
            this.CreateMap<Game, GameIndexViewModel>().AfterMap((src, dest) =>
            {
                dest.Player1Name = src.Player1?.UserName;
                dest.Player2Name = src.Player2?.UserName;
            }).ReverseMap();

            this.CreateMap<Game, GameBaseViewModel>().ReverseMap();
            this.CreateMap<Game, GameCreateViewModel>().ReverseMap();
            this.CreateMap<Game, GameEditViewModel>().ReverseMap();





            #endregion

            #region Round Configs

            this.CreateMap<Round, RoundBaseViewModel>().ReverseMap();
            this.CreateMap<Round, RoundCreateViewModel>().ReverseMap();
            this.CreateMap<Round, RoundIndexViewModel>().ReverseMap();

            #endregion





        }
    }
}
