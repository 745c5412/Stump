using Stump.Core.I18N;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace WorldEditor.Helpers.Controls
{
    /// <summary>
    /// Interaction logic for I18NTextBox.xaml
    /// </summary>
    public partial class I18NTextBox : UserControl
    {
        private List<Languages> m_availableLanguages;

        public I18NTextBox()
        {
            InitializeComponent();
            m_availableLanguages = Enum.GetValues(typeof(Languages)).OfType<Languages>().ToList();
        }

        public List<Languages> AvailableLanguages
        {
            get { return m_availableLanguages; }
        }
    }
}