using SignUpProject.Models;

namespace SignUpProject.Services
{
    public class ModelService
    {
        private ViewModel viewModel;

        public ModelService()
        {
            viewModel = new ViewModel();
        }

        public ViewModel GetViewModel()
        {
            return viewModel;
        }
        
    }
}
