using System.Windows;

namespace Vues
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public ManagerVue managerVue;

        public App()
        {
            managerVue = new ManagerVue();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            managerVue.Sauvegarde();
            base.OnExit(e);
        }
    }
}
