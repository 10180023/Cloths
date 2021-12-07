using Cloths.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Cloths.Pages
{
    /// <summary>
    /// Логика взаимодействия для PageMaterials.xaml
    /// </summary>
    public partial class PageMaterials : Page
    {
        List<Material> materials = ClothsEntities.GetContext().Material.ToList();

        public PageMaterials()
        {
            InitializeComponent();

            var types = ClothsEntities.GetContext().MaterialType.ToList();
            types.Insert(0, new MaterialType { 
                Title = "Все типы"
            });

            List<string> sort = new List<string>();
            sort.Add("Сортировка");
            sort.Add("По наименованию");
            sort.Add("По остаткам");
            sort.Add("По стоимости");

            cbFilter.ItemsSource = types;
            cbFilter.DisplayMemberPath = "Title";
            cbFilter.SelectedValue = "ID";
            cbFilter.SelectedIndex = 0;

            cbSort.ItemsSource = sort;
            cbSort.SelectedIndex = 0;

            tbFinder.Text = "Поиск по наименованию";

            lvMaterials.ItemsSource = materials;
        }
        /// <summary>
        /// Поиск, сортировка и фильтрация материалов
        /// </summary>
        private void UpdateMaterials()
        {
            materials = ClothsEntities.GetContext().Material.ToList();

            if (cbSort.SelectedIndex > 0)
            {
                switch (cbSort.SelectedIndex)
                {
                    case 1:
                        if (rbAsc.IsChecked == true)
                        {
                            materials = materials.OrderBy(m => m.Title).ToList();
                        }
                        else
                        materials = materials.OrderByDescending(m => m.Title).ToList();
                        break;
                    case 2:
                        if (rbAsc.IsChecked == true)
                        {
                            materials = materials.OrderBy(m => m.CountInStock).ToList();
                        }
                        else
                        materials = materials.OrderByDescending(m => m.CountInStock).ToList();
                        break;
                    case 3:
                        if (rbAsc.IsChecked == true)
                        {
                            materials = materials.OrderBy(m => m.Cost).ToList();
                        }
                        else
                        materials = materials.OrderByDescending(m => m.Cost).ToList();
                        break;
                }
            }

            if (tbFinder.Text != "Поиск по наименованию")
            {
                materials = ClothsEntities.GetContext().Material.ToList();
                materials = materials.Where(m => m.Title.ToLower().Contains(tbFinder.Text.ToLower())).ToList();
            }    
            
            if (cbFilter.SelectedIndex > 0)
            {
                materials = materials.Where(m => m.MaterialType.Equals(cbFilter.SelectedValue)).ToList();
            }

            lvMaterials.ItemsSource = materials;
        }
        /// <summary>
        /// Открытие окна добавления материалов
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new PageAddEdit(null));
        }
        /// <summary>
        /// открытие окна для редактирования записи
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new PageAddEdit((sender as Button).DataContext as Material));
        }
        /// <summary>
        /// Обновление списка материалов
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            ClothsEntities.GetContext().ChangeTracker.Entries().ToList().ForEach(m => m.Reload());
            materials = ClothsEntities.GetContext().Material.ToList();
            lvMaterials.ItemsSource = materials;
        }
        /// <summary>
        /// Сброс поля поиска
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbFinder_GotFocus(object sender, RoutedEventArgs e)
        {
            tbFinder.Text = "";
        }
        /// <summary>
        /// Поиск по наименованию
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbFinder_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateMaterials();
        }
        /// <summary>
        /// Сортировка
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateMaterials();
            if (cbSort.SelectedIndex == 0)
            {
                rbAsc.IsEnabled = false;
                rbDesc.IsEnabled = false;
            }
            else
            {
                rbAsc.IsEnabled = true;
                rbDesc.IsEnabled = true;
            }
        }
        /// <summary>
        /// Фильтрация
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateMaterials();
        }
        /// <summary>
        /// Переключение режима сортировки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateMaterials();
        }
    }
}
