
using ApplicationCore.Entities;
using ApplicationCore.Interfaces;

namespace Web.WebApiServices.LikesApiService
{
    public class LikesApiService
    {
        private readonly IRepository<Like> _likeRepo;

        public LikesApiService(IRepository<Like> likeRepo)
        {
            _likeRepo = likeRepo;
        }

        public void Like(int userId, int eventId)
        {
            _likeRepo.Add(new Like
            {
                MemberId = userId,
                EventId = eventId
            });
        }

        public void UnLike(int userId, int eventId)
        {
            var likeData = _likeRepo.FirstOrDefault(x => x.MemberId == userId && x.EventId == eventId);

            if (likeData != null)
            {
                _likeRepo.Delete(likeData);
            }
        }

        public int GetLikeCount(int eventId)
        {
            return _likeRepo.List(x => x.EventId == eventId).Count();
        }
    }
}
