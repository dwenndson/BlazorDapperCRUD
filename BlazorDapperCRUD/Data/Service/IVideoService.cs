using BlazorDapperCRUD.Data.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorDapperCRUD.Data.Service
{
    public interface IVideoService
    {
        Task<bool> VideoInset(Video video);
        Task<IEnumerable<Video>> VideoList();
        Task<Video> Video_GetOne(int Id);
        Task<bool> VideoUpdate(Video video);
        Task<bool> VideoDelete(int Id);
    }
}