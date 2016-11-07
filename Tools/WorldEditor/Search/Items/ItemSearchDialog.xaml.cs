using System.Windows;

namespace WorldEditor.Search.Items
{
    /// <summary>
    /// Interaction logic for ItemSearchDialog.xaml
    /// </summary>
    public partial class ItemSearchDialog : Window
    {
        public ItemSearchDialog()
        {
            InitializeComponent();
            ModelView = new ItemSearchDialogModelView();
            DataContext = ModelView;
        }

        public ItemSearchDialogModelView ModelView
        {
            get;
            set;
        }
    }
}