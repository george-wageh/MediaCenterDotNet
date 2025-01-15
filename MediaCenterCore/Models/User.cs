using Microsoft.AspNetCore.Identity;
using System.Collections;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediaCenterCore.Models
{
    public class User:IdentityUser
    {
        public virtual ICollection<MediaAddFav>? MediaFav { get; set; }

        public virtual ICollection<MediaLike>? MediaLike { get; set; }
        public virtual ICollection<MediaWatchLater>? MediaWatchLater { get; set; }

        public string? FullName { get; set; }
    }
}
