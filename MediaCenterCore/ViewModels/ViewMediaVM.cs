using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaCenterCore.ViewModels
{
    public class ViewMediaVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Image { get; set; }
        public IEnumerable<string> Directors { get; set; }
        public IEnumerable<string> Actors { get; set; }
        public IEnumerable<CommentVM>? Comments { get; set; }
        public string FileName { get; set; }

    }
}
