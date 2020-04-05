using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Persistence.Entities;

namespace Web.Hubs
{
    public interface IImageOperationsClient
    {
        Task AddImage(Image image);
        Task RemoveImage(Image image);
        Task EditImage(Image image);
    }

    public class ImageOperationsHub : Hub<IImageOperationsClient> { }
}
