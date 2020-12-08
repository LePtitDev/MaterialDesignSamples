using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using MaterialDesignThemes.Wpf;

namespace ExpandersInListView
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public static readonly DependencyProperty CollectionViewProperty = DependencyProperty.Register(nameof(CollectionView), typeof(ICollectionView), typeof(MainWindow), new PropertyMetadata(null));

        public MainWindow()
        {
            InitializeComponent();

            var list = new List<ItemModel>();
            for (var i = 1; i <= 4; i++)
            {
                for (var j = 1; j <= 10; j++)
                {
                    list.Add(new ItemModel { Name = $"Item {j}", Group = i });
                }
            }

            CollectionView = new CollectionViewSource { Source = list }.View;
            CollectionView.GroupDescriptions.Add(new ItemsGrouping());
        }

        public ICollectionView CollectionView
        {
            get => (ICollectionView) GetValue(CollectionViewProperty);
            set => SetValue(CollectionViewProperty, value);
        }

        private class ItemModel
        {
            public string Name { get; set; }

            public int Group { get; set; }

            public override string ToString()
            {
                return Name;
            }
        }

        private class GroupModel : INotifyPropertyChanged
        {
            private bool _isExpanded = true;

            public string Name { get; set; }

            public PackIconKind Icon { get; set; }

            public string ToolTip { get; set; }

            public bool IsExpanded
            {
                get => _isExpanded;
                set
                {
                    _isExpanded = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsExpanded)));
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;
        }

        public class ItemsGrouping : GroupDescription
        {
            private readonly GroupModel[] _groups =
            {
                new GroupModel { Name = "Group 1", Icon = PackIconKind.Numeric1, ToolTip = "#1" },
                new GroupModel { Name = "Group 2", Icon = PackIconKind.Numeric2, ToolTip = "#2" },
                new GroupModel { Name = "Group 3", Icon = PackIconKind.Numeric3, ToolTip = "#3" },
                new GroupModel { Name = "Group 4", Icon = PackIconKind.Numeric4, ToolTip = "#4" }
            };

            public override object GroupNameFromItem(object item, int level, CultureInfo culture)
            {
                if (item is ItemModel model)
                {
                    return _groups[model.Group - 1];
                }

                return null;
            }
        }
    }
}
