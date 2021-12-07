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
    /// Логика взаимодействия для PageAddEdit.xaml
    /// </summary>
    public partial class PageAddEdit : Page
    {
        private Material currentMaterial = new Material();

        public PageAddEdit(Material selectedMaterial)
        {
            InitializeComponent();

            if (selectedMaterial != null)
            {
                currentMaterial = selectedMaterial;
            }

            DataContext = currentMaterial;

            cbType.ItemsSource = ClothsEntities.GetContext().MaterialType.ToList();
        }
        /// <summary>
        /// Проверка введенных данных, добавление или сохранение изменений записи
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(tbTitle.Text))
                errors.AppendLine("Введите наименование");
            if (int.Parse(tbInPack.Text.ToString()) <= 0)
                errors.AppendLine("Введите кол-во в упаковке");
            if (string.IsNullOrWhiteSpace(tbUnit.Text))
                errors.AppendLine("Введите ед. измерения");
            if (tbInStock.Text.ToString() == "0")
                errors.AppendLine("Введите кол-во на складе");
            if (tbMin.Text.ToString() == "0")
                errors.AppendLine("Введите мин. кол-во");
            if (tbCost.Text.ToString() == "0")
                errors.AppendLine("Введите стоимость");
            if (string.IsNullOrWhiteSpace(tbInStock.Text))
                currentMaterial.Image = null;
            if (cbType.SelectedItem == null)
                errors.AppendLine("Выберите тип");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString(), "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (currentMaterial.ID == 0)
            {
                currentMaterial.MaterialType =  (MaterialType)cbType.SelectedItem;
                ClothsEntities.GetContext().Material.Add(currentMaterial);
            }

            try
            {
                currentMaterial.MaterialType = (MaterialType)cbType.SelectedItem;
                ClothsEntities.GetContext().SaveChanges();
                Manager.MainFrame.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        /// <summary>
        /// Отмена ввода изменений
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.GoBack();
        }
        /// <summary>
        /// Удаление выбранной записи
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы хотите удалить запись?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    ClothsEntities.GetContext().Material.Remove(currentMaterial);
                    ClothsEntities.GetContext().SaveChanges();
                    MessageBox.Show("Запись удалена", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                    Manager.MainFrame.GoBack();
                }
                catch (Exception)
                {
                    MessageBox.Show("Невозможно удалить не созданную запись", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }
    }
}
