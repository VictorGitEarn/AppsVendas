using Apps.Domain.Business.Interfaces;
using Apps.Domain.Business.Notes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MongoDB.Bson;

namespace Apps.APIRest.Controllers
{
    [ApiController]
    public abstract class MainController : ControllerBase
    {
        private readonly INotes _notes;

        protected ObjectId UserId { get; set; }

        protected MainController(INotes notes, IUser userApp)
        {
            _notes = notes;

            if (userApp.IsAuthenticated())
            {
                UserId = userApp.GetUserId();
            }
        }

        protected bool IsValid()
        {
            return !_notes.HasNotes();
        }

        protected ActionResult CustomResponse(object? result = null)
        {
            if (IsValid())
            {
                return Ok(new
                {
                    success = true,
                    data = result
                });
            }

            return BadRequest(new
            {
                success = false,
                errors = _notes.GetNotes().Select(n => n.Message)
            });
        }

        protected ActionResult CustomResponse(ModelStateDictionary modelState)
        {
            if (!modelState.IsValid) NotificarErroModelInvalida(modelState);

            return CustomResponse();
        }

        protected void NotificarErroModelInvalida(ModelStateDictionary modelState)
        {
            var erros = modelState.Values.SelectMany(e => e.Errors);

            foreach (var erro in erros)
            {
                var errorMsg = erro.Exception == null ? erro.ErrorMessage : erro.Exception.Message;
                NotifyError(errorMsg);
            }
        }

        protected void NotifyError(string mensagem)
        {
            _notes.Handle(new Notification(mensagem));
        }

    }
}
