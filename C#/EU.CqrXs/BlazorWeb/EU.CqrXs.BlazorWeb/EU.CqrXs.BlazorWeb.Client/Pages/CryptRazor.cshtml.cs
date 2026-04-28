using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EU.CqrXs.BlazorWeb.Client.Pages
{
    public class CryptRazorModel : PageModel
    {

        public string TextBoxKey_Text { get; set; } = "";
        public string TextBoxIV_Text { get; set; } = "";

        public void OnGet()
        {
        }


        public async Task SelectCipherMode2_async(string cipherMode)
        {

        }

        public void SelectCipherMode2(string cipherMode)
        {

        }


        public void TextBoxKey_TextChanged()
        {

        }


        public void ImageButtonHash_Click()
        {
        }


        public void ButtonClear_Click()
        {
        }

        public async Task ButtonClear_Click_Async()
        {

        }

        public void ButtonSetPipe_Click()
        {

        }

        public void ButtonHashPipe_Click()
        {

        }

    }
}
