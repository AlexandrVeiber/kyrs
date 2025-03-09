using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using КурсоваяПрог.Интерфейс.склад.запасы_стройматериалов;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace КурсоваяПрог.Интерфейс.склад.Поставки
{
    public partial class AddDeliveryForm : Form
    {
        private List<ConstructionMaterial> materials; // Список строительных материалов
        public Delivery NewDelivery { get; private set; } // Свойство для возвращения новой поставки

        public AddDeliveryForm(List<Supplier> suppliers, Delivery delivery = null)
        {
            InitializeComponent(); // Инициализация компонентов формы
            materials = new List<ConstructionMaterial>(); // Инициализация списка материалов
            LoadSuppliers(suppliers); // Загружаем поставщиков в список
            InitializeDataGridView(); // Инициализация DataGridView для отображения материалов
            InitializeUnitComboBox(); // Инициализация ComboBox для единиц измерения

            // Подключение обработчика события
            dataGridViewMaterials.CellClick += dataGridViewMaterials_CellClick; // Подписка на событие клика по ячейке DataGridView

            if (delivery != null) // Если передана поставка для редактирования
            {
                LoadDeliveryData(delivery); // Загружаем данные поставки
            }
        }

        private void LoadDeliveryData(Delivery delivery) // Метод для загрузки данных поставки
        {
            var selectedSupplier = delivery.Supplier; // Получаем поставщика из поставки
            if (selectedSupplier != null) // Если поставщик не равен null
            {
                listBoxSuppliers.SelectedItem = selectedSupplier; // Устанавливаем выбранного поставщика
            }
            dateTimePickerDeliveryDate.Value = delivery.DeliveryDate; // Устанавливаем дату поставки
            materials = delivery.ReceivedMaterials ?? new List<ConstructionMaterial>(); // Загружаем полученные материалы или создаем новый список
            UpdateMaterialsGrid(); // Обновляем отображение материалов в DataGridView
        }

        private void InitializeUnitComboBox() // Метод для инициализации ComboBox единиц измерения
        {
            comboBoxUnit.Items.AddRange(new[] { "кг", "тонна", "шт" }); // Добавляем единицы измерения
            comboBoxUnit.SelectedIndex = 0; // Устанавливаем значение по умолчанию
        }

        private void LoadSuppliers(List<Supplier> suppliers) // Метод для загрузки поставщиков в список
        {
            listBoxSuppliers.DataSource = suppliers; // Устанавливаем источник данных для списка поставщиков
            listBoxSuppliers.DisplayMember = "Name"; // Указываем, какое свойство отображать
            listBoxSuppliers.ValueMember = "Id"; // Указываем, какое свойство использовать в качестве значения
        }





        private void InitializeDataGridView()
        {
            dataGridViewMaterials.AutoGenerateColumns = false;
            dataGridViewMaterials.Columns.Clear();
            dataGridViewMaterials.Columns.Add(new DataGridViewTextBoxColumn { Name = "Name", HeaderText = "Название", DataPropertyName = "Name" });
            dataGridViewMaterials.Columns.Add(new DataGridViewTextBoxColumn { Name = "Unit", HeaderText = "Единица измерения", DataPropertyName = "Unit" });
            dataGridViewMaterials.Columns.Add(new DataGridViewTextBoxColumn { Name = "Quantity", HeaderText = "Количество", DataPropertyName = "Quantity" });
            dataGridViewMaterials.Columns.Add(new DataGridViewTextBoxColumn { Name = "Price", HeaderText = "Закупочная цена", DataPropertyName = "Price" });
        }

        private void UpdateMaterialsGrid() // Метод для обновления отображения материалов в DataGridView
        {
            dataGridViewMaterials.DataSource = null; // Сбрасываем источник данных
            dataGridViewMaterials.DataSource = materials; // Устанавливаем новый источник данных
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void buttonSaveDelivery_Click(object sender, EventArgs e)
        {
            if (listBoxSuppliers.SelectedItem == null)
            {
                MessageBox.Show("Пожалуйста, выберите поставщика.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (materials.Count == 0)
            {
                MessageBox.Show("Список материалов не может быть пустым.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Проверка, редактируется ли существующий материал
            if (dataGridViewMaterials.SelectedRows.Count > 0)
            {
                var selectedMaterial = (ConstructionMaterial)dataGridViewMaterials.SelectedRows[0].DataBoundItem;

                // Проверка на заполнение полей
                if (!ValidateMaterialInputs())
                    return;

                // Обновление выбранного материала
                selectedMaterial.Name = textBoxMaterialName.Text.Trim();
                selectedMaterial.Unit = comboBoxUnit.SelectedItem.ToString();
                selectedMaterial.Quantity = Convert.ToDouble(numericUpDownMaterialQuantity.Value);
                selectedMaterial.Price = (double)numericUpDownPrice.Value;

                UpdateMaterialsGrid(); // Обновляем отображение в DataGridView
                ClearMaterialFields(); // Очищаем поля ввода
            }

            // Сохранение новой поставки
            NewDelivery = new Delivery
            {
                Supplier = (Supplier)listBoxSuppliers.SelectedItem,
                DeliveryDate = dateTimePickerDeliveryDate.Value,
                ReceivedMaterials = materials
            };

            DialogResult = DialogResult.OK;
            Close();
        }


        private void buttonAddMaterial_Click(object sender, EventArgs e)
        {
            if (!ValidateMaterialInputs()) // Проверка на валидность полей
                return; // Выход из метода

            var selectedSupplier = (Supplier)listBoxSuppliers.SelectedItem; // Получаем выбранного поставщика

            var material = new ConstructionMaterial // Создаем новый объект материала
            {
                Supplier = selectedSupplier, // Устанавливаем поставщика
                Name = textBoxMaterialName.Text.Trim(), // Убираем пробелы в начале и конце
                Unit = comboBoxUnit.SelectedItem.ToString(), // Устанавливаем единицу измерения
                Quantity = Convert.ToDouble(numericUpDownMaterialQuantity.Value), // Устанавливаем количество
                Price = (double)numericUpDownPrice.Value // Устанавливаем цену
            };

            materials.Add(material); // Добавляем материал в список
            UpdateMaterialsGrid(); // Обновляем отображение в DataGridView
            ClearMaterialFields(); // Очищаем поля ввода
        }

        private bool ValidateMaterialInputs()
        {
            if (string.IsNullOrWhiteSpace(textBoxMaterialName.Text))
            {
                MessageBox.Show("Название материала не может быть пустым или содержать только пробелы.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (comboBoxUnit.SelectedItem == null)
            {
                MessageBox.Show("Пожалуйста, выберите единицу измерения.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (numericUpDownPrice.Value <= 0)
            {
                MessageBox.Show("Введите корректное значение для цены (больше нуля).", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (numericUpDownMaterialQuantity.Value <= 0)
            {
                MessageBox.Show("Количество должно быть больше нуля.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }



        private void dataGridViewMaterials_CellClick(object sender, DataGridViewCellEventArgs e) // Обработчик события клика по ячейке DataGridView
        {
            if (e.RowIndex >= 0 && e.RowIndex < materials.Count) // Проверка на допустимый индекс
            {
                var selectedMaterial = materials[e.RowIndex]; // Получаем выбранный материал
                FillMaterialFields(selectedMaterial); // Заполняем текстовые поля данными выбранного материала
            }
        }

        private void FillMaterialFields(ConstructionMaterial material) // Метод для заполнения полей ввода данными материала
        {
            textBoxMaterialName.Text = material.Name; // Устанавливаем имя материала
            comboBoxUnit.SelectedItem = material.Unit; // Устанавливаем выбранную единицу измерения
            numericUpDownMaterialQuantity.Value = (decimal)material.Quantity; // Устанавливаем количество
            numericUpDownPrice.Value = (decimal)material.Price; // Устанавливаем цену
        }

        private void ClearMaterialFields() // Метод для очистки полей ввода
        {
            textBoxMaterialName.Text = string.Empty; // Очищаем имя материала
            comboBoxUnit.SelectedIndex = 0; // Сбрасываем выбор единицы измерения
            numericUpDownPrice.Value = 0; // Сбрасываем цену
            numericUpDownMaterialQuantity.Value = 0; // Сбрасываем количество
        }
        private void buttonEditMaterial_Click(object sender, EventArgs e)
        {
            if (dataGridViewMaterials.SelectedRows.Count > 0)
            {
                var selectedMaterial = (ConstructionMaterial)dataGridViewMaterials.SelectedRows[0].DataBoundItem;

                // Проверка на заполнение полей
                if (string.IsNullOrWhiteSpace(textBoxMaterialName.Text))
                {
                    MessageBox.Show("Название материала не может быть пустым или содержать только пробелы.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (comboBoxUnit.SelectedItem == null)
                {
                    MessageBox.Show("Пожалуйста, выберите единицу измерения.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (numericUpDownPrice.Value <= 0)
                {
                    MessageBox.Show("Введите корректное значение для цены (больше нуля).", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (numericUpDownMaterialQuantity.Value <= 0)
                {
                    MessageBox.Show("Количество должно быть больше нуля.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Обновление выбранного материала
                selectedMaterial.Name = textBoxMaterialName.Text.Trim(); // Убираем пробелы в начале и конце
                selectedMaterial.Unit = comboBoxUnit.SelectedItem.ToString(); // Обновляем единицу измерения
                selectedMaterial.Quantity = Convert.ToDouble(numericUpDownMaterialQuantity.Value);
                selectedMaterial.Price = (double)numericUpDownPrice.Value; // Обновляем цену

                UpdateMaterialsGrid(); // Обновляем отображение в DataGridView
                ClearMaterialFields(); // Очищаем поля ввода
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите материал для редактирования.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void btnDeleteMaterial_Click(object sender, EventArgs e)
        {
            if (dataGridViewMaterials.SelectedRows.Count > 0) // Если выбрана строка в DataGridView
            {
                var selectedMaterial = (ConstructionMaterial)dataGridViewMaterials.SelectedRows[0].DataBoundItem; // Получаем выбранный материал
                materials.Remove(selectedMaterial); // Удаляем материал из списка
                UpdateMaterialsGrid(); // Обновляем отображение в DataGridView
                MessageBox.Show("Материал успешно удален.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information); // Сообщение об успешном удалении
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите материал для удаления.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning); // Сообщение об ошибке
            }
        }


        private void btnEditMaterial_Click(object sender, EventArgs e)
        {
            if (dataGridViewMaterials.SelectedRows.Count > 0) // Если выбрана строка в DataGridView
            {
                var selectedMaterial = (ConstructionMaterial)dataGridViewMaterials.SelectedRows[0].DataBoundItem; // Получаем выбранный материал
                FillMaterialFields(selectedMaterial); // Заполняем текстовые поля данными выбранного материала
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите материал для редактирования.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning); // Сообщение об ошибке
            }
        }
    }

}

