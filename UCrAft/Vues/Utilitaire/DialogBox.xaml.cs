using System.Windows;
using System.Windows.Input;

namespace Vues.Utilitaire
{
    /// <summary>
    /// Logique d'interaction pour DialogBox.xaml
    /// </summary>
    public partial class DialogBox : Window
    {
        public uint choix;
        public DialogBox(string titre, string question, string choix1, string choix2)
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            BouttonChoix1.Content = choix1;
            BouttonChoix2.Content = choix2;
            TextBlockQuestion.Text = question;
            Title = titre;
        }

        private void BouttonChoix_Click(object sender, RoutedEventArgs e)
        {
            if (BouttonChoix1.Equals(sender))
            {
                choix = 1;
            }
            else if (BouttonChoix2.Equals(sender))
            {
                choix = 2;
            }
            this.DialogResult = true;
        }

        private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void CloseCommande(object sender, ExecutedRoutedEventArgs e)
        {
            Close();
        }
    }
}
