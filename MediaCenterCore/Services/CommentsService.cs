using MediaCenterCore.IRepositories;
using MediaCenterCore.Models;
using MediaCenterCore.Shared;
using MediaCenterCore.ViewModels;
using System.Security.Claims;

namespace MediaCenterCore.Services
{
    public class CommentsService
    {
        public CommentsService(ICommentsRapository commentsRapository, UnitAppWork unitAppWork)
        {
            CommentsRapository = commentsRapository;
            UnitAppWork = unitAppWork;
        }

        public ICommentsRapository CommentsRapository { get; }
        public UnitAppWork UnitAppWork { get; }

        public async Task<OperationResult<object>> AddCommentAsync(CommentVM CommentVM , string userName)
        {
            var comment = new MediaComment
            {
                UserName = userName,
                Comment = CommentVM.Comment,
                MediaId = CommentVM.MediaId,
                Title = CommentVM.Title
            };
            await CommentsRapository.AddAsync(comment);
            await UnitAppWork.SaveChanges();
            return new OperationResult<object>
            {
                IsSuccess = true,
                Object = null
            };
        }


        public async Task<OperationResult<object>> RemoveCommentAsync(int commentId) { 
            var comment = await CommentsRapository.GetAsync(commentId);
            if (comment != null) { 
                CommentsRapository.Remove(comment);
                await UnitAppWork.SaveChanges();
            }
            return new OperationResult<object>
            {
                IsSuccess = true
            };
        }

        public async Task<IEnumerable<CommentVM>> GetAllInMediaAsync(int mediaId)
        {
            var comments = await CommentsRapository.GetAllAsync(mediaId);
            var commentsVM = comments.Select(x => new CommentVM
            {
                ID = x.Id,
                Comment = x.Comment,
                MediaId = x.MediaId,
                Title = x.Title,
                UserName = x.UserName,
            });
            return commentsVM;
        }
    }
}
