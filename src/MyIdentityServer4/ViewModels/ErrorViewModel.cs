namespace MyIdentityServer4.ViewModels
{
    using IdentityServer4.Models;

    public class ErrorViewModel
    {
        public ErrorViewModel()
        {
        }

        public ErrorViewModel(string error) => this.Error = new ErrorMessage { Error = error };

        public ErrorMessage Error { get; set; }
    }
}
