using FreshMvvm;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TherapyBoxDemo.Interface;

namespace TherapyBoxDemo.PageModels
{
    public class PicturesPageModel : FreshBasePageModel
    {
        IMediaService mediaService;
        public PicturesPageModel()
        {

        }
        private FreshAwaitCommand _addNewImageCommand;
        public FreshAwaitCommand AddNewImageCommand
        {
            get
            {
                if (_addNewImageCommand == null)
                {
                    _addNewImageCommand = new FreshAwaitCommand(async (tcs) =>
                    {
                        await AddNewImageCommandHandler();
                        tcs.SetResult(true);
                    });
                }
                return _addNewImageCommand;
            }

        }
        private async Task AddNewImageCommandHandler()
        {
            mediaService = FreshIOC.Container.Resolve<IMediaService>();
            await mediaService.OpenGallery();

        }
    }
}
