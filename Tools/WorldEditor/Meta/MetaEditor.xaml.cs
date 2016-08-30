﻿using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace WorldEditor.Meta
{
    /// <summary>
    /// Interaction logic for MetaEditor.xaml
    /// </summary>
    public partial class MetaEditor : Window
    {
        public MetaEditor(MetaFile file)
        {
            InitializeComponent();
            ModelView = new MetaEditorModelView(this, file);
            DataContext = ModelView;
        }

        public MetaEditorModelView ModelView
        {
            get;
            private set;
        }

        private void EntriesGrid_PreviewDragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, true))
            {
                var filenames = (string[])e.Data.GetData(DataFormats.FileDrop, true);
                if (filenames.Any(File.Exists))
                    e.Effects = DragDropEffects.Copy;
            }

            e.Handled = true;
        }

        private void EntriesGrid_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, true))
            {
                var filenames = (string[])e.Data.GetData(DataFormats.FileDrop, true);
                foreach (var filename in filenames)
                {
                    if (File.Exists(filename))
                    {
                        ModelView.AddFile(filename);
                    }
                }
            }

            e.Handled = true;
        }

        private void EntriesGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            ModelView.RemoveCommand.RaiseCanExecuteChanged();
        }
    }
}