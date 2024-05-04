using ApplicationCore.Entities;
using ApplicationCore.Interfaces;

namespace Web.WebApiServices.FollowServices
{
    public class FollowApiService
    {
        private readonly IRepository<Follow> _followRepo;

        public FollowApiService(IRepository<Follow> followRepo)
        {
            _followRepo = followRepo;
        }

        public void Follow(int followId, int beingFollowedId)
        {
            _followRepo.Add(new Follow
            {
                FollowerId = followId,
                BeingFollowedId = beingFollowedId
            });
        }

        public void UnFollow(int followId, int beingFollowedId)
        {
            var followData = _followRepo.FirstOrDefault(x => x.FollowerId == followId && x.BeingFollowedId == beingFollowedId);

            if (followData != null)
            {
                _followRepo.Delete(followData);
            }
        }

        public int GetFansCount(int beingFollowedId)
        {
            return _followRepo.List(f => f.BeingFollowedId == beingFollowedId).Count();
        }
    }
}
