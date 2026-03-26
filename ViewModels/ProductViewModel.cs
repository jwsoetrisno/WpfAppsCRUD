using ClosedXML.Excel;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Windows.Input;
using WpfApps.Helper;
using WpfApps.Interface;
using WpfApps.Models;

namespace WpfApps.ViewModels;

public class ProductViewModel : BaseViewModel
{
    private readonly IRepository<Product> _productRepository;

    public ObservableCollection<Product> Products { get; set; } = new();
    private Product _selectedProduct;

    public Product SelectedProduct
    {
        get => _selectedProduct;
        set
        {
            _selectedProduct = value;
            OnPropertyChanged();
        }
    }

    public ICommand AddCommand { get; }
    public ICommand UpdateCommand { get; }
    public ICommand DeleteCommand { get; }
    public ICommand LoadCommand { get; }

    public ICommand ExportCommand { get; }
    public ICommand ImportCommand { get; }

    public ProductViewModel(IRepository<Product> productRepository)
    {
        _productRepository = productRepository;

        AddCommand = new RelayCommand(async _ => await AddProduct());
        UpdateCommand = new RelayCommand(async _ => await UpdateProduct());
        DeleteCommand = new RelayCommand(async _ => await DeleteProduct());
        LoadCommand = new RelayCommand(async _ => await LoadProducts());
        ExportCommand = new RelayCommand(async _ => await ExportToXLS());
        ImportCommand = new RelayCommand(async _ => await ImportXLS());
    }

    private async Task LoadProducts()
    {
        Products.Clear();
        var products = await _productRepository.GetAllAsync();
        foreach (var product in products)
            Products.Add(product);
    }

    private async Task AddProduct()
    {
        if (SelectedProduct != null)
        {
            if (SelectedProduct.Name != null &&
                SelectedProduct.SKU != null)
            {
                await _productRepository.AddAsync(SelectedProduct);
                await LoadProducts();
            }
        }
    }

    private async Task UpdateProduct()
    {
        if (SelectedProduct != null)
        {
            await _productRepository.UpdateAsync(SelectedProduct);
            await LoadProducts();
        }
    }

    private async Task DeleteProduct()
    {
        if (SelectedProduct != null)
        {
            await _productRepository.DeleteAsync(SelectedProduct.Id);
            await LoadProducts();
        }
    }

    private async Task ExportToXLS()
    {
        string fileName = "Product.xlsx";
        if (Products.Count != 0 && fileName != null)
        {
            await Task.Run(() =>
            {
                FileHelper.WriteXLS(Products, fileName);
            });
        }
    }

    private async Task ImportXLS()
    {
        List<Product> newProducts = [];
        await Task.Run(() => 
        { 
            newProducts = FileHelper.ImportXLS(); 
            foreach (var prd in newProducts)
            {
                SelectedProduct = prd;
                using var _ = AddProduct();
            }            
        });

        using var _ = LoadProducts();
    }
}