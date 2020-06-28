using Dapper;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using BlazorDapperCRUD.Data.Model;

namespace BlazorDapperCRUD.Data.Service
{
    public class VideoService : IVideoService
    {
        //Database Connection
        private readonly SqlConnectionConfiguration _configuration;
        public VideoService(SqlConnectionConfiguration configuration)
        {
            _configuration = configuration;
        }

        //Add (create) a video table row (SQL Insert)
        public async Task<bool> VideoInset(Video video)
        {
            using (var conn = new SqlConnection(_configuration.Value))
            {
                var parameters = new DynamicParameters();
                parameters.Add("Title", video.Title, DbType.String);
                parameters.Add("DatePublished", video.DatePublished, DbType.Date);
                parameters.Add("IsActive", video.IsActive, DbType.Boolean);
                //metodo de stored procedure
                await conn.ExecuteAsync("spVideo_Insert", parameters, commandType: CommandType.StoredProcedure);

                // Raw SQL method.
                //const string query = @"INSERT INTO tbl_Video (Title, DatePublished, IsActive) VALUES (@Title, @DatePublished, @IsActive)";
                //await conn.ExecuteAsync(query, new { video.Title, video.DatePublished, video.IsActive }, commandType: CommandType.Text);
            }
            return true;
        }

        //Obtem uma lista de videos
        public async Task<IEnumerable<Video>> VideoList()
        {
            IEnumerable<Video> video;
            using (var conn = new SqlConnection(_configuration.Value))
            {
                video = await conn.QueryAsync<Video>("spVideo_GetAll", commandType: CommandType.StoredProcedure);
            }

            return video;
        }

        //Obter um video por base do Id
        public async Task<Video> Video_GetOne(int Id)
        {
            Video video = new Video();
            var parameters = new DynamicParameters();
            parameters.Add("Id", Id, DbType.Int32);

            using (var conn = new SqlConnection(_configuration.Value))
            {
                video = await conn.QueryFirstOrDefaultAsync<Video>("sp.Video_GetOne", parameters, commandType: CommandType.StoredProcedure);
            }

            return video;
        }


        //Atualizar video basiado na linha e no videoId
        public async Task<bool> VideoUpdate(Video video)
        {
            using (var conn = new SqlConnection(_configuration.Value))
            {
                var parameters = new DynamicParameters();
                parameters.Add("VideoId", video.VideoID, DbType.Int32);
                parameters.Add("Title", video.Title, DbType.String);
                parameters.Add("DatePublished", video.DatePublished, DbType.Date);
                parameters.Add("IsActive", video.IsActive, DbType.Boolean);
                //metodo de stored procedure
                await conn.ExecuteAsync("spVideo_Update", parameters, commandType: CommandType.StoredProcedure);

            }
            return true;
        }

        public async Task<bool> VideoDelete(int Id)
        {
            var parameters = new DynamicParameters();
            parameters.Add("Id", Id, DbType.Int32);
            using (var conn = new SqlConnection(_configuration.Value))
            {
                await conn.ExecuteAsync("spVideo_DELETE", parameters, commandType: CommandType.StoredProcedure);
            }
        return true;
        }
    }
}
