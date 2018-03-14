using AutoMapper;
using Database;
using Database.Repository;
using Database.UnitOfWork;
using DTO;
using Services.Interface;
using System;
using System.Linq;

namespace Services
{
    public class CommentService:ICommentService
    {
        IGenericRepository<Comment> _comment;
        IGenericRepository<Post> _post;
        IGenericRepository<User> _user;
        IUnitOfWork _unitOfWork;
        public CommentService(IGenericRepository<Comment> comment, IGenericRepository<Post> post, IGenericRepository<User> user, IUnitOfWork unitOfWork)
        {
            _comment = comment;
            _post = post;
            _user = user;
            _unitOfWork = unitOfWork;
        }

        public bool IsDeleteSuccessful(int commentId,string email)
        {
            Comment comment = _comment.GetAll().FirstOrDefault(c => c.User.Email == email && c.ID == commentId);
            if(comment!=null)
            {
                _comment.Delete(comment);
                return _unitOfWork.Commit();               
            }
            return false;
        }

        public int? IsCreateSuccessful(CommentDTO commentDTO)
        {
            Post post= _post.GetById(commentDTO.PostId);
            Comment comment = Mapper.Map<CommentDTO, Comment>(commentDTO);
            comment.User = _user.GetAll().FirstOrDefault(c => c.Email==commentDTO.Email);
            comment.CreateDate = DateTime.Now;
            post.Comments.Add(comment);
            _post.Update(post);
            _unitOfWork.Commit();
            return comment.ID;
        }
    }
}
