using Apps.Domain.Business.Interfaces;

namespace Apps.APIRest.Controllers.V1
{
    public class SellingController : MainController
    {
        public SellingController(INotes notes, IUser userApp) : base(notes, userApp)
        {

        }


    }
}
