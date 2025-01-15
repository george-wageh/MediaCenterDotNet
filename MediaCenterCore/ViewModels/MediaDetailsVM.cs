using MediaCenterCore.Models;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediaCenterCore.ViewModels
{
    public class MediaDetailsVM:MediaVM
    {

        public DateTime? ReleaseDate { get; set; }

        public bool? IsLike { get; set; }

        public bool? IsFav{ get; set; }

        public bool? IsWatchLater{ get; set; }

        public CommentVM? CommentVM { get; set; }

        public IEnumerable<CommentVM>? Comments { get; set; }

    }
}
