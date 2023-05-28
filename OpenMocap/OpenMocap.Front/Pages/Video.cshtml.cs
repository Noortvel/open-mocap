using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OpenMocap.Front.Pages
{
    public class VideoModel : PageModel
    {
        public string OperationId { get; private set; }

        public void OnGet(Guid id)
        {
            OperationId = id.ToString();
        }
    }
}
