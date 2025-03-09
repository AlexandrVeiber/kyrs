using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace КурсоваяПрог.Интерфейс.склад.запасы_стройматериалов
{
    public partial class AddMaterialForm : Form
    {
        public ConstructionMaterial NewMaterial { get; private set; } // Свойство для возвращения нового материала
        private int constructionObjectId; // Хранение идентификатора строительного объекта

        public AddMaterialForm(List<Supplier> suppliers, ConstructionMaterial material = null) // Конструктор класса, принимающий список поставщиков и материал для редактирования
        {
            InitializeComponent(); // Инициализация компонентов формы
            comboBoxSupplier.DataSource = suppliers; // Устанавливаем источник данных для ComboBox поставщиков
            comboBoxSupplier.DisplayMember = "Name"; // Отображаем имя поставщика

            // Инициализация ComboBox для единиц измерения
            InitializeUnitComboBox(); // Вызываем метод для инициализации единиц измерения


            if (material != null) // Если передан материал для редактирования
            {
                // Заполняем поля данными материала
                textBoxName.Text = material.Name; // Устанавливаем имя материала
                comboBoxUnit.SelectedItem = material.Unit; // Устанавливаем выбранную единицу измерения
                numericUpDownQuantity.Value = (decimal)material.Quantity; // Устанавливаем количество
                numericUpDownPrice.Value = (decimal)material.Price; // Устанавливаем цену
                numericUpDownStock.Value = (decimal)material.Stock; // Устанавливаем остаток

                // Проверка на null для поставщика
                if (material.Supplier != null) // Если у материала есть поставщик
                {
                    comboBoxSupplier.SelectedItem = suppliers.FirstOrDefault(s => s.Id == material.Supplier.Id); // Устанавливаем выбранного поставщика по Id
                }
            }
        }

        private void InitializeUnitComboBox()
        {
            comboBoxUnit.Items.Add("кг");
            comboBoxUnit.Items.Add("тонны");
            comboBoxUnit.Items.Add("шт"); // Добавьте другие единицы измерения по необходимости
            comboBoxUnit.SelectedIndex = 0; // Устанавливаем значение по умолчанию
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            // Проверка на пустые значения
            if (string.IsNullOrWhiteSpace(textBoxName.Text))
            {
                MessageBox.Show("Название материала не может быть пустым или содержать только пробелы.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Проверка, что название материала не состоит только из цифр
            if (textBoxName.Text.Trim().All(char.IsDigit))
            {
                MessageBox.Show("Название материала не может состоять только из цифр.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (comboBoxUnit.SelectedItem == null)
            {
                MessageBox.Show("Пожалуйста, выберите единицу измерения.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var selectedSupplier = (Supplier)comboBoxSupplier.SelectedItem;

            if (selectedSupplier == null)
            {
                MessageBox.Show("Пожалуйста, выберите поставщика.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Проверка на допустимые значения для количества, цены и остатка
            if (numericUpDownQuantity.Value <= 0)
            {
                MessageBox.Show("Количество должно быть больше нуля.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (numericUpDownPrice.Value < 0)
            {
                MessageBox.Show("Цена не может быть отрицательной.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (numericUpDownStock.Value < 0)
            {
                MessageBox.Show("Остаток на складе не может быть отрицательным.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            NewMaterial = new ConstructionMaterial(
                textBoxName.Text.Trim(), // Убираем пробелы в начале и конце
                comboBoxUnit.SelectedItem.ToString(), // Получаем выбранную единицу измерения
                (double)numericUpDownQuantity.Value, // Получаем количество
                (double)numericUpDownPrice.Value, // Получаем цену
                (double)numericUpDownStock.Value, // Получаем остаток
                selectedSupplier // Передаем объект поставщика
            );


            DialogResult = DialogResult.OK;
            Close();
        }


        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel; // Устанавливаем результат диалога как "Отмена"
            Close();
        }

    }
}
