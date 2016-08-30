using AvalonDock.Layout;
using Stump.DofusProtocol.D2oClasses.Tools.Dlm;
using System.Windows;

namespace WorldEditor.Maps
{
    /// <summary>
    /// Interaction logic for MapEditor.xaml
    /// </summary>
    public partial class MapEditor : Window
    {
        public MapEditor(DlmReader reader)
        {
            InitializeComponent();
            ModelView = new MapEditorModelView(this, reader);
            DataContext = ModelView;
        }

        public MapEditorModelView ModelView
        {
            get;
            private set;
        }

        public LayoutDocument AddTab(string title, object content)
        {
            var document = new LayoutDocument()
            {
                Content = content,
                Title = title,
            };

            DocumentPane.Children.Add(document);

            return document;
        }
    }
}