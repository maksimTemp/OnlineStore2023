using OnlineStore2023.Commands;
using OnlineStore2023.DataContext;
using OnlineStore2023.DataContext.Models;
using OnlineStore2023.Model;
using OnlineStore2023.Utilities;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace OnlineStore2023.ViewModel
{
    internal class ProductListVM : INotifyPropertyChanged
    {
        #region Fields
        private MainWindowVM _mainVM;
        private ObservableCollection<object> _filters;
        private ObservableCollection<ProductListModel> _products;
        private ObservableCollection<SortingCategoryModel> _sortingList;
        private ObservableCollection<Model.ComboBoxItemModel> _showingItems;
        private string _selectedSortingName;
        private string _selectedShowingNumber;
        private Visibility _loadingScreen;
        private string _currentSearch;
        private Visibility _searchVisibility;
        private ObservableCollection<Tuple<int, SimpleListModel>> _navList;
        private uint? _currentCategoryId;
        private Visibility _leftVisibility;
        private Visibility _rightVisibility;
        private int _currentPage;
        private int _countProducts;
        private ObservableCollection<SpecifitationModel> _activeFilters;
        #endregion

        #region Properties
        public int CurrentPage
        {
            get { return _currentPage + 1; }
            set
            {
                _currentPage = value;
                OnPropertyChanged("CurrentPage");
            }
        }
        public Visibility LeftVisibility
        {
            get { return _leftVisibility; }
            set
            {
                if (value == _leftVisibility) return;

                _leftVisibility = value;
                OnPropertyChanged("LeftVisibility");
            }
        }
        public Visibility RightVisibility
        {
            get { return _rightVisibility; }
            set
            {
                if (value == _rightVisibility) return;

                _rightVisibility = value;
                OnPropertyChanged("RightVisibility");
            }
        }
        public ObservableCollection<Tuple<int, SimpleListModel>> NavList
        {
            get { return _navList; }
            set
            {
                if (value == _navList) return;

                _navList = value;
                OnPropertyChanged("NavList");
            }
        }
        public string AdditionalInfoText { get; private set; }
        public Visibility AdditionalInfoVisibility { get; private set; }
        public uint? CurrentCategoryId
        {
            get { return _currentCategoryId; }
            set
            {
                if (value == _currentCategoryId) return;

                _currentCategoryId = value;
                OnPropertyChanged("CurrentCategoryId");
            }
        }
        public Visibility SearchVisibility
        {
            get { return _searchVisibility; }
            set
            {
                if (value == _searchVisibility) return;

                _searchVisibility = value;
                OnPropertyChanged("SearchVisibility");
            }
        }
        public string CurrentSearch
        {
            get { return _currentSearch; }
            set
            {
                if (value == _currentSearch) return;

                _currentSearch = value;
                OnPropertyChanged("CurrentSearch");

                if (_currentSearch == null) SearchVisibility = Visibility.Collapsed;
                else SearchVisibility = Visibility.Visible;
            }
        }
        public Visibility LoadingScreen
        {
            get { return _loadingScreen; }
            set
            {
                if (value == _loadingScreen) return;

                _loadingScreen = value;
                OnPropertyChanged("LoadingScreen");
            }
        }
        public string SelectedShowingNumber
        {
            get { return _selectedShowingNumber; }
            set
            {
                if (value == _selectedShowingNumber) return;

                _selectedShowingNumber = value;
                OnPropertyChanged("SelectedShowingNumber");
            }
        }
        public ObservableCollection<Model.ComboBoxItemModel> ShowingItems
        {
            get { return _showingItems; }
            set
            {
                if (value == _showingItems) return;

                _showingItems = value;
                OnPropertyChanged("ShowingItems");
            }
        }
        public ObservableCollection<SortingCategoryModel> SortingList
        {
            get { return _sortingList; }
            set
            {
                if (value == _sortingList) return;

                _sortingList = value;
                OnPropertyChanged("SortingList");
            }
        }
        public string SelectedSortingName
        {
            get { return _selectedSortingName; }
            set
            {
                if (value == _selectedSortingName) return;

                _selectedSortingName = value;
                OnPropertyChanged("SelectedSortingName");
            }
        }
        public ObservableCollection<object> Filters
        {
            get { return _filters; }
            set
            {
                if (value == _filters) return;

                _filters = value;
                OnPropertyChanged("Filters");
            }
        }
        public ObservableCollection<ProductListModel> Products
        {
            get { return _products; }
            set
            {
                if (value == _products) return;

                _products = value;
                OnPropertyChanged("Products");
            }
        }
        public ObservableCollection<SpecifitationModel> ActiveFilters
        {
            get { return _activeFilters; }
            set
            {
                if (value == _activeFilters) return;

                _activeFilters = value;
                OnPropertyChanged("ActiveFilters");
            }
        }
        #endregion

        #region Commands
        public SimpleCommand FilterCommand { get; set; }
        public SimpleCommand SelectedItemChangedCommand { get; set; }
        public SimpleCommand HomeCommand { get; set; }
        public ParameterCommand SwitchPageCommand { get; set; }
        public ParameterCommand DeleteFilterCommand { get; set; }
        public ParameterCommand ChangeCategoryCommand { get; set; }
        public ParameterCommand ShowProductCommand { get; set; }
        #endregion

        public ProductListVM(MainWindowVM mvm, string search = null)
        {
            _mainVM = mvm;
            SelectedItemChangedCommand = new SimpleCommand(ProcessSortingChange);
            SwitchPageCommand = new ParameterCommand(SetPage);
            ShowProductCommand = new ParameterCommand(ShowProduct);

            CurrentSearch = search;

            FullReload();
        }

        private void ShowProduct(object param)
        {
            var obj = param as ProductListModel;
            _mainVM.LoadProduct(obj.ProductID);
        }
        private void FullReload()
        {
            CurrentPage = 0;
            SelectedShowingNumber = "15";
            SearchVisibility = Visibility.Collapsed;
            CreateShowingItemsComboBox();
            CreateSortingList();

            Action mainLoad = () =>
            {
                SetAdditionalInfo(null);
                LoadProductsFromDB();
                SetPageNavVisibility();
            };
            LoadingScreenProcess(mainLoad);
        }

        private void SetPage(object param)
        {
            if (param == null) return;

            string direction = param.ToString();

            if (direction.ToLower() == "left")
            {
                if (_currentPage == 0) return;
                CurrentPage = _currentPage - 1;
            }
            else
            {
                if (_currentPage == Math.Ceiling(_countProducts / float.Parse(_selectedShowingNumber))) return;
                CurrentPage = _currentPage + 1;
            }

            SetPageNavVisibility();
            LoadingScreenProcess(LoadProductsFromDB);
        }
        private void SetPageNavVisibility()
        {
            LeftVisibility = Visibility.Visible;
            RightVisibility = Visibility.Visible;

            if (_currentPage == 0) LeftVisibility = Visibility.Hidden;
            if (_selectedShowingNumber == null) SelectedShowingNumber = "15";
            if (CurrentPage == Math.Ceiling(_countProducts / float.Parse(_selectedShowingNumber)) || _countProducts <= float.Parse(_selectedShowingNumber)) RightVisibility = Visibility.Hidden;
        }
        private void SetAdditionalInfo(string text)
        {
            if (text == null)
            {
                AdditionalInfoVisibility = Visibility.Collapsed;
                AdditionalInfoText = string.Empty;
            }
            else
            {
                AdditionalInfoVisibility = Visibility.Visible;
                AdditionalInfoText = text;
            }

            OnPropertyChanged("AdditionalInfoVisibility");
            OnPropertyChanged("AdditionalInfoText");
        }
        private void CreateSortingList()
        {
            SortingList = new ObservableCollection<SortingCategoryModel>()
            {
                new SortingCategoryModel()
                {
                    Name = "Название: A-Z",
                    IsSelected = true,
                    Sort = new SortDescription("name", ListSortDirection.Ascending)
                },
                new SortingCategoryModel()
                {
                    Name = "Название: Z-A",
                    IsSelected = false,
                    Sort = new SortDescription("name", ListSortDirection.Descending)
                },
                new SortingCategoryModel()
                {
                    Name = "Цена: от самой высокой",
                    IsSelected = false,
                    Sort = new SortDescription("price", ListSortDirection.Descending)
                },
                new SortingCategoryModel()
                {
                    Name = "Цена: от самой низкой",
                    IsSelected = false,
                    Sort = new SortDescription("price", ListSortDirection.Ascending)
                }
            };
        }

        private void CreateShowingItemsComboBox()
        {
            ShowingItems = new ObservableCollection<Model.ComboBoxItemModel>()
            {
                new Model.ComboBoxItemModel()
                {
                    Name = "15",
                    IsSelected = true,
                    Value = 15
                },
                new Model.ComboBoxItemModel()
                {
                    Name = "30",
                    IsSelected = false,
                    Value = 30
                },
                new Model.ComboBoxItemModel()
                {
                    Name = "45",
                    IsSelected = false,
                    Value = 45
                },
                new Model.ComboBoxItemModel()
                {
                    Name = "90",
                    IsSelected = false,
                    Value = 90
                }
            };
            SelectedShowingNumber = "15";
        }
        private void ProcessSortingChange()
        {
            CurrentPage = 0;
            SetPageNavVisibility();
            LoadingScreenProcess(LoadProductsFromDB);
        }
        private void LoadingScreenProcess(Action action)
        {
            LoadingScreen = Visibility.Visible;

            Task.Factory.StartNew(action).ContinueWith((Task) =>
            {
                LoadingScreen = Visibility.Collapsed;
            });
        }
        private void LoadProductsFromDB()
        {
            try
            {
                var dataContext = new OnlineShop2022Context();
                IQueryable<Product> query = dataContext.Products;

                //Get products from name
                if (CurrentSearch != null)
                {
                    if (query != null)
                        query = from l in query
                                where l.ProductName.ToLower().Contains(CurrentSearch.ToLower())
                                select l;
                    else
                        query = from l in dataContext.Products
                                where l.ProductName.ToLower().Contains(CurrentSearch.ToLower())
                                select l;
                }
                if (query == null)
                    query = from l in dataContext.Products
                            select l;

                //Sort list
                SortingCategoryModel selected = new SortingCategoryModel();
                if (SelectedSortingName != null)
                    selected = SortingList.FirstOrDefault(s => s.Name == SelectedSortingName);
                switch (selected.Sort.PropertyName)
                {
                    case "name":
                        if (selected.Sort.Direction == ListSortDirection.Ascending)
                            query = from q in query
                                    orderby q.ProductName ascending
                                    select q;
                        else
                            query = from q in query
                                    orderby q.ProductName descending
                                    select q;
                        break;
                    case "price":
                        if (selected.Sort.Direction == ListSortDirection.Ascending)
                            query = from q in query
                                    orderby q.ProductPrice ascending
                                    select q;
                        else
                            query = from q in query
                                    orderby q.ProductPrice descending
                                    select q;
                        break;
                    default:
                        query = from q in query
                                orderby q.ProductId
                                select q;
                        break;
                }

                //Page processing
                _countProducts = query.Count();
                var list = query.Skip(_currentPage * int.Parse(SelectedShowingNumber)).Take(int.Parse(SelectedShowingNumber));

                //Mapping into object
                var tmp = new ObservableCollection<ProductListModel>();
                foreach (var e in list)
                {
                    var p = new ProductListModel()
                    {
                        Name = e.ProductName,
                        Price = e.ProductPrice,
                        //image = e.ProductPhoto,
                        ProductID = e.ProductId
                    };
                    p.SetSpecification(e.ProductId);
                    tmp.Add(p);
                }

                Products = tmp;

                //0 element list
                if (tmp.Count == 0) SetAdditionalInfo("Не найдено продуктов, соответствующих вашим критериям поиска");
                else SetAdditionalInfo(null);
            }
            catch (Exception e)
            {
                string mess = "При загрузке списка продуктов произошла ошибка!\n";
                StandardMessages.Error(mess + e.Message);
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(String info)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(info));
        }
    }
}
