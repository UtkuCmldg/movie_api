using AutoMapper;
using movie_api.Models;
using movie_api.ViewModels;

namespace movie_api
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //Telling automapper that it should map MovieModel.Movie_Original_Language_Name to MovieViewModel.Original_Language_Of_Film and MovieModel.Overview to MovieViewModel.Overview_Of_Film
            CreateMap<MovieModel, MovieViewModel>()
                .ForMember(dest => dest.Original_Language_Of_Film, opt => opt.MapFrom(src => src.Movie_Original_Language_Name))
                .ForMember(dest => dest.Overview_Of_Film, opt => opt.MapFrom(src => src.Overview));
        }
    }
}
