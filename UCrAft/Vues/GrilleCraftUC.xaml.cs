using Modele;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Vues
{
    /// <summary>
    /// Logique d'interaction pour UserControl1.xaml
    /// </summary>
    public partial class GrilleCraftUC : UserControl
    {
        private readonly bool modification;
        private ManagerVue ManagerVue => (Application.Current as App).managerVue;

        private Dictionary<Button, EPosition> positionsButtons;

        public GrilleCraftUC(bool modeMofication = false)
        {
            modification = modeMofication;
            InitializeComponent();
            DataContextChanged += SetupElements;
        }

        private void SetupElements(object sender, DependencyPropertyChangedEventArgs e)
        {
            Item itemContext = (DataContext as Item);
            if (itemContext is null) return;
            GridCraft.Children.Clear();

            positionsButtons = new Dictionary<Button, EPosition>();

            var pattern = itemContext.CraftItem.Pattern;

            foreach (EPosition pos in Enum.GetValues(typeof(EPosition)))
            {
                Button button = new Button();
                button.Style = FindResource("ButtonCraft") as Style;

                GridCraft.Children.Add(button);
                Grid.SetRow(button, ((int)pos) / 3);
                Grid.SetColumn(button, ((int)pos) % 3);

                positionsButtons[button] = pos;

                button.Click += ElementClicked;
                button.MouseRightButtonUp += ClicDroitSurButton;

                if (pattern.ContainsKey(pos))
                {
                    Objet objet = pattern[pos];
                    var element = new UCPetitsElements.ObjetReduitUC();
                    button.DataContext = objet;
                    button.Content = element;
                }
            }
        }

        private void ClicDroitSurButton(object sender, MouseButtonEventArgs e)
        {
            if (modification)
            {
                Button button = sender as Button;
                (DataContext as Item).CraftItem.Pattern.Remove(positionsButtons[button]);
                button.DataContext = null;
            }
        }

        private void ElementClicked(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;

            if (modification)
            {
                if (ManagerVue.ObjetSelectionnéPourCreerLesCrafts != null)
                {
                    if (DataContext.Equals(button.DataContext))
                    {
                        button.Content = new UCPetitsElements.ObjetReduitUC();
                    }
                    (DataContext as Item).CraftItem.Pattern[positionsButtons[button]] = ManagerVue.ObjetSelectionnéPourCreerLesCrafts;
                    button.DataContext = ManagerVue.ObjetSelectionnéPourCreerLesCrafts;
                }
            }
            else if (button.DataContext as Objet != null)
            {
                ManagerVue.DemandeChangementObjetAffiché(button.DataContext as Objet);
            }
        }
    }
}
