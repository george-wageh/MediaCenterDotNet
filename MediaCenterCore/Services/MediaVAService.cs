using MediaCenterCore.Data;
using MediaCenterCore.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaCenterCore.Services
{
    public class MediaVAService
    {
        public MediaVAService(MediaCenterDbContext mediaCenterDbContext)
        {
            MediaCenterDbContext = mediaCenterDbContext;
        }

        public MediaCenterDbContext MediaCenterDbContext { get; }

        public async Task<string?> GetFileName(int mediaId)
        {
            var mediaVa = await MediaCenterDbContext.Set<MediaVa>().FirstOrDefaultAsync(x => x.MediaId == mediaId);
            if (mediaVa == null)
            {
                return null;
            }
            else
            {
                return mediaVa.FileName;
            }
        }
    }
}
