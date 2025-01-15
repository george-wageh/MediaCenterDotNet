using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaCenterCore.Models
{
    public class MediaVa
    {
        public int Id { get; set; }

        public int MediaId { get; set; }
        [ForeignKey(nameof(MediaId))]
        public Media Media { get; set; }

        public string FileName { get; set; }

    }
}
