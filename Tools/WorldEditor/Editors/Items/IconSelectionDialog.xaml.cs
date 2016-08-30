﻿using System.Collections.Generic;
using System.Windows;
using WorldEditor.Loaders.Icons;

namespace WorldEditor.Editors.Items
{
    /// <summary>
    /// Interaction logic for IconSelectionDialog.xaml
    /// </summary>
    public partial class IconSelectionDialog : Window
    {
        public IconSelectionDialog()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty SelectedIconProperty =
            DependencyProperty.Register("SelectedIcon", typeof(Icon), typeof(IconSelectionDialog), new PropertyMetadata(default(Icon)));

        public Icon SelectedIcon
        {
            get
            {
                return (Icon)GetValue(SelectedIconProperty);
            }
            set { SetValue(SelectedIconProperty, value); }
        }

        public static readonly DependencyProperty IconsSourceProperty =
            DependencyProperty.Register("IconsSource", typeof(IEnumerable<Icon>), typeof(IconSelectionDialog), new PropertyMetadata(default(IEnumerable<Icon>)));

        public IEnumerable<Icon> IconsSource
        {
            get { return (IEnumerable<Icon>)GetValue(IconsSourceProperty); }
            set { SetValue(IconsSourceProperty, value); }
        }

        private void OnButtonOKClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void OnButtonCancelClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}